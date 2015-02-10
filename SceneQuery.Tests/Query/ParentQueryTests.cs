using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class ParentQueryTests
    {
        [Fact]
        public void result_is_false_when_no_parent()
        {
            var gameObject = new GameObject();

            var mockSubQuery = new Mock<IQuery>(MockBehavior.Strict);

            var testObject = new ParentQuery(mockSubQuery.Object);
            Assert.False(testObject.Match(gameObject));
        }

        [Fact]
        public void result_is_true_when_sub_query_matches_parent()
        {
            var childGameObject = new GameObject();
            var parentGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var mockSubQuery = new Mock<IQuery>();
            mockSubQuery
                .Setup(m => m.Match(parentGameObject))
                .Returns(true);

            var testObject = new ParentQuery(mockSubQuery.Object);
            Assert.True(testObject.Match(childGameObject));
        }

        [Fact]
        public void result_is_false_when_sub_query_doesnt_matches_parent()
        {
            var childGameObject = new GameObject();
            var parentGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var mockSubQuery = new Mock<IQuery>();
            mockSubQuery
                .Setup(m => m.Match(parentGameObject))
                .Returns(false);

            var testObject = new ParentQuery(mockSubQuery.Object);
            Assert.False(testObject.Match(childGameObject));
        }
    }
}
