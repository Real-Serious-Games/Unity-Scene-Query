using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class RegexLayerQueryTests
    {
        [Fact]
        public void test_layer_match_when_gameobject_has_the_expected_layer()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.layer)
                .Returns(10);

            LayerMask.fakeLayerString = "foo";

            var testObject = new RegexLayerQuery("foo");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_layer_doesnt_match_when_gameobject_doesnt_have_the_expected_layer()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.layer)
                .Returns(10);
            mockGameObject
                .Setup(m => m.tag)
                .Returns(string.Empty);

            LayerMask.fakeLayerString = "feg";

            var testObject = new RegexLayerQuery("foo");
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_can_match_layer_with_regex()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.layer)
                .Returns(10);

            LayerMask.fakeLayerString = "foo";

            var testObject = new RegexLayerQuery(".*");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_can_match_partial_layer()
        {
            var mockGameObject1 = new Mock<GameObject>();
            mockGameObject1
                .Setup(m => m.layer)
                .Returns(11);
            mockGameObject1
                .Setup(m => m.tag)
                .Returns(string.Empty);

            var mockGameObject2 = new Mock<GameObject>();
            mockGameObject2
                .Setup(m => m.layer)
                .Returns(22);
            mockGameObject2
                .Setup(m => m.tag)
                .Returns(string.Empty);

            var testObject = new RegexLayerQuery("f.*");

            LayerMask.fakeLayerString = "foo";

            Assert.True(testObject.Match(mockGameObject1.Object));

            LayerMask.fakeLayerString = "bar";

            Assert.False(testObject.Match(mockGameObject2.Object));
        }

        [Fact]
        public void test_layer_match_is_case_insensitive()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.layer)
                .Returns(13);

            LayerMask.fakeLayerString = "foo";

            var testObject = new RegexLayerQuery("Foo");
            Assert.True(testObject.Match(mockGameObject.Object));
        }
    }
}
