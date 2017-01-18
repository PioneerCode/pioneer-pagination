using System.Linq;
using Xunit;

namespace Pioneer.Pagination.Tests
{
    /// <summary>
    /// Next Page Tests
    /// </summary>
    public class NextPageTests
    {
        private readonly PaginatedMetaService _sut = new PaginatedMetaService();

        [Fact]
        public void NextPageNotDisplayed()
        {
            var result = _sut.GetMetaData(10, 8, 1);
            Assert.False(result.NextPage.Display, "Expected: Next Page Display = false");
        }

        [Fact]
        public void NextPageDisplayed()
        {
            var result = _sut.GetMetaData(10, 1, 1);
            Assert.True(result.NextPage.Display, "Expected: Next Page Displayed = true");
        }

        [Fact]
        public void NextPageNextPageOneIndexPastLastNumericShown()
        {
            var result = _sut.GetMetaData(100, 6, 10);
            Assert.True(result.NextPage.PageNumber == result.Pages.First(x => x.IsCurrent).PageNumber + 1,
                string.Format("Expected: Next Page number == {0}", result.Pages.First(x => x.IsCurrent).PageNumber + 1));
        }
    }
}