using System.IO;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadExamplesParser.Entities;
using AdmitadExamplesParser.Workers.Components;

using Common.Settings;

using Newtonsoft.Json;

namespace AdmitadExamplesParser
{
    internal class Program
    {

        private const string SettingsPath = @"o:\admitad\workData\settings.json";
        
        private static void Main(
            string[] args )
        {
            var configString = File.ReadAllText( SettingsPath );
            var settings = JsonConvert.DeserializeObject<ProcessorSettings>( configString );
            var processor = new Processor( settings, new BackgroundBaseContext("1", "name" ) );
            processor.Start();
        }
    }
}