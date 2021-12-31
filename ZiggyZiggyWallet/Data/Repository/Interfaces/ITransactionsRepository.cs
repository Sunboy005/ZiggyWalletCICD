using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface ITransactionsRepository:ICRUDRepository
    {
        Task<List<Tranx>> GetTransactionsByWallet(string address);
    }
}
