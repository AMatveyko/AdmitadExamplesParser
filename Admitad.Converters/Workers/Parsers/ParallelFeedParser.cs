// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Admitad.Converters.Entities;

using Common.Api;
using Common.Entities;

namespace Admitad.Converters.Workers
{
    public sealed class ParallelFeedParser : BaseFeedParser
    {

        private readonly LinesBufferInQueue _buffer = new();
        private readonly int _threadCount;
        private readonly List<Task<(List<RawOffer>,List<RawOffer>)>> _tasks = new();
        private bool _isFileIsRead;

        public int Misses { get; set; }

        public ParallelFeedParser(
            ParsingInfo info,
            BackgroundBaseContext context,
            int threadCount = 8 )
            : base( info, context ) =>
            _threadCount = threadCount;

        protected override void PrepareOffers( bool isOnlyCategories ) {
            InitializeWorkers();
            ProcessFile(isOnlyCategories);
            WaitAllTasks();
            CollectResults();
        }

        private void CollectResults() {
            foreach( var (newOffers,deletedOffers) in _tasks.Select( t => t.Result ) ) {
                ShopData.NewOffers.AddRange( newOffers );
                ShopData.DeletedOffers.AddRange( deletedOffers );
            }
        }
        
        private void WaitAllTasks() {
            while( _tasks.Count( t=> t.IsCompleted == false ) > 0 ) {
                Thread.Sleep(1000);
            }
        }
        
        private void InitializeWorkers() {
            for( var i = 0; i < _threadCount; i++ ) {
                _tasks.Add( Task<(List<RawOffer>,List<RawOffer>)>.Factory.StartNew( WorkInitiator ) );
            }
        }

        private (List<RawOffer>,List<RawOffer>) WorkInitiator()
        {
            var bufferNew = new List<RawOffer>();
            var bufferDeleted = new List<RawOffer>();
            while( IsNeedToStopWork() == false ) {
                DoWork( bufferNew, bufferDeleted );
            }

            return (bufferNew,bufferDeleted);
        }

        private void DoWork( ICollection<RawOffer> bufferNew, ICollection<RawOffer> bufferDeleted ) {
            if( _buffer.IsNotEmpty() == false ) {
                Thread.Sleep(10);
                Misses++;
                return;
            }

            var line = _buffer.Dequeue();

            if( line == "queue empty" ) {
                return;
            }

            ICollection<RawOffer> GetBuffer(
                RawOffer offer ) =>
                offer.IsDeleted ? bufferDeleted : bufferNew;

            ProcessString( GetBuffer, line );
        }
        
        private bool IsNeedToStopWork() => _buffer.IsNotEmpty() == false && _isFileIsRead;
        
        private void ProcessFile( bool isOnlyCategoriesNeed ) {
            var buffer = new StringBuilder();
            var firstOfferLine = GetFirstOfferLine( FilePath );
            var lineNumber = 0;

            var func = new Func<string, bool>(
                line => ProcessLineFromFeed(
                    _buffer,
                    line,
                    ref lineNumber,
                    firstOfferLine,
                    buffer,
                    isOnlyCategoriesNeed ) );
            
            ProcessFile( FilePath, func );

            _isFileIsRead = true;
        }
    }
}