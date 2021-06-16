// a.snegovoy@gmail.com

namespace Common.Entities
{
    public sealed class AgeGenderForCategoryContainer
    {
        public AgeGenderForCategoryContainer( string categoryId, int ageId, int genderId ) =>
            ( CategoryId, AgeId, GenderId ) = ( categoryId, ageId, genderId );
        
        public string CategoryId { get; }
        public int AgeId { get; }
        public int GenderId { get; }
    }
}