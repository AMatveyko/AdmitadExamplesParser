// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "core_settings" ) ]
    public class OptionDb
    {
        [ Key ]
        [Column("option")]
        public string Option { get; set; }
        [Column("value")]
        public string Value { get; set; }
    }
}