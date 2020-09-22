using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuxi.DanceGameKit
{
    public class DestroyOnFinishPlay : MonoBehaviour
    {
        Animator m;
        // Start is called before the first frame update
        void Start()
        {
            m = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var s = m.GetCurrentAnimatorStateInfo(0);
            if (s.normalizedTime > s.length)
            {
                Destroy(gameObject);
            }
        }
    }
}
