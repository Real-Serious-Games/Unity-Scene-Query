using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches if the parent of the game object matches the sub-query.
    /// </summary>
    public class ParentQuery : IQuery
    {
        /// <summary>
        /// The sub-query.
        /// </summary>
        IQuery query;

        public ParentQuery(IQuery query)
        {
            this.query = query;
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            if (gameObject.transform.parent == null)
            {
                return false;
            }

            return query.Match(gameObject.transform.parent.gameObject);
        }
    }
}
