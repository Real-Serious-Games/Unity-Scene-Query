using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class IdQueryTests
    {
        [Fact]
        public void test_returns_object_when_unique_id_match()
        {
            var mockGameObject = new Mock<GameObject>();

            var testId = 12345;

            mockGameObject
                .Setup(m => m.GetInstanceID())
                .Returns(testId);

            var testObject = new UniqueIdQuery(testId);
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_does_not_return_object_when_unique_id_do_not_match()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.GetInstanceID())
                .Returns(1234);

            var testObject = new UniqueIdQuery(54321);
            Assert.False(testObject.Match(mockGameObject.Object));
        }
    }
}
