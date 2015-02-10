using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class LayerQueryTests
    {
        [Fact]
        public void matches_when_gameobject_is_on_expected_layer()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(15);

            LayerMask.fakeLayerString = "some-layer";

            var testObject = new LayerQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void doesnt_match_when_gameobject_is_not_on_expected_layer()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(22);

            mockGameObject
                .Setup(m => m.tag)
                .Returns(string.Empty);

            LayerMask.fakeLayerString = "some-other-layer";

            var testObject = new LayerQuery("some-layer");
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void layer_match_is_case_insensitive()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(33);

            LayerMask.fakeLayerString = "some-layer";

            var testObject = new LayerQuery("Some-Layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void matches_when_gameobject_has_expected_tag()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(44);

            mockGameObject
                .Setup(m => m.tag)
                .Returns("some-layer");

            LayerMask.fakeLayerString = string.Empty;

            var testObject = new LayerQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void doesnt_match_when_gameobject_doesnt_have_expected_tag()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(55);

            mockGameObject
                .Setup(m => m.tag)
                .Returns(string.Empty);

            LayerMask.fakeLayerString = string.Empty;            

            var testObject = new LayerQuery("some-layer");
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void tag_match_is_case_insensitive()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.layer)
                .Returns(88);

            mockGameObject
                .Setup(m => m.tag)
                .Returns("some-layer");

            LayerMask.fakeLayerString = string.Empty;

            var testObject = new LayerQuery("Some-Layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

    }
}
