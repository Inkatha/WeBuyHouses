using System.Collections.Generic;
using System.Threading.Tasks;
using WeBuyHouses.Models;

namespace WeBuyHouses.DataAccess
{
    public interface IDealFinderRepository
    {
        Task<IEnumerable<string>> GetAllDealFinderCodes();
        Task<DealFinder> GetDealFinder(string dealFinderCode);
    }
}