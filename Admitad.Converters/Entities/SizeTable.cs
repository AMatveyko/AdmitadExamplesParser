// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace Admitad.Converters.Entities
{
    public static class SizeTable
    {
        #region Data

        public const int SizeSmThreshold = 50;

        private static Dictionary<string, Dictionary<string, AgeRange>> _allTables;

        private static Dictionary<string, string> _ruBaby = new() {
            {"18", "0-2"},
            {"19", "2-3"},
            {"20", "3-6"},
            {"21-22", "7-9"},
            {"23-24", "10-17"},
            {"25-26", "18-23"},
            {"27", "24-35"},
            {"28", "36-37"}
        };

        private static Dictionary<string, string> _ruChild = new() {
            {"18", "0-2"},
            {"19", "2-3"},
            {"20", "3-6"},
            {"21-22", "7-9"},
            {"23-24", "10-17"},
            {"25-26", "18-23"},
            {"27", "24-35"},
            {"28-29", "36-59"},
            {"30-31", "60-71"},
            {"32", "72-83"},
            {"33", "84-95"},
            {"34-35", "96-107"},
            {"36-37", "108-119"},
            {"38", "120-131"},
            {"39", "132-145"},
            {"40", "144-155"},
            {"41-42", "156-181"}
        };

        private static Dictionary<string, string> _euBaby = new() {
            {"50-61", "0-1"},
            {"62-67", "1-2"},
            {"68-73", "2-3"},
            {"74-79", "3-6"},
            {"80-85", "7-11"},
            {"86-91", "12-17"},
            {"92-97", "18-23"},
            {"98-103", "24-35"},
            {"104", "36-37"}
        };

        private static Dictionary<string, string> _euChild = new() {
            {"50-61", "0-1"},
            {"62-67", "1-2"},
            {"68-73", "2-3"},
            {"74-79", "3-6"},
            {"80-85", "7-11"},
            {"86-91", "12-17"},
            {"92-97", "18-23"},
            {"98-103", "36-47"},
            {"104-109", "48-59"},
            {"110-115", "60-71"},
            {"116-121", "72-83"},
            {"122-127", "84-95"},
            {"128-133", "96-107"},
            {"134-139", "108-119"},
            {"140-145", "120-131"},
            {"146-151", "132-143"},
            {"152-155", "144-155"},
            {"156-157", "156-167"},
            {"158-163", "168-179"},
            {"164", "180-180"}
        };
        
        #endregion
        
        static SizeTable() => Initialize();

        
        public static AgeRange SizeEu( string value, bool isBaby ) => GetRange( value, SizeType.Eu, isBaby );
        public static AgeRange SizeRu( string value, bool isBaby ) => GetRange( value, SizeType.Ru, isBaby );

        public static AgeRange GetRange( string value, SizeType type, bool isBaby )
        {
            if( type == SizeType.Undefined ) {
                return null;
            }
            var key = $"_{ type.ToString().ToLower() }{ ( isBaby ? "Baby" : "Child" ) }";
            var dictionary = _allTables[ key ];
            return dictionary.ContainsKey( value ) ? dictionary[ value ] : null;
        }

        private static void Initialize()
        {
            _allTables = new () {
                { nameof(_ruBaby), ProcessDictionary( _ruBaby ) },
                { nameof(_ruChild), ProcessDictionary( _ruChild ) },
                { nameof(_euBaby), ProcessDictionary( _euBaby ) },
                { nameof(_euChild), ProcessDictionary( _euChild ) },
            };
        }

        private static Dictionary<string, AgeRange> ProcessDictionary( Dictionary<string,string> dictionary )
        {
            var newDictionary = new Dictionary<string, AgeRange>();
            foreach( var pair in dictionary ) {
                var range = CreateRange( pair.Value );
                var iterationRange = pair.Key.Split( "-" );
                var from = int.Parse( iterationRange[ 0 ] );
                var to = iterationRange.Length == 2 ? int.Parse( iterationRange[ 1 ] ) : from;
                for( var i = from; i <= to; i++ ) {
                    newDictionary.Add( i.ToString(), range );
                }
            }

            return newDictionary;
        }

        private static AgeRange CreateRange( string rawRange )
        {
            var rangeParts = rawRange.Split( "-" );
            return rangeParts.Length == 2
                ? new AgeRange( int.Parse( rangeParts[ 0 ] ), int.Parse( rangeParts[ 1 ] ) )
                : new AgeRange( int.Parse( rangeParts[ 0 ] ) );
        }
        
    }
}