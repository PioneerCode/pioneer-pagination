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

        private readonly TagHelperContext _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

        private readonly TagHelperOutput _output = new TagHelperOutput("ul",
            new TagHelperAttributeList(),
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                var helperContent = tagHelperContent.SetHtmlContent(string.Empty);
                return Task.FromResult(helperContent);
            });

        [Fact]
        public void CanShowCustomPreviousPageText()
        {
            const string previousPageText = "Backward";
            _sut.Info = _metaData.GetMetaData(10, 10, 1);
            _sut.PreviousPageText = previousPageText;

            _sut.Process(_context, _output);
            var markupResult = _output.Content.GetContent();

            Assert.Contains(previousPageText, markupResult);
        }

        [Fact]
        public void CanShowCustomNextPageText()
        {
            const string nextPageText = "Forward";
            _sut.Info = _metaData.GetMetaData(10, 1, 1);
            _sut.NextPageText = nextPageText;

            _sut.Process(_context, _output);
            var markupResult = _output.Content.GetContent();

            Assert.Contains(nextPageText, markupResult);
        }
    }
}
