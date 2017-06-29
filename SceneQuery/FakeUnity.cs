using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#if TESTING

// 
// Fake version of Unity GameObject and Transform used for unit testing.
// Only enabled when you are in the 'Testing' configuration.
//

namespace RSG.Scene.Query
{
    public static class LayerMask
    {
        public static string fakeLayerString;

        public static string LayerToName(int layer)
        {
            return fakeLayerString;
        }
    }
    

    public class Transform : List<Transform>
    {
        public Transform() 
        {
        }

        public Transform(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual Transform parent { get; set; }

        public virtual GameObject gameObject { get; set; }
    }

    public class Component
    {
        public GameObject gameObject { get; set; }
    }

    public class GameObject
    {
        private Transform _transform;

        public virtual string name { get; set; }
        public virtual int layer { get; set; }
        public virtual string tag { get; set; }

        public virtual Transform transform 
        {
            get
            {
                if (_transform == null)
                {
                    _transform = new Transform(this);
                }

                return _transform;
            }
        }

        public virtual int GetInstanceID()
        {
            return 0;
        }

        public static T[] FindObjectsOfType<T>()
        {
            return new T[0];
        }

        public virtual ComponentT GetComponent<ComponentT>() 
            where ComponentT : class
        {
            return null;
        }

        internal static ComponentT FindObjectOfType<ComponentT>()
        {
            return (ComponentT)(object)null;            
        }

        internal IEnumerable<ComponentT> GetComponentsInChildren<ComponentT>() 
        {
            return new ComponentT[0];
        }

        internal ComponentT GetComponentInChildren<ComponentT>()
        {
            return (ComponentT)(object)null;
        }
    }
}

#endif // _TESTING