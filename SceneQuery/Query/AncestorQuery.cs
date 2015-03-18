using RSG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches if an ancestor of the game object matches the sub-query.
    /// </summary>
    public class AncestorQuery : IQuery
    {
        /// <summary>
        /// The sub-query.
        /// </summary>
        IQuery query;

        public AncestorQuery(IQuery query)
        {
            Argument.NotNull(() => query);

            this.query = query;
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            var ancestor = gameObject.transform.parent;
            var matchFound = false;

            while (!matchFound && ancestor != null)
            {               
                matchFound = query.Match(ancestor.gameObject);
                ancestor = ancestor.parent;
            }

            return matchFound;
        }
    }
}
