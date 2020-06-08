using System.Threading.Tasks;
using WeBuyHouses.Models;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Linq;
using WeBuyHouses.Database;

namespace WeBuyHouses.DataAccess
{
    public class DealFinderRepository : IDealFinderRepository
    {
        readonly IConfigurationRepository _configuration;
        public DealFinderRepository(IConfigurationRepository configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<string>> GetAllDealFinderCodes()
        {
            var sql = "SELECT DealFinderCode FROM DealFinder";
            using (IDbConnection connection = new MySqlConnection(_configuration.WeBuyHousesConnectionString()))
            {
                connection.Open();
                return await connection.QueryAsync<string>(sql);
            }
        }

        public async Task<DealFinder> GetDealFinder(string dealFinderCode)
        {
            var sql = "SELECT * FROM DealFinder WHERE DealFinderCode = @dealFinderCode";
            using (IDbConnection connection = new MySqlConnection(_configuration.WeBuyHousesConnectionString()))
            {
                connection.Open();
                var dealFinder = await connection.QueryFirstAsync<DealFinder>(sql, new { dealFinderCode = dealFinderCode});
                return dealFinder;
            }
        }
    }
}