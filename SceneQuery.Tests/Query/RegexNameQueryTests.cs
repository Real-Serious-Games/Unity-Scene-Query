using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class RegexNameQueryTests
    {
        [Fact]
        public void test_name_match_when_gameobject_has_the_expected_name()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.name)
                .Returns("some-layer");

            var testObject = new RegexNameQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_name_doesnt_match_when_gameobject_doesnt_have_the_expected_name()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.name)
                .Returns("some-other-layer");

            var testObject = new RegexNameQuery("some-layer");
            Assert.False(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_can_match_name_with_regex()
        {
            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.name)
                .Returns("some-layer");

            var testObject = new RegexNameQuery(".*");
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void test_can_match_partial_name()
        {
            var mockGameObject1 = new Mock<GameObject>();
            var mockGameObject2 = new Mock<GameObject>();

            mockGameObject1
                .Setup(m => m.name)
                .Returns("some-layer");

            mockGameObject2
                .Setup(m => m.name)
                .Returns("another-layer");

            var testObject = new RegexNameQuery("s.*");
            Assert.True(testObject.Match(mockGameObject1.Object));
            Assert.False(testObject.Match(mockGameObject2.Object));
        }

        [Fact]
        public void test_name_filter_is_case_insensitive()
        {
            var mockGameObject = new Mock<GameObject>();

            mockGameObject
                .Setup(m => m.name)
                .Returns("some-layer");

            var testObject = new RegexNameQuery("some-layer");
            Assert.True(testObject.Match(mockGameObject.Object));
        }
    }
}
