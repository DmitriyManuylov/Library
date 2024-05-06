using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library.Models
{
    public class BooksRepository : DbContext, IBooksRepository
    {
        public BooksRepository(string connectionString) : base(connectionString)
        {

        }

        public int AddBook(Book book)
        {
            string commandText = "Insert INTO Books (" +
                                 $"{nameof(Book.Title)}," +
                                 $"{nameof(Book.Description)}) " +
                                  "VALUES (" +
                                 $"@{nameof(Book.Title)}," +
                                 $"@{nameof(Book.Description)});" +
                                 $"SET @{nameof(Book.Id)}=SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(commandText, _connection);
            SetAddBookParameters(book, command);

            try
            {
                command.ExecuteNonQuery();
                book.Id = Convert.ToInt32(command.Parameters[$"@{nameof(Book.Id)}"].Value);
            }
            catch
            {
                throw new Exception("Ошибка добавления книги");
            }
            return book.Id;
        }

        void SetAddBookParameters(Book book, SqlCommand command)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Title)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = book.Title;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Description)}";
            parameter.DbType= System.Data.DbType.String;
            parameter.Value = book.Description;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Id)}";
            parameter.Direction = System.Data.ParameterDirection.Output;
            parameter.DbType = System.Data.DbType.Int32;
            command.Parameters.Add(parameter);
        }
        public IList<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>(10);
            string queryText = "Select * from Books";
            SqlCommand command = new SqlCommand(queryText, _connection);

            try
            {
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book()
                        {
                            Id = Convert.ToInt32(reader[$"{nameof(Book.Id)}"]),
                            Title = Convert.ToString(reader[$"{nameof(Book.Title)}"]),
                            Description = Convert.ToString(reader[$"{nameof(Book.Description)}"])
                        }); ;
                    }
                }
            }
            catch
            {
                throw new Exception("Ошибка выборки списка книг");
            }
            return books;
        }

        public Book GetBook(int id)
        {
            Book book;

            string queryText = $"Select * from Books where {nameof(Book.Id)}=@{nameof(Book.Id)}";

            SqlCommand command = new SqlCommand(queryText, _connection);
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Id)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = id;
            command.Parameters.Add(parameter);

            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new Exception();

                    book = new Book()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Title = Convert.ToString(reader[1]),
                        Description = Convert.ToString(reader[2])
                    };
                }
            }
            catch
            {
                throw new Exception("Ошибка выборки данных о книге");
            }

            return book;
        }

        public void UpdateBook(Book book)
        {
            string commandText = "EditBookDescription";
            SqlCommand command = new SqlCommand(commandText, _connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SetUpdateBookParameters(book, command);

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Ошибка обновления данных о книге");
            }
        }

        void SetUpdateBookParameters(Book book, SqlCommand command)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Title)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = $"{book.Title}";
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Description)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = $"{book.Description}";
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Id)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = book.Id;
            command.Parameters.Add(parameter);
        }

        public void EditDescription(Book book)
        {
            string commandText = "Update Books SET " +
                                 $"{nameof(Book.Description)}=@{nameof(Book.Description)} " +
                                 $"where {nameof(Book.Id)}=@{nameof(Book.Id)}";

            SqlCommand command = new SqlCommand(commandText, _connection);
            SetEditDescriptionParameters(book, command);

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Ошибка обновления данных о книге");
            }
        }

        void SetEditDescriptionParameters(Book book, SqlCommand command)
        {
            SqlParameter parameter = new SqlParameter();
            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Description)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = $"{book.Description}";
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Book.Id)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = book.Id;
            command.Parameters.Add(parameter);
        }

    }
}