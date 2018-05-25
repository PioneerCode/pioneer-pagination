using System.Collections.Generic;

namespace Pioneer.Pagination
{
    public interface INodeService
    {
        List<Page> BuildPageNodes(
            int collectionSize,
            int selectedPageNumber,
            int itemsPerPage,
            int numberOfNodesInPaginatedList
        );
    }
    public class NodeService : INodeService
    {
        private readonly ILastPageInCollectionService _pageInCollectionService;

        public NodeService()
        {
            _pageInCollectionService = new LastPageInCollectionService();
        }

        /// <summary>
        /// Build individual page nodes
        /// </summary>
        public List<Page> BuildPageNodes(int collectionSize, int selectedPageNumber, int itemsPerPage, int numberOfNodesInPaginatedList)
        {
            var lastPage = _pageInCollectionService.GetLastPageInCollection(collectionSize, itemsPerPage);

            // If we have less then numberOfNodesInPaginatedList * itemPage in our collectionSize,
            // then we know we have a partial list
            if (numberOfNodesInPaginatedList * itemsPerPage > collectionSize)
            {
                return BuildPartialList(collectionSize, selectedPageNumber, itemsPerPage);
            }

            //  We are at the first collection of nodes in paginated list: 1, 2 & 3
            if (selectedPageNumber < numberOfNodesInPaginatedList - 2)
            {
                return BuildStartList(selectedPageNumber);
            }

            // We are at the last collection of nodes in paginated list : last -2 , last - 1 & last
            if (selectedPageNumber > lastPage - (numberOfNodesInPaginatedList - 2))
            {
                return BuildEndList(collectionSize, selectedPageNumber, itemsPerPage, numberOfNodesInPaginatedList);
            }

            // We are at an in between collection of node in paginated list
            return BuildFullList(selectedPageNumber);
        }

        /// <summary>
        /// Build a full (numberOfNodesInPaginatedList) collection of page nodes
        /// [ ][ ][ ][x][x][x][x][x][x][x][x][x][x][ ][ ][ ]
        /// </summary>
        private static List<Page> BuildFullList(int selectedPageNumber)
        {
            var pages = new List<Page>
            {
                BuildNode(selectedPageNumber - 2),
                BuildNode(selectedPageNumber - 1),
                BuildNode(selectedPageNumber),
                BuildNode(selectedPageNumber + 1),
                BuildNode(selectedPageNumber + 2)
            };

            pages[2].IsCurrent = true;

            return pages;
        }

        /// <summary>
        /// Build a full (numberOfNodesInPaginatedList) collection of page nodes
        /// [x][x][ ][ ][ ][ ][ ][ ][ ][ ] ][ ][ ][ ][ ][ ]
        /// </summary>
        private List<Page> BuildPartialList(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var pages = new List<Page>();
            for (var i = 0; i < _pageInCollectionService.GetLastPageInCollection(collectionSize, itemsPerPage); i++)
            {
                pages.Add(BuildNode(i + 1));
            }

            pages[selectedPageNumber - 1].IsCurrent = true;

            return pages;
        }

        /// <summary>
        /// Build list when selected page falls into first collection of node list
        /// Start shifting after three
        /// [*][*][*][x][x][ ][ ][ ][ ][ ][ ][ ][ ][ ][ ]
        /// </summary>
        private static List<Page> BuildStartList(int selectedPageNumber)
        {
            var pages = new List<Page>
            {
                BuildNode(1),
                BuildNode(2),
                BuildNode(3),
                BuildNode(4),
                BuildNode(5)
            };

            pages[selectedPageNumber - 1].IsCurrent = true;

            return pages;
        }

        /// <summary>
        /// Build list when selected page falls into last collection of nodes
        /// Stop shifting after three from end
        /// [ ][ ][ ][ ][ ][ ][ ][ ][ ][ ][x][x][*][*][*]
        /// </summary>
        private List<Page> BuildEndList(int collectionSize, int selectedPageNumber, int itemsPerPage, int numberOfNodesInPaginatedList)
        {
            var lastPage = _pageInCollectionService.GetLastPageInCollection(collectionSize, itemsPerPage);

            var pages = new List<Page>
            {
                BuildNode(lastPage - 4),
                BuildNode(lastPage - 3),
                BuildNode(lastPage - 2),
                BuildNode(lastPage - 1),
                BuildNode(lastPage)
            };

            var unshiftedIndex = lastPage - selectedPageNumber;
            pages[numberOfNodesInPaginatedList - 1 - unshiftedIndex].IsCurrent = true;

            return pages;
        }

        /// <summary>
        /// Build Selectable Node
        /// </summary>
        private static Page BuildNode(int pageNumber)
        {
            return new Page
            {
                IsCurrent = false,
                PageNumber = pageNumber
            };
        }
    }
}
