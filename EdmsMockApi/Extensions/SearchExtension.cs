using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using EdmsMockApi.Converters;
using EdmsMockApi.Helpers;
using ServiceReference;

namespace EdmsMockApi.Extensions
{
    public static class SearchExtension
    {
        public static IList<DataProfileResult> GetSearchQuery(this IQueryable<DataProfileResult> query, string requestQuery, int page = Configurations.DefaultPageValue, int limit = Configurations.DefaultLimit, string order = Configurations.DefaultOrder)
        {
            var searchParams = QueryHelper.EnsureSearchQueryIsValid(requestQuery, QueryHelper.ParseSearchQuery);
            if (searchParams != null)
            {
                foreach (var searchParam in searchParams)
                {
                    if (ReflectionHelper.HasProperty(searchParam.Key, typeof(DataProfileResult)))
                    {
                        query = query.Where(String.Format("{0} = @0 || {0}.Contains(@0)", searchParam.Key), searchParam.Value);
                    }
                }
            }

            IList<DataProfileResult> result = new ApiList<DataProfileResult>(query, page - 1, limit);

            return result.AsQueryable().OrderBy(order).ToList();
        }

        public static ArrayOfAnyType ToArrayOfAnyType(this List<string> columnNames)
        {
            if (columnNames.Count <= 0)
                return null;

            var objects = new ArrayOfAnyType();

            foreach (var column in columnNames)
            {
                objects.Add(column);
            }

            return objects.Count > 0 ? objects : null;
        }
    }
}