using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class NotQueryTests
    {
        [Fact]
        public void true_in_inverted()
        {
            var mockGameObject = new Mock<GameObject>();
            var mockChildQuery = new Mock<IQuery>();
            mockChildQuery
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);

            var testObject = new NotQuery(mockChildQuery.Object);
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void false_in_inverted()
        {
            var mockGameObject = new Mock<GameObject>();
            var mockChildQuery = new Mock<IQuery>();
            mockChildQuery
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(false);

            var testObject = new NotQuery(mockChildQuery.Object);
            Assert.True(testObject.Match(mockGameObject.Object));
        }
    }

}
