using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Inverts the logic of a child query.
    /// </summary>
    public class NotQuery : IQuery
    {
        /// <summary>
        /// Defines game objects that are to be excluded.
        /// Only public for unit testing.
        /// </summary>
        public IQuery ChildQuery { get; private set; }

        public NotQuery(IQuery childQuery)
        {
            this.ChildQuery = childQuery;
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return !ChildQuery.Match(gameObject);
        }
    }
}
