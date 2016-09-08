using System.Collections.Generic;

namespace Pioneer.Pagination
{
    public class PaginatedMetaModel
    {
        public PaginatedMetaModel()
        {
            Pages = new List<Page>();
        }

        /// <summary>
        /// Active page nodes
        /// </summary>
        public List<Page> Pages { get; set; }
   
        /// <summary>
        /// Previous Page node
        /// </summary>
        public PreviousPage PreviousPage { get; set; }

        /// <summary>
        /// Next page node
        /// </summary>
        public NextPage NextPage { get; set; }
    }

    /// <summary>
    /// Previous Node Meta
    /// </summary>
    public class PreviousPage
    {
        /// <summary>
        /// Do we need to display this node
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Associated Page Number
        /// </summary>
        public int PageNumber { get; set; }
    }

    /// <summary>
    /// Next Node Meta
    /// </summary>
    public class NextPage
    {
        /// <summary>
        /// Do we need to display this node
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Associated Page Number
        /// </summary>
        public int PageNumber { get; set; }
    }

    public class Page
    {
        /// <summary>
        /// Associated Page Number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Is this the current page
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}
