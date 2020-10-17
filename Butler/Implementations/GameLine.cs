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

        public bool IsEmpty()
        {
            if (Spread.Equals(0.0M)
                && Total.Equals(0.0M))
                return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Spread} : {Total}";
        }
    }
}
