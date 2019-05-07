using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

namespace MH.UI
{
    public class ATipsEvtReceiver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ATipsTrigger _trigger;

        void Awake()
        {
            _trigger = GetComponent<ATipsTrigger>();
            Assert.IsNotNull(_trigger);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _trigger.StartTips();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _trigger.EndTips();
        }
    }
}