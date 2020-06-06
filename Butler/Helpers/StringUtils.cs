namespace Butler.Helpers
{
    public static class StringUtils
    {
        public static string StringOfSize(
            int size,
            string theString)
        {
            return theString.PadRight(size).Substring(0, size);
        }

        public static string PadLeft(
            int size,
            string theString)
        {
            theString = theString.PadLeft(4);
            return theString.Substring(
                theString.Length - size,
                size);
        }
    }
}
