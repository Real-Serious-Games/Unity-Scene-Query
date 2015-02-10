
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches game objects by id.
    /// </summary>
    public class UniqueIdQuery : IQuery
    {
        private int uniqueId;

        public UniqueIdQuery(int uniqueId)
        {
            this.uniqueId = uniqueId;
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return gameObject.GetInstanceID() == this.uniqueId;
        }
    }
}
