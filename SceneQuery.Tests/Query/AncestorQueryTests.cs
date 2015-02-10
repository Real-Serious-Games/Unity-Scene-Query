using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class AncestorQueryTests
    {
        [Fact]
        public void result_is_false_when_no_parent()
        {
            var childGameObject = new GameObject();

            var mockSubQuery = new Mock<IQuery>(MockBehavior.Strict);

            var testObject = new AncestorQuery(mockSubQuery.Object);
            Assert.False(testObject.Match(childGameObject));
        }

        [Fact]
        public void result_is_true_when_parent_matches_sub_query()
        {
            var parentGameObject = new GameObject();
            var childGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var mockSubQuery = new Mock<IQuery>();
            mockSubQuery
                .Setup(m => m.Match(parentGameObject))
                .Returns(true);

            var testObject = new AncestorQuery(mockSubQuery.Object);
            Assert.True(testObject.Match(childGameObject));
        }

        [Fact]
        public void result_is_true_when_ancestor_matches_sub_query()
        {
            var childGameObject = new GameObject();
            var parentGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var ancestorGameObject = new GameObject();
            parentGameObject.transform.parent = ancestorGameObject.transform;

            var mockSubQuery = new Mock<IQuery>();
            mockSubQuery
                .Setup(m => m.Match(parentGameObject))
                .Returns(false);
            mockSubQuery
                .Setup(m => m.Match(ancestorGameObject))
                .Returns(true);

            var testObject = new AncestorQuery(mockSubQuery.Object);
            Assert.True(testObject.Match(childGameObject));
        }

        [Fact]
        public void result_is_false_when_ancestor_doesnt_match_sub_query()
        {
            var childGameObject = new GameObject();
            var parentGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var ancestorGameObject = new GameObject();
            parentGameObject.transform.parent = ancestorGameObject.transform;

            var mockSubQuery = new Mock<IQuery>();
            mockSubQuery
                .Setup(m => m.Match(parentGameObject))
                .Returns(false);
            mockSubQuery
                .Setup(m => m.Match(ancestorGameObject))
                .Returns(false);

            var testObject = new AncestorQuery(mockSubQuery.Object);
            Assert.False(testObject.Match(childGameObject));
        }
    }
}
