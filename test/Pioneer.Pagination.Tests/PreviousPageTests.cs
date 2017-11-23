using Xunit;

namespace Pioneer.Pagination.Tests
{
    /// <summary>
    /// Previous Page Tests
    /// </summary>
    public class PreviousPageTests
    {
        private readonly PaginatedMetaService _sut = new PaginatedMetaService();

        [Fact]
        public void PreviousPageNotDisplayed()
        {
            var result = _sut.GetMetaData(10, 1, 1);
            Assert.False(result.PreviousPage.Display, "Expected : Previous Page Displayed = false");
        }

        [Fact]
        public void PreviousPageDisplayed()
        {
            var result = _sut.GetMetaData(10, 2, 1);
            Assert.True(result.PreviousPage.Display, "Expected : Previous Page Displayed = true");
        }
    }
}