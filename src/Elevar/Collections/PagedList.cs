using System.Collections.Generic;

namespace Elevar.Collections
{
    public class PagedList<T> : List<T>
    {
        public PagedList() { }
        public PagedList(IEnumerable<T> items, int totalRecords = 0) : base(items)
        {
            TotalRecords = totalRecords;
        }
        public long TotalRecords { get; set; }
        public long RecordsReturned { get { return Count; } }
        public long GetTotalPages(int pageSize) 
        {
            if (pageSize == 0) return 0;
            if (TotalRecords == 0) return 0;
            return TotalRecords > pageSize ? (TotalRecords / pageSize) + ((TotalRecords % pageSize) > 0 ? 1 : 0) : 1;
        }
    }
}