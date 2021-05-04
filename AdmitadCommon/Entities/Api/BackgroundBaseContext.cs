// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

using AdmitadCommon.Extensions;
using AdmitadCommon.Helpers;

namespace AdmitadCommon.Entities.Api
{
    public class BackgroundBaseContext
    {

        private readonly Stopwatch _stopwatch;
        private bool _isFinished = false;
        
        public BackgroundBaseContext( string id, string name ) {
            Id = id;
            _stopwatch = new Stopwatch();
            Name = name;
        }
        
        public string Id { get; }
        public string Name { get; } 
        [ JsonIgnore ] public BackgroundStatus WorkStatus { get; set; } = BackgroundStatus.OutInLine;
        public string Status => WorkStatus.ToString();
        
        public bool IsFinished
        {
            get => _isFinished;
            set
            {
                if( value ) {
                    _stopwatch.Stop();
                }

                _isFinished = value;
            }
        }
        public int PercentFinished { get; private set; }
        public string Content { get; set; } = "Ждем результат...";
        public bool IsError { get; set; }
        public string Time => GetTime();
        public List<string> Messages =>
            RawMessages.Select( t => $"{ ( t.Item1 ? "Error" : "Info") }: {t.Item2}" ).ToList();

        public readonly List<( bool, string )> RawMessages = new();
        
        [ JsonIgnore ]
        public int TotalActions { get; set; }
        [ JsonIgnore ]
        public int CurrentAction { get; set; }

        public void AddMessage( string text, bool isError = false ) =>
            RawMessages.Add( ( isError, GetMessage( text ) ) );

        public void CalculatePercent() => 
            PercentFinished = MathHelper.GetPercent( CurrentAction++, TotalActions );
        
        public void SetProgress( int complete, int total ) =>
            PercentFinished = MathHelper.GetPercent( complete, total );

        public void Start() => _stopwatch.Start();

        private string GetMessage(
            string text ) =>
            $"{text} time {GetTime()}";

        private string GetTime() => _stopwatch.ElapsedMilliseconds.ToString();

        protected static string GetCollectedId( params string[] ids ) =>
            string.Join( ":", ids.Where( i => i.IsNotNullOrWhiteSpace() ) );

    }
}