using Common.Entities;
using Nest;

namespace Common.Elastic.Queries
{
    internal abstract class BaseQuery
    {

        protected readonly int Size;
        protected readonly string FieldModifier;
        protected readonly int DaysBefore;

        protected BaseQuery(int size, string fieldModifier, int daysBefore) =>
            (Size, FieldModifier, DaysBefore) = (size, fieldModifier, daysBefore);

        public abstract ISearchRequest Query(SearchDescriptor<UrlStatisticEntry> descriptor);

        public static BaseQuery GetQueryMaker(int size, string fieldModifier, int daysBefore, bool isOrdered) =>
            isOrdered
                ? (BaseQuery) new GetUnIndexedUrls(size, fieldModifier, daysBefore)
                : new GettingUrlForIndexingInOrder(size, fieldModifier, daysBefore);
    }
}