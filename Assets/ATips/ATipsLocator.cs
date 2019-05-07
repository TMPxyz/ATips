using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MH.UI
{
    public abstract class ATipsLocator : MonoBehaviour
    {
        

        protected Transform _tr;

        protected virtual void Awake()
        {
            _tr = transform;
        }

        public abstract void Execute();
    }
}
