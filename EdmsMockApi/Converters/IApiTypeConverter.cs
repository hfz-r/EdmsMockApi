using System;
using System.Collections.Generic;
using ServiceReference;

namespace EdmsMockApi.Converters
{
    public interface IApiTypeConverter
    {
        object ToEnumNullable(string value, Type type);

        int ToInt(string value);

        int? ToIntNullable(string value);

        IList<int> ToListOfInts(string value);

        IList<string> ToListOfStrings(string value);

        DateTime? ToUtcDateTimeNullable(string value);
    }
}