using System.Collections.Generic;
using System.Linq;

namespace EdmsMockApi.Converters
{
    public class ApiList<T> : List<T>
    {
        public int PageIndex { get; }
        public int PageSize { get; }

        public ApiList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }
    }
}