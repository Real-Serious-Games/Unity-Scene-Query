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
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        GameObject SelectOne(string selector);

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        GameObject ExpectOne(string selector);

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        IEnumerable<GameObject> SelectAll(string selector);

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        GameObject SelectOne(GameObject gameObject, string selector);

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        GameObject ExpectOne(GameObject gameObject, string selector);

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        IEnumerable<GameObject> SelectAll(GameObject gameObject, string selector);

        /// <summary>
        /// Expect that a component should exist on the single game object that is identified by 'selector'.
        /// </summary>
        ComponentT ExpectComponent<ComponentT>(string selector)
            where ComponentT : Component;

        /// <summary>
        /// Expect that a component should exist on the single game object that is identified by 'selector'.
        /// </summary>
        ComponentT ExpectComponent<ComponentT>(GameObject parentGameObject, string selector)
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

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(string selector)
        {
            try
            {
                var query = queryParser.Parse(selector);
                return sceneTraversal
                    .PreOrderHierarchy()
                    .Where(gameObject => query.Match(gameObject));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception was thrown while processing selector: " + selector + ", searching entire scene", ex);
            }
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
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(GameObject gameObject, string selector)
        {
            try
            {
                var query = queryParser.Parse(selector);
                return sceneTraversal
                    .PreOrderHierarchy(gameObject)
                    .Where(childGameObject => query.Match(childGameObject));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception was thrown while processing selector: " + selector + ", searching under game object " + gameObject.name, ex);
            }
        }

        /// <summary>
        /// Expect that a component should exist on the single game object that is identified by 'selector'.
        /// </summary>
        public ComponentT ExpectComponent<ComponentT>(string selector)
            where ComponentT : Component
        {
            var gameObject = ExpectOne(selector);

            var component = gameObject.GetComponent<ComponentT>();
            if (component == null)
            {
                throw new ApplicationException("Game object " + gameObject.name + " doesnt have component " + typeof(ComponentT).Name + " attached!");
            }

            return component;
        }

        /// <summary>
        /// Expect that a component should exist on the single game object that is identified by 'selector'.
        /// </summary>
        public ComponentT ExpectComponent<ComponentT>(GameObject parentGameObject, string selector)
            where ComponentT : Component
        {
            var gameObject = ExpectOne(parentGameObject, selector);

            var component = gameObject.GetComponent<ComponentT>();
            if (component == null)
            {
                throw new ApplicationException("Game object " + gameObject.name + " doesnt have component " + typeof(ComponentT).Name + " attached!");
            }

            return component;
        }
    }
}
