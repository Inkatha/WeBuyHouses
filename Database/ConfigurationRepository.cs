using Microsoft.Extensions.Configuration;

namespace WeBuyHouses.Database
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        readonly IConfiguration _configuration;
        public ConfigurationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string WeBuyHousesConnectionString()
        {
            return _configuration.GetValue<string>("ConnectionStrings:WeBuyHouses");
        }
    }
}