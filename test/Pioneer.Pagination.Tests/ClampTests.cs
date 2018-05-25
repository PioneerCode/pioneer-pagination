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
        public void NextPageIsLastPageInCollectionWhenRequestedPageIsGreatedThenCollection()
        {
            var result = _sut.GetMetaData(10, 50, 1);
            Assert.True(result.NextPage.PageNumber == 10, "Expected: Next page should be last page in collection.");
        }

        [Fact]
        public void PreviousPageIsLastPageMinusOneInCollectionWhenRequestedPageIsGreatedThenCollection()
        {
            var result = _sut.GetMetaData(10, 100, 1);
            Assert.True(result.PreviousPage.PageNumber == 9, "Expected: Previous page should be last page in collection minus 1.");
        }

        [Fact]
        public void PreviousPageIsFirstPageInCollectionWhenRequestedPageIsZero()
        {
            var result = _sut.GetMetaData(10, 0, 1);
            Assert.True(result.PreviousPage.PageNumber == 1, "Expected: Previous page should be first page in collection.");
        }
       
        [Fact]
        public void NextPageIsSecondPageInCollectionWhenRequestedPageIsZero()
        {
            var result = _sut.GetMetaData(10, 0, 1);
            Assert.True(result.NextPage.PageNumber == 2, "Expected: Next page should be second page in collection.");
        }

        [Fact]
        public void PreviousPageIsFirstPageInCollectionWhenRequestedPageIsNegative()
        {
            var result = _sut.GetMetaData(10, -1, 1);
            Assert.True(result.PreviousPage.PageNumber == 1, "Expected: Previous page should be first page in collection.");
        }

        [Fact]
        public void NextPageIsSecondPageInCollectionWhenRequestedPageIsNegative()
        {
            var result = _sut.GetMetaData(10, -1, 1);
            Assert.True(result.NextPage.PageNumber == 2, "Expected: Next page should be second page in collection.");
        }
    }
}