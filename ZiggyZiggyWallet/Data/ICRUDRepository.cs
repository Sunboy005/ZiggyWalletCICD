using System.Threading.Tasks;

namespace ZiggyZiggyWallet.Data
{
    public interface ICRUDRepository
    {
        Task<bool> Add<T>(T entity);
        Task<bool> Edit<T>(T entity);
        Task<bool> Delete<T>(T entity);
        Task<bool> SaveChanges();
        Task<int> RowCount();
    }
}
