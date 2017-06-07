using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Builds meta data to be used with PioneerPaginationTagHelper
        /// </summary>
        /// <param name="collectionSize">Total size of collection we are paginating</param>
        /// <param name="selectedPageNumber">Selected page number of pagination</param>
        /// <param name="itemsPerPage">How many items per paginated list</param>
        public PaginatedMetaModel GetMetaData(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            if (collectionSize == 0)
            {
                return GetCollectionSizeZeroModel();
            }

            _pages =  BuildPageNodes(collectionSize, selectedPageNumber, itemsPerPage);
            return new PaginatedMetaModel
            {
                PreviousPage = BuildPreviousPage(collectionSize, selectedPageNumber, itemsPerPage),
                Pages = _pages,
                NextPage = BuildNextPage(collectionSize, selectedPageNumber, itemsPerPage)
            };
        }

        /// <summary>
        /// Get a PaginatedMetaModel based on a collection with the size of zero.
        /// </summary>
        /// <returns></returns>
        private PaginatedMetaModel GetCollectionSizeZeroModel()
        {
            return new PaginatedMetaModel
            {
                PreviousPage = new PreviousPage(),
                Pages = new List<Page>(),
                NextPage = new NextPage()
            };
        }

        #region Previous Page

        /// <summary>
        /// Build previous page object
        /// </summary>
        private PreviousPage BuildPreviousPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var display = DisplayPreviousPage(collectionSize, selectedPageNumber, itemsPerPage);
            return new PreviousPage
            {
                Display = display,
                PageNumber = display ? _pages.First(x => x.IsCurrent).PageNumber - 1  : 1
            };
        }

        /// <summary>
        /// Determine if we need a Previous Page
        /// </summary>
        private bool DisplayPreviousPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            return selectedPageNumber > 1 && collectionSize >= itemsPerPage;
        }

        /// <summary>
        /// Determine page number for previous node
        /// </summary>
        private int GetPreviousPageNumber(int selectedPageNumber)
        {
            return selectedPageNumber > 1 ? selectedPageNumber - 1 : 1;
        }

        #endregion

        #region Next Page

        /// <summary>
        /// Build next page object
        /// </summary>
        private NextPage BuildNextPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var display = DisplayNextPage(collectionSize, selectedPageNumber, itemsPerPage);
            return new NextPage
            {
                Display = display,
                PageNumber = display ? _pages.First(x => x.IsCurrent).PageNumber + 1 : NumberOfNodesInPaginatedList + 1
            };
        }

        /// <summary>
        /// Determine if we need a Next Page
        /// </summary>
        private bool DisplayNextPage(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            return selectedPageNumber <= GetLastPageInCollection(collectionSize, itemsPerPage) - (NumberOfNodesInPaginatedList - 2);
        }

        #endregion

        #region Page Nodes

        /// <summary>
        /// Build individual page nodes
        /// </summary>
        private List<Page> BuildPageNodes(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var lastPage = GetLastPageInCollection(collectionSize, itemsPerPage);

            // If we have less then NumberOfNodesInPaginatedList * itemPage in our collectionSize,
            // then we know we have a partial list
            if (NumberOfNodesInPaginatedList * itemsPerPage > collectionSize)
            {
                return BuildPartialList(collectionSize, selectedPageNumber, itemsPerPage);
            }

            //  We are at the first collection of nodes in paginated list: 1, 2 & 3
            if (selectedPageNumber < NumberOfNodesInPaginatedList - 2)
            {
                return BuildStartList(selectedPageNumber);
            }

            // We are at the last collection of nodes in paginated list : last -2 , last - 1 & last
            if (selectedPageNumber > lastPage - (NumberOfNodesInPaginatedList - 2))
            {
                return BuildEndList(collectionSize, selectedPageNumber, itemsPerPage);
            }

            // We are at an in between collection of node in paginated list
            return BuildFullList(selectedPageNumber);
        }

        /// <summary>
        /// Build a full (NumberOfNodesInPaginatedList) collection of page nodes
        /// [ ][ ][ ][x][x][x][x][x][x][x][x][x][x][ ][ ][ ]
        /// </summary>
        private List<Page> BuildFullList(int selectedPageNumber)
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
        /// Build a full (NumberOfNodesInPaginatedList) collection of page nodes
        /// [x][x][ ][ ][ ][ ][ ][ ][ ][ ] ][ ][ ][ ][ ][ ]
        /// </summary>
        private List<Page> BuildPartialList(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var pages = new List<Page>();
            for (var i = 0; i < GetLastPageInCollection(collectionSize, itemsPerPage); i++)
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
        private List<Page> BuildStartList(int selectedPageNumber)
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
        private List<Page> BuildEndList(int collectionSize, int selectedPageNumber, int itemsPerPage)
        {
            var lastPage = GetLastPageInCollection(collectionSize, itemsPerPage);

            var pages = new List<Page>
            {
                BuildNode(lastPage - 4),
                BuildNode(lastPage - 3),
                BuildNode(lastPage - 2),
                BuildNode(lastPage - 1),
                BuildNode(lastPage)
            };

            var unshiftedIndex = lastPage - selectedPageNumber;
            pages[NumberOfNodesInPaginatedList - 1 - unshiftedIndex].IsCurrent = true;

            return pages;
        }

        /// <summary>
        /// Build Selectable Node
        /// </summary>
        private Page BuildNode(int pageNumber)
        {
            return new Page
            {
                IsCurrent = false,
                PageNumber = pageNumber
            };
        }

        #endregion

        /// <summary>
        /// Get last page number in collection
        /// </summary>
        private int GetLastPageInCollection(int collectionSize, int itemsPerPage)
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