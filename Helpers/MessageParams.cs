namespace nomo_bucket_api.Helpers
{
    public class MessageParams
    {
        public string MessageType { get; set; } = "received";
        private const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 9;
        public int PageSize

        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }
}