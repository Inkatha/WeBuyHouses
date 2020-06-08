using System.Threading.Tasks;
using WeBuyHouses.Models;

namespace WeBuyHouses.DataAccess
{
    public interface IMessageRepository
    {
        Task<int> CreateNewMessage(Message message);
    }
}