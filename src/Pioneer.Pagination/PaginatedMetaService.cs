using System.Collections.Generic;

namespace Pioneer.Pagination
{
    public interface IPaginatedMetaService
    {
        PaginatedMetaModel GetMetaData(int collectionSize, int selectedPageNumber, int itemsPerPage);
    }

    public class PaginatedMetaService : IPaginatedMetaService
    {
        /// <summary>
        /// As of current, we will always be displaying 5 options per pagination list.
        /// If this value changes at some point, some logic to calculate "middle" will break.
        /// </summary>
        private const int NumberOfNodesInPaginatedList = 5;

        /// <summary>
        /// Allows us to track indexes for previous and next 
        /// </summary>
        private List<Page> _pages;

        private readonly IPreviousPageService _previousPageService;
        private readonly INextPageService _nextPageService;
        private readonly INodeService _nodeService;
        private readonly ILastPageInCollectionService _pageInCollectionService;

        public PaginatedMetaService()
        {
            _previousPageService = new PreviousPageService();
            _nextPageService = new NextPageService();
            _nodeService = new NodeService();
            _pageInCollectionService = new LastPageInCollectionService();
        }

        /// <summary>
        /// Builds meta data to be used with PioneerPaginationTagHelper
        /// </summary>
        /// <param name="collectionSize">Total size of collection we are paginating</param>
        /// <param name="selectedPageNumber">Selected page number of pagination</param>
        /// <param name="itemsPerPage">How many items per paginated list</param>
        public PaginatedMetaModel GetMetaData(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var lastPage = _pageInCollectionService.GetLastPageInCollection(collectionSize, itemsPerPage);

            // Cover > out of range exceptions
            if (lastPage < selectedPageNumber)
            {
                selectedPageNumber = lastPage;
            }

            // Cover < out of range exceptions
            if (selectedPageNumber < 1)
            {
                selectedPageNumber = 1;
            }

            if (collectionSize == 0)
            {
                return GetCollectionSizeZeroModel();
            }

            _pages = _nodeService.BuildPageNodes(collectionSize, selectedPageNumber, itemsPerPage, NumberOfNodesInPaginatedList);
            return new PaginatedMetaModel
            {
                PreviousPage = _previousPageService.BuildPreviousPage(_pages, collectionSize, selectedPageNumber, itemsPerPage),
                Pages = _pages,
                NextPage = _nextPageService.BuildNextPage(
                    _pages, 
                    collectionSize,
                    selectedPageNumber, 
                    itemsPerPage, 
                    NumberOfNodesInPaginatedList
                )
            };
        }

        /// <summary>
        /// Get a PaginatedMetaModel based on a collection with the size of zero.
        /// </summary>
        private static PaginatedMetaModel GetCollectionSizeZeroModel()
        {
            return new PaginatedMetaModel
            {
                PreviousPage = new PreviousPage
                {
                    Display = false
                },
                Pages = new List<Page>(),
                NextPage = new NextPage
                {
                    Display = false
                }
            };
        }
    }
}