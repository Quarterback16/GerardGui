
namespace Butler.Implementations
{
    public class GameLine
    {
        public decimal Spread { get; set; }
        public decimal Total { get; set; }

        public GameLine()
        {
            Spread = 0.0M;
            Total = 0.0M;
        }
    }
}
