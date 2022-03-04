using System;
using Common.Entities;
using Nest;

namespace Common.Elastic.Queries
{
    internal class GetUnIndexedUrls : BaseQuery
    {
        
        public GetUnIndexedUrls(int size, string fieldModifier, int daysBefore)
            : base(size, fieldModifier, daysBefore) { }
        
        public override ISearchRequest Query( SearchDescriptor<UrlStatisticEntry> descriptor ) {

            var fieldName = $"dateLastIndexCheck{FieldModifier}";
            
            return descriptor.Query(q =>
                    q.Bool(b =>
                        b.Should(bs =>
                                bs.Bool(bbs =>
                                    bbs.MustNot(bbsm =>
                                        bbsm.Exists(e =>
                                            e.Field(fieldName)
                                        )
                                    )
                                ),
                            bs2 =>
                                bs2.DateRange(r =>
                                    r.LessThan(DateTime.Now.AddDays(-DaysBefore )).Field(fieldName)))))
                .Size(Size).Source(s => s.Includes(i => i.Field(e => e.Url)));
            
        }
    }
}