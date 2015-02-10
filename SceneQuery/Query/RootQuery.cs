using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches a root object only.
    /// </summary>
    public class RootQuery : IQuery
    {
        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return gameObject.transform.parent == null;
        }
    }
}
