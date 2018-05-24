using Xunit;

namespace Pioneer.Pagination.Tests
{
    /// <summary>
    /// Clamp Tests
    /// </summary>
    public class ClampTests
    {
        private readonly PaginatedMetaService _sut = new PaginatedMetaService();

        [Fact]
        public void LastPageDisplayed()
        {
            var result = _sut.GetMetaData(10, 11, 1);
            Assert.True(result.NextPage.PageNumber == 10, "Expected: Last Page ");
        }

        [Fact]
        public void FirstPageDisplayed()
        {
            var result = _sut.GetMetaData(10, 0, 1);
            Assert.True(result.NextPage.PageNumber == 1, "Expected: First Page ");
        }
    }
}