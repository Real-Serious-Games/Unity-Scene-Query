using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class AndQueryTests
    {
        [Fact]
        public void result_is_true_when_inputs_are_true()
        {
            var mockGameObject = new Mock<GameObject>();
            var child1 = new Mock<IQuery>();
            child1
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);
            var child2 = new Mock<IQuery>();
            child2
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);

            var testObject = new AndQuery(child1.Object, child2.Object);
            Assert.True(testObject.Match(mockGameObject.Object));
        }

        [Fact]
        public void result_is_false_when_either_or_both_inputs_are_false()
        {
            var mockGameObject = new Mock<GameObject>();
            var child1 = new Mock<IQuery>();
            var child2 = new Mock<IQuery>();

            var testObject = new AndQuery(child1.Object, child2.Object);

            child1
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(false);
            child2
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);
            Assert.False(testObject.Match(mockGameObject.Object));

            child1
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);
            child2
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(false);
            Assert.False(testObject.Match(mockGameObject.Object));

            child1
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(false);
            child2
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(false);
            Assert.False(testObject.Match(mockGameObject.Object));
        }
    }
}
