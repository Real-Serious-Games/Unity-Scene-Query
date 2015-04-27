using RSG.Scene.Query.Parser;
using RSG.Utils;
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
            Argument.NotNull(() => sceneTraversal);

            this.sceneTraversal = sceneTraversal;
            this.queryParser = queryParser;
        }

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public GameObject SelectOne(string selector)
        {
            Argument.StringNotNullOrEmpty(() => selector);

            return SelectAll(selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public GameObject ExpectOne(string selector)
        {
            Argument.StringNotNullOrEmpty(() => selector);

            var foundGameObject = SelectOne(selector);
            if (foundGameObject == null)
            {
                throw new ApplicationException("Game object with selector '" + selector + "' was not found.");
            }
            return foundGameObject;

        }

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(string selector)
        {
            Argument.StringNotNullOrEmpty(() => selector);

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
            Argument.NotNull(() => gameObject);
            Argument.StringNotNullOrEmpty(() => selector);

            return SelectAll(gameObject, selector).FirstOrDefault();
        }

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// Throws an exception if there is no game object that matches the selector.
        /// </summary>
        public GameObject ExpectOne(GameObject gameObject, string selector)
        {
            Argument.NotNull(() => gameObject);
            Argument.StringNotNullOrEmpty(() => selector);

            var foundGameObject = SelectOne(gameObject, selector);
            if (foundGameObject == null)
            {
                throw new ApplicationException("Child game object with selector '" + selector + "' was not found.");
            }
            return foundGameObject;
        }

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// </summary>
        public IEnumerable<GameObject> SelectAll(GameObject gameObject, string selector)
        {
            Argument.NotNull(() => gameObject);
            Argument.StringNotNullOrEmpty(() => selector);

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
    }
}
