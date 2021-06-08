using System;
using System.IO;
using System.Linq;

namespace GetUserAgents
{
    class Program
    {
        static void Main(
            string[] args )
        {
            var agents = File.ReadAllLines( @"o:\admitad\access.log" );
            var withoutBots = agents.Where( a => a.ToLower().Contains( "bot" ) == false ).Distinct();
            File.WriteAllLines( @"o:\admitad\cleanUA.txt", withoutBots );
        }
    }
}