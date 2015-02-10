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
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
        /// </summary>
        GameObject SelectOne(string selector);

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
        /// </summary>
        IEnumerable<GameObject> SelectAll(string selector);

        /// <summary>
        /// Select the first child game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
        /// </summary>
        GameObject SelectOne(GameObject gameObject, string selector);

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
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

        public SceneQuery(ISceneTraversal sceneTraversal, IQueryParser queryParser)
        {
            Argument.NotNull(() => sceneTraversal);

            this.sceneTraversal = sceneTraversal;
            this.queryParser = queryParser;
        }

        /// <summary>
        /// Select the first game object that matches the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
        /// </summary>
        public GameObject SelectOne(string selector)
        {
            Argument.StringNotNullOrEmpty(() => selector);

            return SelectAll(selector).FirstOrDefault();
        }

        /// <summary>
        /// Select object in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<uid>      -> Retreive the object that matches the unique id.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
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
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
        /// </summary>
        public GameObject SelectOne(GameObject gameObject, string selector)
        {
            Argument.NotNull(() => gameObject);
            Argument.StringNotNullOrEmpty(() => selector);

            return SelectAll(gameObject, selector).FirstOrDefault();
        }

        /// <summary>
        /// Select child objects in the scene based on the specified selector.
        /// This is very similar to CSS/JQuery selection.
        /// 
        ///     #<regex>    -> Retreive objects that match the regex.
        ///     .<regex>    -> Retrieve objects whose layer, tag or category matches the regex.
        ///     
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
