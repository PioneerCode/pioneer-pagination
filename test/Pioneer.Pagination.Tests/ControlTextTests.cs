using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Pioneer.Pagination.Tests
{
    public class ControlTextTests
    {
        private readonly PioneerPaginationTagHelper _sut = new PioneerPaginationTagHelper();
        private readonly PaginatedMetaService _metaData = new PaginatedMetaService();

        TagHelperContext context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

        TagHelperOutput output = new TagHelperOutput("ul",
            new TagHelperAttributeList(),
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        [Fact]
        public void CanShowCustomPreviousPageText()
        {
            var previousPageText = "Backward";
            _sut.Info = _metaData.GetMetaData(10, 10, 1);
            _sut.PreviousPageText = previousPageText;

            _sut.Process(context, output);
            var markupResult = output.Content.GetContent();

            Assert.Contains(previousPageText, markupResult);
        }

        [Fact]
        public void CanShowCustomNextPageText()
        {
            var nextPageText = "Forward";
            _sut.Info = _metaData.GetMetaData(10, 1, 1);
            _sut.NextPageText = nextPageText;

            _sut.Process(context, output);
            var markupResult = output.Content.GetContent();

            Assert.Contains(nextPageText, markupResult);
        }
    }
}
