// a.snegovoy@gmail.com

using System.Linq;

using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Repositories;

namespace WorkWithTags
{
    internal sealed class TagsCloner
    {

        private readonly TagsRepository _repository;

        private readonly string _replace;
        private readonly string _append;

        public TagsCloner( TagsRepository repository, string replace, string append )
        {
            _repository = repository;
            _append = append.ToLower();
            _replace = replace.ToLower();
        }

        public int CopyTags( int sourceCategoryId, int destinationCategoryId, (string,string)? replace = null )
        {
            var tags = _repository.GetTagsByCategory( sourceCategoryId ); //.Where( t => t.Id != 3970 );
            var cloned = tags.Select( t => t.Clone() ).Cast<TagDb>().ToList();
            foreach( var clone in cloned ) {
                clone.CategoryId = destinationCategoryId;
                ChangeH1AndTitle( clone, replace );
            }
            _repository.AddNewTags( cloned );
            return cloned.Count;
        }

        private void ChangeH1AndTitle( TagDb tag, ( string, string )? replace )
        {
            tag.Name2 = Replace( tag.Name2, replace );
            tag.H1 = ProcessText( Replace( tag.H1, replace ) );
            tag.Title = ProcessText( Replace( tag.Title, replace ) );
        }

        private string ProcessText( string text )
        {
            var lower = text.ToLower();
            
            var replaced = lower.Replace( _replace, string.Empty ).Trim();
            var appended = replaced + $" { _append }";
            return Helper.ToUpperFirstLetter( appended );
        }
        
        private static string Replace( string text, (string, string)? replace ) =>
            replace.HasValue && string.IsNullOrWhiteSpace( text ) == false
                ? text.ToLower().Replace( replace.Value.Item1, replace.Value.Item2 )
                : text;

    }
}