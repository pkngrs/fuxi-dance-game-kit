using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuxi.DanceGameKit
{
    public class Utility : MonoBehaviour
    {
        public static void SetMaterial(GameObject target, Material mat)
        {
            var renderers = target.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                r.sharedMaterial = mat;
            }
        }
    }
}
