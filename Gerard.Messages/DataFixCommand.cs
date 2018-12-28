namespace Gerard.Messages
{
    public class DataFixCommand : ICommand
    {
        public string TeamCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"DataFix {FirstName} {LastName} ({TeamCode})";
        }
    }
}
