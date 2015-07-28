using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Extensions to GameObject for scene traversal and query.
    /// </summary>
    public static class GameObjectExts
    {
        private static SceneTraversal sceneTraversal = new SceneTraversal();
        private static SceneQuery sceneQuery = new SceneQuery();

        /// <summary>
        /// Get the collection of children for a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> Children(this GameObject parent)
        {
            return sceneTraversal.Children(parent);
        }

        /// <summary>
        /// Get collection of all ancestors of a particular game object, starting with the immediate parent and working up to the root object.
        /// </summary>
        public static IEnumerable<GameObject> Ancestors(this GameObject parent)
        {
            return sceneTraversal.Ancestors(parent);
        }

        /// <summary>
        /// Get collection of all descendents, the entire tree of children under a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> Descendents(this GameObject parent)
        {
            return sceneTraversal.Descendents(parent);
        }

        /// <summary>
        /// Traverse only all game objects breadth first.
        /// </summary>
        public static IEnumerable<GameObject> BreadthFirst(this GameObject parent)
        {
            return sceneTraversal.BreadthFirst(parent);
        }

        /// <summary>
        /// Get the entire sub-hierarchy in pre-order under a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> PreOrderHierarchy(this GameObject parent)
        {
            return sceneTraversal.PreOrderHierarchy(parent);
        }

        /// <summary>
        /// Get the entire sub-hierarchy in post-order under a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> PostOrderHierarchy(this GameObject parent)
        {
            return sceneTraversal.PostOrderHierarchy(parent);
        }

        /// <summary>
        /// Get all leaf game objects in the hierarchy under a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> HierarchyLeafNodes(this GameObject parent)
        {
            return sceneTraversal.HierarchyLeafNodes(parent);
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public static GameObject SelectOne(this GameObject parent, string selector)
        {
            return sceneQuery.SelectOne(parent, selector);
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public static GameObject ExpectOne(this GameObject parent, string selector)
        {
            return sceneQuery.ExpectOne(parent, selector);
        }

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public static IEnumerable<GameObject> SelectAll(this GameObject parent, string selector)
        {
            return sceneQuery.SelectAll(parent, selector);
        }

        /// <summary>
        /// Expect that a component should exist on the single game object that is identified by 'selector'.
        /// </summary>
        public static ComponentT ExpectComponent<ComponentT>(this GameObject parent, string selector)
            where ComponentT : Component
        {
            return sceneQuery.ExpectComponent<ComponentT>(parent, selector);
        }
    }
}
