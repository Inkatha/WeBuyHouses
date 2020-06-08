using System.Threading.Tasks;
using WeBuyHouses.Models;

namespace WeBuyHouses.DataAccess
{
    public interface ICustomerRepository
    {
        Task<int> CreateNewCustomer(Customer customer);
    }
}