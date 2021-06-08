// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace Common.Settings
{
    public class DbSettings
    {
        [ JsonProperty( "mysql_host" ) ]
        public string Host { get; set; }
        [ JsonProperty( "mysql_user" ) ]
        public string UserName { get; set; }
        [ JsonProperty( "mysql_password" ) ]
        public string Password { get; set; }
        [ JsonProperty( "mysql_database" ) ]
        public string DatabaseName { get; set; }
        [ JsonProperty( "mysql_version" ) ]
        public string Version { get; set; }

        public string GetConnectionString() => $"server={Host};user={UserName};password={Password};database={DatabaseName};convert zero datetime=True;";
    }
}