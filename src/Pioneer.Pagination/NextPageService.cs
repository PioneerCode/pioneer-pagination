using System.Collections.Generic;
using System.Linq;

namespace Pioneer.Pagination
{

    public interface INextPageService
    {
        NextPage BuildNextPage(
            IEnumerable<Page> pages,
            int collectionSize,
            int selectedPageNumber,
            int itemsPerPage,
            int numberOfNodesInPaginatedList
        );
    }

    public class NextPageService : INextPageService
    {
        /// <summary>
        /// Build next page object
        /// </summary>
        public NextPage BuildNextPage(
            IEnumerable<Page> pages,
            int collectionSize,
            int selectedPageNumber,
            int itemsPerPage,
            int numberOfNodesInPaginatedList
        )
        {
            var display = DisplayNextPage(collectionSize, selectedPageNumber, itemsPerPage);
            return new NextPage
            {
                Display = display,
                PageNumber = display ? pages.First(x => x.IsCurrent).PageNumber + 1 : numberOfNodesInPaginatedList + 1
            };
        }

        /// <summary>
        /// Determine if we need a Next Page
        /// </summary>
        private static bool DisplayNextPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            return selectedPageNumber < GetLastPageInCollection(collectionSize, itemsPerPage);
        }

        /// <summary>
        /// Get last page number in collection
        /// </summary>
        private static int GetLastPageInCollection(int collectionSize, int itemsPerPage)
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