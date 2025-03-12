namespace MeuDicionarioV2.Core.Paginator
{
    public class QueryParams
    {
        public string OrderBy { get; set; }
        public OrderDirection Direction { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Offset => (Page - 1) * PageSize;
        public QueryParams()
        {
            OrderBy = "Id";
            Direction = OrderDirection.ASC;
        }
    }
}
