using Moq;
using RSG.Scene.Query.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
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
                .Returns(FromItems(mockGameObject1.Object, mockGameObject2.Object));

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
                .Returns(FromItems(mockGameObject1.Object, mockGameObject2.Object));

            Assert.Equal(mockGameObject1.Object, testObject.SelectOne(selector));
        }

        [Fact]
        public void expect_one_throws_exception_on_no_matching_game_objects()
        {
            Init();

            var selector = "some-selector";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(Enumerable.Empty<GameObject>());

            Assert.Throws<ApplicationException>(() => testObject.ExpectOne(selector));
        }

        [Fact]
        public void expect_one_throws_exception_on_no_matching_child_game_objects()
        {
            Init();

            var mockParentGameObject = new Mock<GameObject>();

            var selector = "some-selector";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy(mockParentGameObject.Object))
                .Returns(Enumerable.Empty<GameObject>());

            Assert.Throws<ApplicationException>(() => testObject.ExpectOne(mockParentGameObject.Object, selector));
        }

        [Fact]
        public void expect_one_throws_exception_on_multiple_matching_game_objects()
        {
            Init();

            var selector = "some-selector";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            var mockGameObject1 = new Mock<GameObject>();
            var mockGameObject2 = new Mock<GameObject>();
            mockQuery
                .Setup(m => m.Match(mockGameObject1.Object))
                .Returns(true);
            mockQuery
                .Setup(m => m.Match(mockGameObject2.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(FromItems(mockGameObject1.Object, mockGameObject2.Object));

            Assert.Throws<ApplicationException>(() => testObject.ExpectOne(selector));
        }

        [Fact]
        public void expect_one_throws_exception_on_multiple_matching_child_game_objects()
        {
            Init();

            var mockParentGameObject = new Mock<GameObject>();

            var selector = "some-selector";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            var mockGameObject1 = new Mock<GameObject>();
            var mockGameObject2 = new Mock<GameObject>();
            mockQuery
                .Setup(m => m.Match(mockGameObject1.Object))
                .Returns(true);
            mockQuery
                .Setup(m => m.Match(mockGameObject2.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy(mockParentGameObject.Object))
                .Returns(FromItems(mockGameObject1.Object, mockGameObject2.Object));

            Assert.Throws<ApplicationException>(() => testObject.ExpectOne(mockParentGameObject.Object, selector));
        }

        [Fact]
        public void can_filter_collection_of_game_objects()
        {
            Init();

            var selector = "some-selector";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            var mockGameObject1 = new Mock<GameObject>();
            var mockGameObject2 = new Mock<GameObject>();
            var mockGameObject3 = new Mock<GameObject>();
            mockQuery
                .Setup(m => m.Match(mockGameObject1.Object))
                .Returns(false);
            mockQuery
                .Setup(m => m.Match(mockGameObject2.Object))
                .Returns(true);
            mockQuery
                .Setup(m => m.Match(mockGameObject3.Object))
                .Returns(false);

            var output = testObject.Filter(FromItems(mockGameObject1.Object, mockGameObject2.Object, mockGameObject3.Object), selector).ToArray();
            Assert.Equal(1, output.Length);
            Assert.Equal(mockGameObject2.Object, output[0]);
        }

        /// <summary>
        /// Convert a variable length argument list of items to an enumerable.
        /// </summary>
        internal static IEnumerable<T> FromItems<T>(params T[] items)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        public class MyComponent : Component
        {

        }

        /*fio:
        [Fact]
        public void expect_component_can_retrieve_component()
        {
            Init();

            var mockGameObject = new Mock<GameObject>();
            var component = new MyComponent();
            mockGameObject
                .Setup(m => m.GetComponent<MyComponent>())
                .Returns(component);            

            var selector = "foo";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockQuery
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(LinqExts.FromItems(mockGameObject.Object));

            Assert.Equal(component, testObject.ExpectComponent<MyComponent>(selector));
        }

        [Fact]
        public void expect_component_throws_when_component_doesnt_exist()
        {
            Init();

            var mockGameObject = new Mock<GameObject>();
            mockGameObject
                .Setup(m => m.GetComponent<MyComponent>())
                .Returns((MyComponent)null);

            var selector = "foo";
            var mockQuery = new Mock<IQuery>();
            mockQueryParser
                .Setup(m => m.Parse(selector))
                .Returns(mockQuery.Object);

            mockQuery
                .Setup(m => m.Match(mockGameObject.Object))
                .Returns(true);

            mockSceneTraversal
                .Setup(m => m.PreOrderHierarchy())
                .Returns(LinqExts.FromItems(mockGameObject.Object));

            Assert.Throws<ApplicationException>(() => testObject.ExpectComponent<MyComponent>(selector));
        }
        */
    }
}
