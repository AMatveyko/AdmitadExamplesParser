// a.snegovoy@gmail.com

namespace Common.Entities
{
    public class AgeRange
    {

        private decimal _to;
        
        public decimal From { get; set; }
        public decimal To
        {
            get => _to;
            set => _to = value > 1200 ? 1200 : value;
        }
    }
}