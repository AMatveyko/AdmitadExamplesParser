// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers
{
    public abstract class BaseComponent
    {
        private readonly ComponentType _type;
        private readonly StatisticsBlock _statisticsBlock;
        protected readonly BackgroundBaseContext _context;

        protected BaseComponent(
            ComponentType componentType, BackgroundBaseContext context ) {
            _type = componentType;
            _context = context;
            _statisticsBlock = StatisticsContainer.GetNewBlock( _type.ToString() );
        }

        protected void AddStatisticLine(
            string line,
            long? workTime = null ) {
            _statisticsBlock.AddLine( line, workTime );
        }

        protected void MeasureWorkTime( Action action ) {
            DebugHelper.MeasureExecuteTime( action, out var executeTime );
            _statisticsBlock.WorkTime = executeTime;
        }
        
        protected T MeasureWorkTime<T>( Func<T> func ) {
            var result = DebugHelper.MeasureExecuteTime( func, out var executeTime );
            _statisticsBlock.WorkTime = executeTime;
            return result;
        }

        protected T Measure< T >(
            Func<T> func,
            out long time )
        {
            return DebugHelper.MeasureExecuteTime( func, out time );
        }
        
        protected void Measure(
            Action action,
            out long time )
        {
            DebugHelper.MeasureExecuteTime( action, out time );
        }
        
    }
}