using System.Data;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using WeBuyHouses.Database;
using WeBuyHouses.Models;

namespace WeBuyHouses.DataAccess
{
    public class MessageRepository : IMessageRepository
    {
        readonly IConfigurationRepository _configuration;
        public MessageRepository(IConfigurationRepository configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateNewMessage(Message message)
        {
            
            var sql = @"INSERT INTO Message (FromPhoneNumber, ToPhoneNumber, Body, DateCreated, DateModified) 
            VALUES (@fromPhoneNumber, @toPhoneNumber, @body, SYSDATE(), SYSDATE());
            SELECT LAST_INSERT_ID();";

            var parameters = new DynamicParameters();

            parameters.Add("@fromPhoneNumber", message.FromPhoneNumber);
            parameters.Add("@toPhoneNumber", message.ToPhoneNumber);
            parameters.Add("@body", message.Body);

            using (IDbConnection connection = new MySqlConnection(_configuration.WeBuyHousesConnectionString()))
            {
                connection.Open();
                return await connection.QueryFirstAsync<int>(sql, parameters);
            }
        }
    }
}