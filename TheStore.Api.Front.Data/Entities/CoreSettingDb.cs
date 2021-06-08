// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace TheStore.Api.Front.Data.Entities
{
    [ Keyless ]
    [ Table( "core_settings" ) ]
    public class CoreSettingDb
    {
        [ Column( "option" ) ]
        public string Option { get; set; }
        [ Column( "value" ) ]
        public string Value { get; set; }
    }
}