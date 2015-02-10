using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches game objects by name (using partial name or regex).
    /// </summary>
    public class RegexNameQuery : IQuery
    {
        private Regex regex;

        public RegexNameQuery(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Invalid NameFilter input.", "input");
            }

            try
            {
                this.regex = new Regex(input, RegexOptions.IgnoreCase);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create regular expression from input: " + input, ex);
            }
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            return this.regex.IsMatch(gameObject.name);
        }
    }
}
