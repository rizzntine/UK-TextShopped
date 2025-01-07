using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UK_TextShopped
{
    public static class TransformExtensions
    {
        public static Transform FindDeep(this Transform parent, string name) 
        {
            Transform result = parent.Find(name);
            if (result != null) return result;

            foreach (Transform child in parent) 
            {
                result = child.FindDeep(name);
                if(result != null) return result;
            }
            return null;
        }
    }
}
