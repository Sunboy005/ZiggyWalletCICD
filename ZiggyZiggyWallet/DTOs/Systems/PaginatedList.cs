using System.Collections.Generic;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.DTOs.Systems
{
    public class PaginatedList<T>
    {
        public PageMeta MetaData { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedList()
        {
            Data = new List<T>();
        }
    }
}
