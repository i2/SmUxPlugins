namespace Navigation
{
    internal class SearchResult
    {
        public int ItemNumber
        {
            get; private set;
        }

        public string Example
        {
            get; private set;
        }

        public SearchResult(int itemNumber, string example)
        {
            ItemNumber = itemNumber;
            Example = example;
        }
    }
}