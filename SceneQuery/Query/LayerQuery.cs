using RSG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RSG.Scene.Query
{
    /// <summary>
    /// Matches game objects by layer.
    /// </summary>
    public class LayerQuery : IQuery
    {
        private string layerName;

        public LayerQuery(string layerName)
        {
            Argument.StringNotNullOrEmpty(() => layerName);

            this.layerName = layerName.ToLower();
        }

        /// <summary>
        /// Returns true if the gameObject matches the criteria defined by the derived object.
        /// </summary>
        public bool Match(GameObject gameObject)
        {
            var layer = LayerMask.LayerToName(gameObject.layer);

            return
                !string.IsNullOrEmpty(layer) && this.layerName == layer.ToLower() ||
                !string.IsNullOrEmpty(gameObject.tag) && this.layerName == gameObject.tag.ToLower();
        }
    }
}
