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
        public int TotalRecords { get; set; }
        public int RecordsReturned { get { return Count; } }
    }
}