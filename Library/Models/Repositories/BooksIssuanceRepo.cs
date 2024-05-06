using Library.Models.ViewModels;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library.Models.Repositories
{
    public class BooksIssuanceRepo : DbContext, IBooksIssuanceRepo
    {

        public BooksIssuanceRepo(string connectionString): base(connectionString)
        {

        }
        public int AddIssuance(IssuanceViewModel issuance)
        {
            Issuance _issuance = new Issuance()
            {
                BookId = issuance.BookId,
                ClientId = issuance.ClientId,
                RequiredReturningDate = issuance.RequiredReturningDate,
                ActualReturningDate = null,
            };
            string commandText = "INSERT INTO Issuancies (" +
                                $"{nameof(Issuance.BookId)}," +
                                $"{nameof(Issuance.ClientId)}," +
                                $"{nameof(Issuance.IssuanceDate)}," +
                                $"{nameof(Issuance.RequiredReturningDate)}," +
                                $"{nameof(Issuance.Status)})" +
                                "Values(" +
                                $"@{nameof(Issuance.BookId)}," +
                                $"@{nameof(Issuance.ClientId)}," +
                                $"@{nameof(Issuance.IssuanceDate)}," +
                                $"@{nameof(Issuance.RequiredReturningDate)}," +
                                $"@{nameof(Issuance.Status)});" +
                                $"SET @{nameof(_issuance.Id)}=SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(commandText, _connection);
            try
            {
                SetAddParameters(_issuance, command);

                command.ExecuteNonQuery();

                _issuance.Id = Convert.ToInt32(command.Parameters["@id"].Value);

            }
            catch
            {
                throw new Exception("Ошибка добавления информации о выдаче книги");
            }
            return _issuance.Id;

        }

        

        public IList<Issuance> GetIssuances()
        {
            List<Issuance> result = new List<Issuance>(10);
            string commandText = "Select " +
                                 $"{nameof(Issuance.Id)}, " +
                                 $"{nameof(Issuance.BookId)}, " +
                                 $"{nameof(Issuance.ClientId)}, " +
                                 $"{nameof(Issuance.IssuanceDate)}, " +
                                 $"{nameof(Issuance.RequiredReturningDate)}, " +
                                 $"{nameof(Issuance.ActualReturningDate)}, " +
                                 $"{nameof(Issuance.Status)} " +
                                 $"from Issuancies";
            SqlCommand command = new SqlCommand( commandText, _connection);
            Issuance issue;
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        issue = new Issuance()
                        {
                            Id = Convert.ToInt32(reader[$"{nameof(Issuance.Id)}"]),
                            BookId = Convert.ToInt32(reader[$"{nameof(Issuance.BookId)}"]),
                            ClientId = Convert.ToInt32(reader[$"{nameof(Issuance.ClientId)}"]),
                            IssuanceDate = Convert.ToDateTime(reader[$"{nameof(Issuance.IssuanceDate)}"]),
                            RequiredReturningDate = Convert.ToDateTime(reader[$"{nameof(Issuance.RequiredReturningDate)}"]),
                            Status = (IssuanceStatus)Convert.ToInt32(reader[$"{nameof(Issuance.Status)}"])
                        };
                        result.Add(issue);
                        if (Convert.IsDBNull(reader[$"{nameof(Issuance.ActualReturningDate)}"]))
                        {
                            issue.ActualReturningDate = null;
                            continue;
                        }
                        issue.ActualReturningDate = Convert.ToDateTime(reader[$"{nameof(Issuance.ActualReturningDate)}"]);


                    }
                }
            }
            catch
            {
                throw new Exception("Ошибка выборки данных о выданных книгах");
            }
            return result;
        }

        public void ReturnBook(int issuanceId)
        {
            string commandText = $"Update Issuancies SET " +
                                 $"{nameof(Issuance.Status)}=1," +
                                 $"{nameof(Issuance.ActualReturningDate)}=@{nameof(Issuance.ActualReturningDate)}" +
                                 $"  where Id=@Id";
            SqlCommand command = new SqlCommand(commandText, _connection);
            SetReturnParameters(issuanceId, command);

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Ошибка изменения статуса выдачи книги");
            }

        }

        private static void SetReturnParameters(int issuanceId, SqlCommand command)
        {
            SqlParameter parameter = command.CreateParameter();
            parameter.ParameterName = $"@{nameof(Issuance.Id)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = issuanceId;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.ActualReturningDate)}";
            parameter.DbType = System.Data.DbType.Date;
            parameter.Value = DateTime.Now;
            command.Parameters.Add(parameter);
        }

        private static void SetAddParameters(Issuance issuance, SqlCommand command)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.BookId)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = issuance.BookId;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName =  $"@{nameof(Issuance.ClientId)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = issuance.ClientId;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);

            DateTime currentDate = DateTime.Now;
            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.IssuanceDate)}";
            parameter.DbType = System.Data.DbType.DateTime;
            parameter.Value = currentDate;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.RequiredReturningDate)}";
            parameter.DbType = System.Data.DbType.DateTime;
            parameter.Value = issuance.RequiredReturningDate;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.Status)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Value = IssuanceStatus.NotReturned;
            parameter.Direction = System.Data.ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Issuance.Id)}";
            parameter.DbType = System.Data.DbType.Int32;
            parameter.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(parameter);

        }

    }
}