namespace Pioneer.Pagination
{
    public interface ILastPageInCollectionService
    {
        int GetLastPageInCollection(int collectionSize, int itemsPerPage);
    }

    /// <summary>
    /// TODO: DI should be setup to handle this dependency.
    /// </summary>
    public class LastPageInCollectionService : ILastPageInCollectionService
    {
        /// <summary>
        /// Get last page number in collection
        /// </summary>
        public int GetLastPageInCollection(int collectionSize, int itemsPerPage)
        {
            var lastPage = (double)collectionSize / itemsPerPage;

            // If whole number, we know nothing exist past lastPage
            if (lastPage % 1 == 0)
            {
                return (int)lastPage;
            }

            // If not whole number, we know there should be one more page past lastPage
            return (int)lastPage + 1;
        }
    }
}
