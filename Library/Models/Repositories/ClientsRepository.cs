using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class ClientsRepository : DbContext, IClientsRepository
    {
        public ClientsRepository(string connectionString): base(connectionString)
        {
            
        }
        public int AddClient(Client client)
        {

            string commandText = $"INSERT INTO Clients (" +
                                 $"{nameof(Client.FirstName)}," +
                                 $"{nameof(Client.LastName)}," +
                                 $"{nameof(Client.Phone)}," +
                                 $"{nameof(Client.Email)}" +
                                 $") VALUES (" +
                                 $"@{nameof(Client.FirstName)}," +
                                 $"@{nameof(Client.LastName)}," +
                                 $"@{nameof(Client.Phone)}," +
                                 $"@{nameof(Client.Email)});" +
                                 $"SET @{nameof(Client.Id)}=SCOPE_IDENTITY()";
            SqlCommand command = new SqlCommand(commandText, _connection);

            SetCreateParameters(client, command);
            try
            {
                command.ExecuteNonQuery();
                client.Id = Convert.ToInt32(command.Parameters[$"@{nameof(Client.Id)}"].Value);
            }
            catch
            {
                throw new Exception("Ошибка добавления клиента");
            }
            
            return client.Id;
        }

        public IList<Client> GetClients()
        {
            List<Client> clients = new List<Client>(10);

            string queryText = "Select * FROM Clients";
            SqlCommand command = new SqlCommand(queryText, _connection);
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client()
                        {
                            Id = Convert.ToInt32(reader[$"{nameof(Client.Id)}"]),
                            FirstName = Convert.ToString(reader[$"{nameof(Client.FirstName)}"]),
                            LastName = Convert.ToString(reader[$"{nameof(Client.LastName)}"]),
                            Phone = Convert.ToString(reader[$"{nameof(Client.Phone)}"]),
                            Email = Convert.ToString(reader[$"{nameof(Client.Email)}"])
                        });
                    }
                }
            }
            catch
            {
                throw new Exception("Ошибка выборки данных о клиентах");
            }
            return clients;
        }

        void SetCreateParameters(Client client, SqlCommand command)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(client.FirstName)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = client.FirstName;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(client.LastName)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = client.LastName;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(client.Phone)}";
            parameter.DbType= System.Data.DbType.String;
            parameter.Value = client.Phone;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(client.Email)}";
            parameter.DbType = System.Data.DbType.String;
            parameter.Value = client.Email;
            command.Parameters.Add(parameter);

            parameter = new SqlParameter();
            parameter.ParameterName = $"@{nameof(Client.Id)}";
            parameter.Direction = System.Data.ParameterDirection.Output;
            parameter.DbType = System.Data.DbType.Int32;
            command.Parameters.Add(parameter);
        }
    }
}
