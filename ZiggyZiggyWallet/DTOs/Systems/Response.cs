using System.Collections.Generic;

namespace ZiggyZiggyWallet.DTOs.Systems
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<ErrorItem> Errors { get; set; }
        public T Data { get; set; }

        public Response()
        {
            Errors = new List<ErrorItem>();
        }
    }
    public class ErrorItem
    {
        public string Key { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
