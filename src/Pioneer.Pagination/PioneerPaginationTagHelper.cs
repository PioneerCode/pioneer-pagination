using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pioneer.Pagination
{
    public class PioneerPaginationTagHelper : TagHelper
    {
        /// <summary>
        /// Paginated Meta data
        /// </summary>
        public PaginatedMetaModel Info { get; set; }

        /// <summary>
        /// Text to display for previous page
        /// </summary>
        public string PreviousPageText { get; set; } = "Previous";

        /// <summary>
        /// Text to display for next page
        /// </summary>
        public string NextPageText { get; set; } = "Next";

        /// <summary>
        /// Base route minus page value
        /// </summary>
        public string Route { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            BuildParent(output);
            if (Info.PreviousPage.Display) AddPreviousPage(output);
            AddPageNodes(output);
            if (Info.NextPage.Display) AddNextPage(output);
        }

        /// <summary>
        /// Build parent tag (ul)
        /// </summary>
        private static void BuildParent(TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "pagination");
            output.Attributes.Add("role", "navigation");
            output.Attributes.Add("aria-label", "Pagination");
        }

        /// <summary>
        /// Build previous page list item
        /// </summary>
        private void AddPreviousPage(TagHelperOutput output)
        {
            var html =
$@"<li class=""pagination-previous"">
    <a href=""{Route}/{Info.PreviousPage.PageNumber}"" aria-label=""{PreviousPageText} page"">{PreviousPageText} <span class=""show-for-sr"">page</span></a>
</li>";

            output.Content.SetHtmlContent(output.Content.GetContent() + html);
        }

        /// <summary>
        /// Build next page list item
        /// </summary>
        private void AddNextPage(TagHelperOutput output)
        {
            var html =
$@"<li class=""pagination-next"">
    <a href=""{Route}/{Info.NextPage.PageNumber}"" aria-label=""{NextPageText} page"">{NextPageText} <span class=""show-for-sr"">page</span></a>
</li>";

            output.Content.SetHtmlContent(output.Content.GetContent() + html);
        }

        private void AddPageNodes(TagHelperOutput output)
        {
            foreach (var infoPage in Info.Pages)
            {
                string html;
                if (infoPage.IsCurrent)
                {
                    html = $@"<li class=""current""><span class=""show-for-sr"">You're on page</span> {infoPage.PageNumber}</li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                    continue;
                }
                html = $@"<li><a href=""{Route}/{infoPage.PageNumber}"" aria-label=""Page {infoPage.PageNumber}"">{infoPage.PageNumber}</a></li>";
                output.Content.SetHtmlContent(output.Content.GetContent() + html);
            }
        }
    }
}