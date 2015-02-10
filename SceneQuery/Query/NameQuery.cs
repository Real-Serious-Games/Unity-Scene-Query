using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches game objects by name.
    /// </summary>
    public class NameQuery : IQuery
    {
        /// <summary>
        /// The exact name of scene objects being searched for.
        /// </summary>
        private string exactName;

        public NameQuery(string exactName)
        {
            if (string.IsNullOrEmpty(exactName))
            {
                throw new ArgumentException("Invalid NameFilter exactName.", "exactName");
            }

            this.exactName = exactName.ToLower();
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return this.exactName == gameObject.name.ToLower();
        }
    }
}
