using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}
