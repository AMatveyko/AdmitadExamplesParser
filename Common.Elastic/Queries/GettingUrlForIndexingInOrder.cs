using System;
using System.Collections.Generic;
using Common.Entities;
using Nest;

namespace Common.Elastic.Queries
{
    internal class GettingUrlForIndexingInOrder : BaseQuery
    {
        private const string ScoreField = "_score";
        private string LastIndexCheckField => $"dateLastIndexCheck{FieldModifier}";
        private string LastVisitDate => $"lastVisitDate{FieldModifier}";
        private string DateLastIndexCheck => $"dateLastIndexCheck{FieldModifier}";
        private string Indexed => $"indexed{FieldModifier}";

        private string Script =>
            $"( doc['lastVisitDate{FieldModifier}'].size() > 0 ) && ( doc['dateLastIndexCheck{FieldModifier}'].size() > 0 ) && doc['dateLastIndexCheck{FieldModifier}'].value.isBefore( doc['lastVisitDate{FieldModifier}'].value ) ";

        public GettingUrlForIndexingInOrder(int size, string fieldModifier, int daysBefore)
            : base(size, fieldModifier, daysBefore) { }
        
        public override ISearchRequest Query(SearchDescriptor<UrlStatisticEntry> descriptor ) =>
            descriptor.Query( MakeQuery ).Size(Size).Sort( GetSort );

        private IPromise<IList<ISort>> GetSort(SortDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor
                .Field(f => f.Field(ScoreField).Order(SortOrder.Descending))
                .Field(f => f.Field(LastIndexCheckField ).Order(SortOrder.Ascending));


        private QueryContainer MakeQuery(QueryContainerDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor.FunctionScore( MakeFunctionScore );

        private IFunctionScoreQuery MakeFunctionScore(FunctionScoreQueryDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor.Query(MakeFunctionQuery).Functions(MakeScoreFunctions).BoostMode(FunctionBoostMode.Sum);

        private QueryContainer MakeFunctionQuery(QueryContainerDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor.Bool(b => b.Should(MakeShouldForFunctionQuery()));

        private Func<QueryContainerDescriptor<UrlStatisticEntry>, QueryContainer>[] MakeShouldForFunctionQuery() =>
            new Func<QueryContainerDescriptor<UrlStatisticEntry>, QueryContainer>[] {
                s => s.Bool(b =>
                    b.MustNot(mn =>
                        mn.Exists(e => e.Field(LastIndexCheckField)))),
                s => s.DateRange( dr => dr.Field( LastIndexCheckField ).LessThan(DateTime.Now.AddDays(-DaysBefore)))
            };

        private IPromise<IList<IScoreFunction>> MakeScoreFunctions(
            ScoreFunctionsDescriptor<UrlStatisticEntry> descriptor) {
            var withFirstFunction = WeightShownButNotVisited(descriptor);
            return WeightCheckedBeforeVisited(withFirstFunction);
        }

        private ScoreFunctionsDescriptor<UrlStatisticEntry> WeightShownButNotVisited(
            ScoreFunctionsDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor.Weight(w =>
                w.Filter(f =>
                    f.Bool(b =>
                        b.Must(m =>
                                m.Exists(e => e.Field(LastVisitDate)),
                            m => m.Bool(bm =>
                                bm.MustNot(mn => mn.Exists(e => e.Field(DateLastIndexCheck))))
                        ))).Weight(20));
        
        private ScoreFunctionsDescriptor<UrlStatisticEntry> WeightCheckedBeforeVisited(ScoreFunctionsDescriptor<UrlStatisticEntry> descriptor) =>
            descriptor.Weight( w =>
                w.Filter( f =>
                    f.Bool( b =>
                        b.Must( m =>
                                m.Term( t =>
                                    t.Field( Indexed ).Value( false )),
                            m =>
                                m.Script( s => s.Script( ss => ss.Source( Script ))
                                )))).Weight(10));

    }
}