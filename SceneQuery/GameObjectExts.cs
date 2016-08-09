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

        //
        // Get the root object of the game object
        //
        public static GameObject RootObject(this GameObject gameObject)
        {
            if (gameObject.transform.parent == null)
            {
                //
                // The object itself is the root GameObject.
                return gameObject;
            }

            var rootTransform = gameObject.transform.parent;
            while (rootTransform.parent != null)
            {
                rootTransform = rootTransform.parent;
            }

            return rootTransform.gameObject;
        }

        //
        // Get the distinct collection of root objects for the input collection of GameObjects.
        //
        public static IEnumerable<GameObject> RootObjects(this IEnumerable<GameObject> gameObjects)
        {
            return gameObjects
                .Select(go => go.RootObject())
                .Distinct();
        }

        /// <summary>
        /// Retreive the parent of a game object.
        /// </summary>
        public static GameObject Parent(this GameObject gameObject)
        {
            var transform = gameObject.transform;
            if (transform.parent == null)
            {
                return null;
            }

            return transform.parent.gameObject;
        }

        /// <summary>
        /// Get the collection of children for a particular game object.
        /// </summary>
        public static IEnumerable<GameObject> Children(this GameObject parent)
        {
            return sceneTraversal.Children(parent);
        }

        /// <summary>
        /// Get the collection of all child gameobjects for the input collection of game objects.
        /// </summary>
        public static IEnumerable<GameObject> Children(this IEnumerable<GameObject> source)
        {
            return source.SelectMany(go => go.Children());
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
        /// Get collection of all descendents of the input collection of game objects.
        /// </summary>
        public static IEnumerable<GameObject> Descendents(this IEnumerable<GameObject> source)
        {
            return source.SelectMany(go => go.Descendents());
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
        /// </summary>
        public static GameObject SelectOne(this GameObject parent, string selector)
        {
            return sceneQuery.SelectOne(parent, selector);
        }

        /// <summary>
        /// Select the first child game object with the specifed component.
        /// </summary>
        public static ComponentT SelectOne<ComponentT>(this GameObject parent)
            where ComponentT : Component
        {
            return sceneQuery.SelectOne<ComponentT>(parent);
        }

        /// <summary>
        /// Select the first child game object with the specifed component that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public static ComponentT SelectOne<ComponentT>(this GameObject parent, string selector)
            where ComponentT : Component
        {
            return sceneQuery.SelectOne<ComponentT>(parent, selector);
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public static GameObject ExpectOne(this GameObject parent, string selector)
        {
            return sceneQuery.ExpectOne(parent, selector);
        }

        /// <summary>
        /// Select the first child game object that has the specified component.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public static ComponentT ExpectOne<ComponentT>(this GameObject parent)
            where ComponentT : Component
        {
            return sceneQuery.ExpectOne<ComponentT>(parent);
        }

        /// <summary>
        /// Select the first child game object that has the specified component and matches the specified selector.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public static ComponentT ExpectOne<ComponentT>(this GameObject parent, string selector)
            where ComponentT : Component
        {
            return sceneQuery.ExpectOne<ComponentT>(parent, selector);
        }

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// </summary>
        public static IEnumerable<GameObject> SelectAll(this GameObject parent, string selector)
        {
            return sceneQuery.SelectAll(parent, selector);
        }

        /// <summary>
        /// Select child objects that have the specified component.
        /// </summary>
        public static IEnumerable<ComponentT> SelectAll<ComponentT>(this GameObject parent)
            where ComponentT : Component
        {
            return sceneQuery.SelectAll<ComponentT>(parent);
        }

        /// <summary>
        /// Select child objects that have the specified component and matches the specified selector.
        /// </summary>
        public static IEnumerable<ComponentT> SelectAll<ComponentT>(this GameObject parent, string selector)
            where ComponentT : Component
        {
            return sceneQuery.SelectAll<ComponentT>(parent, selector);
        }
    }
}
