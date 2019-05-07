using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

// using TEXT = TMPro.TextMeshProUGUI;
using TEXT = UnityEngine.UI.Text;

namespace MH.UI
{
    public class ATips : MonoBehaviour
    {
        ///<summary>
        /// the location of tips relative to cursor
        ///</summary>
        public enum ELocation 
        {
            TopLeft,
            TopRight,
            BottomRight,
            BottomLeft,
            AutoDetect, //based on position
            END,
        }

        #pragma warning disable 0649
        [SerializeField][Tooltip("")]
        private bool _asFallback = false;

        [SerializeField][Tooltip("")]
        protected HorizontalLayoutGroup _horzGroup;
        public HorizontalLayoutGroup horzGroup => _horzGroup;
        [SerializeField][Tooltip("")]
        protected RectTransform _rectRoot;
        public RectTransform  rectRoot => _rectRoot;
        [SerializeField][Tooltip("")]
        private RectTransform _rectBackground;
        public RectTransform rectBackground => _rectBackground;

        [SerializeField][Tooltip("")]
        protected TEXT _lblText;
        #pragma warning restore 0649
        
        private static ATips _fallback;
        public static ATips fallback { get { return _fallback; } }

        private ATipsLocator _locator;
        public Vector2 size => _rectBackground.rect.size;

        void Awake()
        {
            Assert.IsNotNull(_lblText);
            Assert.IsNotNull(_horzGroup);
            Assert.IsNotNull(_rectRoot);
            Assert.IsNotNull(_rectBackground);

            if (_asFallback)
            {
                if (fallback != this && fallback )
                    Debug.LogWarning("ATips.Awake: multiple ATips is marked as fallback", this);
                else
                    _fallback = this;
            }

            _locator = this.GetComponentInChildren<ATipsLocator>();
            Assert.IsNotNull(_locator);
            HideTips();
        }


        public virtual void HideTips()
        {
            gameObject.SetActive(false);
        }

        public virtual void ShowTips(string s)
        {
            gameObject.SetActive(true);
            _lblText.text = s;
            _locator.Execute();
        }

        
    }
}

