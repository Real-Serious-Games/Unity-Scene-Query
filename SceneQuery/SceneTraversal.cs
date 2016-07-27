using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Scene traversal for Unity.
    /// </summary>
    public interface ISceneTraversal
    {
        /// <summary>
        /// Traverse root game objects in the scene.
        /// </summary>
        IEnumerable<GameObject> RootNodes();

        /// <summary>
        /// Traverse only all game objects breadth first.
        /// </summary>
        IEnumerable<GameObject> BreadthFirst();

        /// <summary>
        /// Traverse games object in the hierarchy in pre-order.
        /// </summary>
        IEnumerable<GameObject> PreOrderHierarchy();

        /// <summary>
        /// Traverse games object in the hierarchy in post-order.
        /// </summary>
        IEnumerable<GameObject> PostOrderHierarchy();

        /// <summary>
        /// Traverse only leaf game objects in scene hierarchy.
        /// </summary>
        IEnumerable<GameObject> HierarchyLeafNodes();

        /// <summary>
        /// Get the collection of children for a particular game object.
        /// </summary>
        IEnumerable<GameObject> Children(GameObject parent);

        /// <summary>
        /// Get collection of all ancestors of a particular game object, starting with the immediate parent and working up to the root object.
        /// </summary>
        IEnumerable<GameObject> Ancestors(GameObject parent);

        /// <summary>
        /// Get collection of all descendents, the entire tree of children under a particular game object.
        /// </summary>
        IEnumerable<GameObject> Descendents(GameObject parent);

        /// <summary>
        /// Traverse only all game objects breadth first.
        /// </summary>
        IEnumerable<GameObject> BreadthFirst(GameObject parent);

        /// <summary>
        /// Get the entire sub-hierarchy in pre-order under a particular game object.
        /// </summary>
        IEnumerable<GameObject> PreOrderHierarchy(GameObject parent);

        /// <summary>
        /// Get the entire sub-hierarchy in post-order under a particular game object.
        /// </summary>
        IEnumerable<GameObject> PostOrderHierarchy(GameObject parent);

        /// <summary>
        /// Get all leaf game objects in the hierarchy under a particular game object.
        /// </summary>
        IEnumerable<GameObject> HierarchyLeafNodes(GameObject parent);
    }

    /// <summary>
    /// Scene traversal for Unity.
    /// </summary>
    public class SceneTraversal : ISceneTraversal
    {
        /// <summary>
        /// Traverse root game objects in the scene.
        /// </summary>
        public IEnumerable<GameObject> RootNodes()
        {
            return SceneManager.GetActiveScene().GetRootGameObjects();
        }

        /// <summary>
        /// Traverse only all game objects breadth first.
        /// </summary>
        public IEnumerable<GameObject> BreadthFirst()
        {
            var rootObjects = this.RootNodes().ToArray();
            return rootObjects.Concat(rootObjects.SelectMany(root => BreadthFirst(root)));
        }

        /// <summary>
        /// Traverse games object in the hierarchy in pre-order.
        /// </summary>
        public IEnumerable<GameObject> PreOrderHierarchy()
        {
            return RootNodes().SelectMany(go => PreOrderHierarchy(go));
        }

        /// <summary>
        /// Traverse games object in the hierarchy in post-order.
        /// </summary>
        public IEnumerable<GameObject> PostOrderHierarchy()
        {
            return RootNodes().SelectMany(go => PostOrderHierarchy(go));
        }

        /// <summary>
        /// Traverse only leaf game objects in scene hierarchy.
        /// </summary>
        public IEnumerable<GameObject> HierarchyLeafNodes()
        {
            return RootNodes().SelectMany(go => HierarchyLeafNodes(go));
        }

        /// <summary>
        /// Get the collection of children for a particular game object.
        /// </summary>
        public IEnumerable<GameObject> Children(GameObject parent)
        {
            var transform = parent.transform;

            foreach (var child in transform.Cast<Transform>())
            {
                yield return child.gameObject;
            }
        }

        /// <summary>
        /// Get collection of all ancestors of a particular game object, starting with the immediate parent and working up to the root object.
        /// </summary>
        public IEnumerable<GameObject> Ancestors(GameObject parent)
        {
            var ancestorTransform = parent.transform.parent;
            while (ancestorTransform != null)
            {
                yield return ancestorTransform.gameObject;
                ancestorTransform = ancestorTransform.parent;
            }
        }

        /// <summary>
        /// Get collection of all descendents, the entire tree of children under a particular game object.
        /// </summary>
        public IEnumerable<GameObject> Descendents(GameObject parent)
        {
            var descendents = Children(parent).SelectMany(child => PreOrderHierarchy(child));
            foreach (var descendent in descendents)
            {
                yield return descendent;
            }
        }

        /// <summary>
        /// Traverse only all game objects breadth first.
        /// </summary>
        public IEnumerable<GameObject> BreadthFirst(GameObject parent)
        {
            var transform = parent.transform;

            foreach (var child in transform.Cast<Transform>())
            {
                yield return child.gameObject;
            }

            foreach (var child in transform.Cast<Transform>())
            {
                foreach (var output in BreadthFirst(child.gameObject))
                {
                    yield return output;
                }
            }
        }

        /// <summary>
        /// Get the entire sub-hierarchy in pre-order under a particular game object.
        /// </summary>
        public IEnumerable<GameObject> PreOrderHierarchy(GameObject parent)
        {
            yield return parent;

            var descendents = Children(parent).SelectMany(child => PreOrderHierarchy(child));
            foreach (var descendent in descendents)
            {
                yield return descendent;
            }
        }

        /// <summary>
        /// Get the entire sub-hierarchy in post-order under a particular game object.
        /// </summary>
        public IEnumerable<GameObject> PostOrderHierarchy(GameObject parent)
        {
            var descendents = Children(parent).SelectMany(child => PostOrderHierarchy(child));
            foreach (var descendent in descendents)
            {
                yield return descendent;
            }

            yield return parent;
        }

        /// <summary>
        /// Get all leaf game objects in the hierarchy under a particular game object.
        /// </summary>
        public IEnumerable<GameObject> HierarchyLeafNodes(GameObject parent)
        {
            foreach (var leafNode in Children(parent).SelectMany(child => HierarchyLeafNodes(child)))
            {
                yield return leafNode;
            }
        }
    }
}
