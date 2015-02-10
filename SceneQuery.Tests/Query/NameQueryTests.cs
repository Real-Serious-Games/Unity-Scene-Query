using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class NameQueryTests
    {
        [Fact]
        public void test_name_match_when_gameobject_has_the_expected_name()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.name)
                .Returns("some-layer");

            var testObject = new NameQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_name_doesnt_match_when_gameobject_doesnt_have_the_expected_name()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.name)
                .Returns("some-other-layer");

            var testObject = new NameQuery("some-layer");
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_name_filter_is_case_insensitive()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.name)
                .Returns("some-layer");

            var testObject = new NameQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }
    }
}
