using RSG.Scene.Query.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Interface to query the scene.
    /// </summary>
    public interface ISceneQuery
    {
        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// </summary>
        GameObject SelectOne(string selector);

        /// <summary>
        /// Select the first game object that has the specified component.
        /// Returns null if none found.
        /// </summary>
        ComponentT SelectOne<ComponentT>()
            where ComponentT : Component;

        /// <summary>
        /// Select the first game object that has the specified component and matches the specified selector.
        /// Returns null if none found.
        /// </summary>
        ComponentT SelectOne<ComponentT>(string selector)
            where ComponentT : Component;

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        GameObject ExpectOne(string selector);

        /// <summary>
        /// Get the first game object that has the specified component.
        /// Throws an exception if no such game object exists.
        /// </summary>
        ComponentT ExpectOne<ComponentT>()
            where ComponentT : Component;

        /// <summary>
        /// Get the first game object that has the specified component and matches the specified selector.
        /// Throws an exception if no such game object exists.
        /// </summary>
        ComponentT ExpectOne<ComponentT>(string selector)
            where ComponentT : Component;

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// </summary>
        IEnumerable<GameObject> SelectAll(string selector);

        /// <summary>
        /// Select all game objects that have the specified component.
        /// </summary>
        IEnumerable<ComponentT> SelectAll<ComponentT>()
            where ComponentT : Component;

        /// <summary>
        /// Select all game objects that have the specified component and match the specified selector.
        /// </summary>
        IEnumerable<ComponentT> SelectAll<ComponentT>(string selector)
            where ComponentT : Component;

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// </summary>
        GameObject SelectOne(GameObject gameObject, string selector);

        /// <summary>
        /// Select the first child game object that has the specified component.
        /// Returns null if no such game object exists.
        /// </summary>
        ComponentT SelectOne<ComponentT>(GameObject gameObject)
            where ComponentT : Component;

        /// <summary>
        /// Select the first child game object that has the specified component and matches the specified selector.
        /// Returns null if no such game object exists.
        /// </summary>
        ComponentT SelectOne<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component;

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        GameObject ExpectOne(GameObject gameObject, string selector);

        /// <summary>
        /// Select the first child game object that has the specified component.
        /// Throws an exception if no such game object esists.
        /// </summary>
        ComponentT ExpectOne<ComponentT>(GameObject gameObject)
            where ComponentT : Component;

        /// <summary>
        /// Select the first child game object that has the specified component and matches the specified selector.
        /// Throws an exception if no such game object esists.
        /// </summary>
        ComponentT ExpectOne<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component;

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// </summary>
        IEnumerable<GameObject> SelectAll(GameObject gameObject, string selector);

        /// <summary>
        /// Select child objects that have the specified component.
        /// </summary>
        IEnumerable<ComponentT> SelectAll<ComponentT>(GameObject gameObject)
            where ComponentT : Component;

        /// <summary>
        /// Select child objects that have the specified component and match the specified selector
        /// </summary>
        IEnumerable<ComponentT> SelectAll<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component;
    }

    /// <summary>
    /// Interface to query the scene.
    /// </summary>
    public class SceneQuery : ISceneQuery
    {
        private ISceneTraversal sceneTraversal;
        private IQueryParser queryParser;

        /// <summary>
        /// Constructor for normal use.
        /// </summary>
        public SceneQuery()
        {
            this.sceneTraversal = new SceneTraversal();
            this.queryParser = new QueryParser(new QueryTokenizer());
        }

        /// <summary>
        /// Constructor for testing, allows dependencies to be injected.
        /// </summary>
        public SceneQuery(ISceneTraversal sceneTraversal, IQueryParser queryParser)
        {
            this.sceneTraversal = sceneTraversal;
            this.queryParser = queryParser;
        }

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public GameObject SelectOne(string selector)
        {
            return SelectAll(selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first game object that has the specified component.
        /// Returns null if none found.
        /// </summary>
        public ComponentT SelectOne<ComponentT>()
            where ComponentT : Component
        {
            return GameObject.FindObjectOfType<ComponentT>();
        }

        /// <summary>
        /// Select the first game object that has the specified component and matches the specified selector.
        /// Returns null if none found.
        /// </summary>
        public ComponentT SelectOne<ComponentT>(string selector)
            where ComponentT : Component
        {
            return SelectAll<ComponentT>(selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public GameObject ExpectOne(string selector)
        {
            var gameObjects = SelectAll(selector);
            if (!gameObjects.Any())
            {
                throw new ApplicationException("Game object with selector '" + selector + "' was not found.");
            }

            if (gameObjects.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by selector '" + selector + "'.");
            }

            return gameObjects.First();
        }

        /// Get the first game object that has the specified component.
        /// Throws an exception if no such game object exists.
        /// </summary>
        public ComponentT ExpectOne<ComponentT>()
            where ComponentT : Component
        {
            var components = SelectAll<ComponentT>();
            if (!components.Any())
            {
                throw new ApplicationException("Game object with component '" + typeof(ComponentT).Name + "' was not found.");
            }

            if (components.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by component '" + typeof(ComponentT).Name + "'.");
            }

            return components.First();
        }

        /// <summary>
        /// Get the first game object that has the specified component and matches the specified selector.
        /// Throws an exception if no such game object exists.
        /// </summary>
        public ComponentT ExpectOne<ComponentT>(string selector)
            where ComponentT : Component
        {
            var components = SelectAll<ComponentT>(selector);
            if (!components.Any())
            {
                throw new ApplicationException("Game object with component '" + typeof(ComponentT).Name + "' and selector '" + selector + "' was not found.");
            }

            if (components.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by component '" + typeof(ComponentT).Name + "' and selector '" + selector + "'.");
            }

            return components.First();
        }

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(string selector)
        {
            try
            {
                return Filter(sceneTraversal.PreOrderHierarchy(), selector);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception was thrown while processing selector: " + selector + ", searching entire scene", ex);
            }
        }

        /// <summary>
        /// Select all game objects that have the specified component.
        /// </summary>
        public IEnumerable<ComponentT> SelectAll<ComponentT>()
            where ComponentT : Component
        {
            return GameObject.FindObjectsOfType<ComponentT>();
        }

        /// <summary>
        /// Select all game objects that have the specified component and match the specified selector.
        /// </summary>
        public IEnumerable<ComponentT> SelectAll<ComponentT>(string selector)
            where ComponentT : Component
        {
            var query = queryParser.Parse(selector);
            return GameObject.FindObjectsOfType<ComponentT>()
                .Where(component => query.Match(component.gameObject));
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public GameObject SelectOne(GameObject gameObject, string selector)
        {
            return SelectAll(gameObject, selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first child game object that has the specified component.
        /// Returns null if no such game object exists.
        /// </summary>
        public ComponentT SelectOne<ComponentT>(GameObject gameObject)
            where ComponentT : Component
        {
            return gameObject.GetComponentInChildren<ComponentT>();
        }

        /// <summary>
        /// Select the first child game object that has the specified component and matches the specified selector.
        /// Returns null if no such game object exists.
        /// </summary>
        public ComponentT SelectOne<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component
        {
            return SelectAll<ComponentT>(gameObject, selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public GameObject ExpectOne(GameObject parentGameObject, string selector)
        {
            var gameObjects = SelectAll(parentGameObject, selector);
            if (!gameObjects.Any())
            {
                throw new ApplicationException("Game object with selector '" + selector + "' was not found.");
            }

            if (gameObjects.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by selector '" + selector + "'.");
            }

            return gameObjects.First();
        }

        /// <summary>
        /// Select the first child game object that has the specified component.
        /// Throws an exception if no such game object esists.
        /// </summary>
        public ComponentT ExpectOne<ComponentT>(GameObject gameObject)
            where ComponentT : Component
        {
            var components = SelectAll<ComponentT>(gameObject);
            if (!components.Any())
            {
                throw new ApplicationException("Game object with component '" + typeof(ComponentT).Name + "' was not found under '" + gameObject.name + "'.");
            }

            if (components.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by component '" + typeof(ComponentT).Name + "' under '" + gameObject.name + "'.");
            }

            return components.First();
        }

        /// <summary>
        /// Select the first child game object that has the specified component and matches the specified selector.
        /// Throws an exception if no such game object esists.
        /// </summary>
        public ComponentT ExpectOne<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component
        {
            var components = SelectAll<ComponentT>(gameObject, selector);
            if (!components.Any())
            {
                throw new ApplicationException("Game object with component '" + typeof(ComponentT).Name + "' and selector '" + selector + "' was not found under '" + gameObject.name + "'.");
            }

            if (components.Skip(1).Any())
            {
                throw new ApplicationException("Expected only a single game object to be identified by component '" + typeof(ComponentT).Name + "' and selector '" + selector + "' under '" + gameObject.name + "'.");
            }

            return components.First();
        }

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(GameObject gameObject, string selector)
        {
            try
            {
                return Filter(sceneTraversal.PreOrderHierarchy(gameObject), selector);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception was thrown while processing selector: " + selector + ", searching under game object " + gameObject.name, ex);
            }
        }

        /// <summary>
        /// Select child objects that have the specified component.
        /// </summary>
        public IEnumerable<ComponentT> SelectAll<ComponentT>(GameObject gameObject)
            where ComponentT : Component
        {
            return gameObject.GetComponentsInChildren<ComponentT>();
        }

        /// <summary>
        /// Select child objects that have the specified component and match the specified selector
        /// </summary>
        public IEnumerable<ComponentT> SelectAll<ComponentT>(GameObject gameObject, string selector)
            where ComponentT : Component
        {
            var query = queryParser.Parse(selector);
            return gameObject
                .GetComponentsInChildren<ComponentT>()
                .Where(component => query.Match(component.gameObject))
                ;
        }

        /// <summary>
        /// Filter game objects by the specified selector.
        /// </summary>
        public IEnumerable<GameObject> Filter(IEnumerable<GameObject> gameObjects, string selector)
        {
            var query = queryParser.Parse(selector);
            return gameObjects.Where(gameObject => query.Match(gameObject));
        }
    }
}
