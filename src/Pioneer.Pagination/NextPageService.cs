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
        private readonly ILastPageInCollectionService _pageInCollectionService;

        public NextPageService()
        {
            _pageInCollectionService = new LastPageInCollectionService();
        }

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
                PageNumber = selectedPageNumber == collectionSize ?
                    selectedPageNumber :
                    GetPageNumber(display, pages, numberOfNodesInPaginatedList)
            };
        }

        private static int GetPageNumber(bool display, IEnumerable<Page> pages, int numberOfNodesInPaginatedList)
        {
            return display ? pages.First(x => x.IsCurrent).PageNumber + 1 : numberOfNodesInPaginatedList + 1;
        }

        /// <summary>
        /// Determine if we need a Next Page
        /// </summary>
        private bool DisplayNextPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            return selectedPageNumber < _pageInCollectionService.GetLastPageInCollection(collectionSize, itemsPerPage);
        }
    }
}