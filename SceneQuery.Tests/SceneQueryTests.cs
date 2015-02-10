using Moq;
using RSG.Scene.Query.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class SceneQueryTests
    {
        Mock<ISceneTraversal> mockSceneTraversal;
        Mock<IQueryParser> mockQueryParser;

        SceneQuery testObject;

        void Init()
        {
            mockSceneTraversal = new Mock<ISceneTraversal>();
            mockQueryParser = new Mock<IQueryParser>();

            testObject = new SceneQuery(mockSceneTraversal.Object, mockQueryParser.Object);
        }

        [Fact]
        public void select_all_passes_objects_through_filter_created_by_parser()
        {
            Init();

            var selector = "foo";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            var mockGameObject1 = new Mock<GameObject>();
            mockQuery
                .Setup(m => m.Match(mockGameObject1.Object))
                .Returns(false);

            var mockGameObject2 = new Mock<GameObject>();
            mockQuery
                .Setup(m => m.Match(mockGameObject2.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(new GameObject[] { mockGameObject1.Object, mockGameObject2.Object });

            var testOutput = new GameObject[] { mockGameObject2.Object };
            Assert.Equal(testOutput, testObject.SelectAll(selector));
        }

        [Fact]
        public void select_one_returns_null_when_there_is_no_input()
        {
            Init();

            var selector = "foo";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(new GameObject[0]);

            Assert.Null(testObject.SelectOne(selector));
        }

        [Fact]
        public void select_one_returns_first_item_from_select_all()
        {
            Init();

            var mockGameObject1 = new Mock<GameObject>();
            var mockGameObject2 = new Mock<GameObject>();
            var testInput = new GameObject[] { mockGameObject1.Object, mockGameObject2.Object };
            var testOutput = new GameObject[] { mockGameObject1.Object, mockGameObject2.Object };

            var selector = "foo";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockQuery
                .Setup(m => m.Match(mockGameObject1.Object))
                .Returns(true);
            mockQuery
                .Setup(m => m.Match(mockGameObject2.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(testInput);

            Assert.Equal(mockGameObject1.Object, testObject.SelectOne(selector));
        }
    }
}
