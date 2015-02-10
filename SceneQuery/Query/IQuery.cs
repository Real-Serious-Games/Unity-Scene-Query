using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Used to match a game object against some query criteria.
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        bool Match(GameObject gameObject);
    }
}
