using System.Linq;
using Xunit;

namespace Pioneer.Pagination.Tests
{
    /// <summary>
    /// Page Node Tests
    /// </summary>
    public class PageNodeTests
    {
        private readonly PaginatedMetaService _sut = new PaginatedMetaService();


        [Fact]
        public void CollectionSizeZeroReturnsValidObject()
        {
            var result = _sut.GetMetaData(0, 1, 1);
            Assert.True(result.Pages.Count == 0, "Zero Size Valid Object");
        }


        [Fact]
        public void Full()
        {
            var result = _sut.GetMetaData(10, 1, 1);
            Assert.True(result.Pages.Count == 5, "5 Pages");
        }

        [Fact]
        public void FullMiddlePageEqualsSelectedPage()
        {
            var result = _sut.GetMetaData(10, 5, 1);
            Assert.True(result.Pages[2].PageNumber == 5, "Middle page in full collection should equal the page selected");
        }

        [Fact]
        public void FullMiddlePageIsCurrent()
        {
            var result = _sut.GetMetaData(10, 5, 1);
            Assert.True(result.Pages[2].IsCurrent, "Middle page in full collection should equal the current page");
        }

        [Fact]
        public void StartShiftAfterThree()
        {
            var result = _sut.GetMetaData(10, 1, 1);
            Assert.True(result.Pages[0].IsCurrent, "First index should be current");
            result = _sut.GetMetaData(10, 2, 1);
            Assert.True(result.Pages[1].IsCurrent, "Second index should be current");
            result = _sut.GetMetaData(10, 3, 1);
            Assert.True(result.Pages[2].IsCurrent, "Third index should be current");
            result = _sut.GetMetaData(10, 4, 1);
            Assert.True(result.Pages[2].IsCurrent, "Middle index should be current");
        }

        [Fact]
        public void EndShiftThreeFromEnd()
        {
            var result = _sut.GetMetaData(10, 10, 1);
            Assert.True(result.Pages[4].IsCurrent, "Last index should be current");
            result = _sut.GetMetaData(10, 9, 1);
            Assert.True(result.Pages[3].IsCurrent, "Second from last index should be current");
            result = _sut.GetMetaData(10, 8, 1);
            Assert.True(result.Pages[2].IsCurrent, "Third from last index should be current");
            result = _sut.GetMetaData(10, 7, 1);
            Assert.True(result.Pages[2].IsCurrent, "Middle index should be current");
        }

        [Fact]
        public void PartialEqualsCurrentCount()
        {
            var result = _sut.GetMetaData(2, 2, 1);
            Assert.True(result.Pages.Count == 2, "Count should equal total collection");
        }

        [Fact]
        public void PartialLastIsSelected()
        {
            var result = _sut.GetMetaData(2, 2, 1);
            Assert.True(result.Pages[1].IsCurrent, "Last index should be current");
        }

        [Fact]
        public void PreviousPagePageOneIndexBeforeFirstNumericShown()
        {
            var result = _sut.GetMetaData(100, 6, 10);
            Assert.True(result.PreviousPage.PageNumber == result.Pages.First(x => x.IsCurrent).PageNumber - 1,
                string.Format("Expected: Previous Page number == {0}", result.Pages.First(x => x.IsCurrent).PageNumber - 1));
        }
    }
}