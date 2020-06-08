using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WeBuyHouses.Database;
using WeBuyHouses.Models;

namespace WeBuyHouses.DataAccess
{
    public class CustomerRepository : ICustomerRepository
    {
        readonly IConfigurationRepository _configuration;
        public CustomerRepository(IConfigurationRepository configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<int> CreateNewCustomer(Customer customer)
        {
            var sql = @"INSERT INTO Customer(PhoneNumber, DealFinderCode, City, State, Zip, EmailAddress, FirstName, LastName, isClosed, DateCreated, DateModified) 
                VALUES (@phoneNumber, @dealFinderCode, @city, @state, @zip, @emailAddress, @firstName, @lastName, 0, SYSDATE(), SYSDATE());
                SELECT LAST_INSERT_ID();";

            var parameters = new DynamicParameters();
            parameters.Add("@phoneNumber", customer.PhoneNumber);
            parameters.Add("@dealFinderCode", customer.DealFinderCode);
            parameters.Add("@city", customer.City);
            parameters.Add("@state", customer.State);
            parameters.Add("@zip", customer.Zip);
            parameters.Add("@emailAddress", customer.EmailAddress);
            parameters.Add("@firstName", customer.FirstName);
            parameters.Add("@lastName", customer.LastName);

            using (IDbConnection connection = new MySqlConnection(_configuration.WeBuyHousesConnectionString()))
            {
                connection.Open();
                return await connection.QueryFirstAsync<int>(sql, parameters);
            }
        }        
    }
}