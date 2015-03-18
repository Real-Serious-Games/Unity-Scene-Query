using RSG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Represents the logical and of two other queries.
    /// </summary>
    public class AndQuery : IQuery
    {
        private IQuery left;
        private IQuery right;

        public AndQuery(IQuery left, IQuery right)
        {
            Argument.NotNull(() => left);
            Argument.NotNull(() => right);

            this.left = left;
            this.right = right;
        }
        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return left.Match(gameObject) && right.Match(gameObject);
        }
    }
}
