using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MH.UI
{
    [RequireComponent(typeof(ATipsEvtReceiver))]
    public class ATipsTrigger : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField][Tooltip("if null, use the fallback one")]
        private ATips _target;
        [SerializeField][Multiline]
        public string content = "";
        #pragma warning restore 0649

        void Awake()
        {
            enabled = false;
        }

        void Update()
        {
            ATips tip = _target ? _target : ATips.fallback;
            tip.ShowTips(content);
        }

        public void StartTips()
        {
            enabled = true;
            ATips tip = _target ? _target : ATips.fallback;
            tip.ShowTips(content);
        }

        public void EndTips()
        {
            enabled = false;
            ATips tip = _target ? _target : ATips.fallback;
            tip.HideTips();
        }

    }
}
