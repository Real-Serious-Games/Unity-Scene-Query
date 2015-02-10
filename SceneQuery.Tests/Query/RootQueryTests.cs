using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RSG.Scene.Query.Tests
{
    public class RootQueryTests
    {
        [Fact]
        public void result_is_true_for_root_object()
        {
            var gameObject = new GameObject();

            var testObject = new RootQuery();
            Assert.True(testObject.Match(gameObject));
        }

        [Fact]
        public void result_is_false_for_non_root_object()
        {
            var childGameObject = new GameObject();
            var parentGameObject = new GameObject();
            childGameObject.transform.parent = parentGameObject.transform;

            var testObject = new RootQuery();
            Assert.False(testObject.Match(childGameObject));
        }
    }
}
