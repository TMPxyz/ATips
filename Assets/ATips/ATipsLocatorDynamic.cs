using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MH.UI
{
    using ELocation = ATips.ELocation;
    public class ATipsLocatorDynamic : ATipsLocator
    {
#pragma warning disable 0649

        [SerializeField][Tooltip("offset value on x & y axis\npositive value means away from cursor, negative value means closer to cursor")]
        protected Vector2 _offset;
        public Vector2 offset { get { return _offset; } set { _offset = value; } }

        [SerializeField][Tooltip("")]
        protected ELocation _eLoc = ELocation.AutoDetect;
        public ELocation eLoc { get { return _eLoc; } }

#pragma warning restore 0649

        private ATips _tips;
        private ELocation _eRealLoc;

        protected override void Awake()
        {
            base.Awake();

            _tips = GetComponentInParent<ATips>();
            Assert.IsNotNull(_tips);

            _eRealLoc = ELocation.END;
        }

        void OnEnable()
        {
            if( _eLoc == ELocation.AutoDetect )
            {
                _eRealLoc = ELocation.END;
            }
        }

        void LateUpdate()
        {
            Execute();
        }

        public void SetLocation(ELocation eLoc)
        {
            if( _eLoc == eLoc )
                return;
            
            _eLoc = eLoc;
            if( gameObject.activeInHierarchy)
                Execute();
        }

        public override void Execute()
        {
            Vector3 lastPos = Input.mousePosition;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            Vector2 size = _tips.size;

            float x=0, y=0;
            switch( _eLoc )
            {
                case ELocation.TopLeft: (x,y) = _LocationTopLeft(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.TopRight: (x,y) = _LocationTopRight(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.BottomRight: (x,y) = _LocationBottomRight(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.BottomLeft: (x,y) = _LocationBottomLeft(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.AutoDetect: (x,y) = _LocationAutoDetect(lastPos, screenWidth, screenHeight, size); break;
                default: Assert.IsTrue(false, "ATipsLocatorDynamic.Execute: unexpected Location"); break;
            }

            var worldPos = new Vector3(x, y, 0);
            _tr.position = worldPos;
        }

        private (float x, float y) _LocationAutoDetect(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            if( _eRealLoc == ELocation.END )
            {
                if( lastPos.x <= screenWidth * 0.5f )
                    _eRealLoc = lastPos.y <= screenHeight * 0.5f ? ELocation.TopRight : ELocation.BottomRight;
                else
                    _eRealLoc = lastPos.y <= screenHeight * 0.5f ? ELocation.TopLeft : ELocation.BottomLeft;

                AdjustBasedOnELocation(_eRealLoc);
            }

            float x=0, y=0;
            switch(_eRealLoc)
            {
                case ELocation.TopLeft: (x,y)=_DoTopLeft(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.TopRight: (x,y)=_DoTopRight(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.BottomRight: (x,y)=_DoBottomRight(lastPos, screenWidth, screenHeight, size); break;
                case ELocation.BottomLeft: (x,y)=_DoBottomLeft(lastPos, screenWidth, screenHeight, size); break;
            }
            return (x,y);
        }

        private (float x, float y) _LocationBottomLeft(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            if (_eRealLoc != ELocation.BottomLeft)
            {
                _eRealLoc = ELocation.BottomLeft;
                AdjustBasedOnELocation(_eRealLoc);
            }
            return _DoBottomLeft(lastPos, screenWidth, screenHeight, size);
        }

        private (float x, float y) _DoBottomLeft(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            float x = 0, y = 0;
            x = Mathf.Clamp(lastPos.x - _offset.x, size.x, screenWidth);
            y = Mathf.Clamp(lastPos.y - _offset.y, size.y, screenHeight);
            return (x, y);
        }

        private (float x, float y) _LocationBottomRight(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            if (_eRealLoc != ELocation.BottomRight)
            {
                _eRealLoc = ELocation.BottomRight;
                AdjustBasedOnELocation(_eRealLoc);
            }
            return _DoBottomRight(lastPos, screenWidth, screenHeight, size);
        }

        private (float x, float y) _DoBottomRight(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            float x = 0, y = 0;
            x = Mathf.Clamp(lastPos.x + _offset.x, 0, screenWidth - size.x);
            y = Mathf.Clamp(lastPos.y - _offset.y, size.y, screenHeight);
            return (x, y);
        }

        private (float x, float y) _LocationTopRight(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            if (_eRealLoc != ELocation.TopRight)
            {
                _eRealLoc = ELocation.TopRight;
                AdjustBasedOnELocation(_eRealLoc);
            }
            return _DoTopRight(lastPos, screenWidth, screenHeight, size);
        }

        private (float x, float y) _DoTopRight(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            float x = 0, y = 0;
            x = Mathf.Clamp(lastPos.x + _offset.x, 0, screenWidth - size.x);
            y = Mathf.Clamp(lastPos.y + _offset.y, 0, screenHeight - size.y);
            return (x, y);
        }

        private (float x, float y) _LocationTopLeft(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            if (_eRealLoc != ELocation.TopLeft)
            {
                _eRealLoc = ELocation.TopLeft;
                AdjustBasedOnELocation(_eRealLoc);
            }
            return _DoTopLeft(lastPos, screenWidth, screenHeight, size);
        }

        private (float x, float y) _DoTopLeft(Vector3 lastPos, float screenWidth, float screenHeight, Vector2 size)
        {
            float x = 0, y = 0;
            x = Mathf.Clamp(lastPos.x - _offset.x, size.x, screenWidth);
            y = Mathf.Clamp(lastPos.y + _offset.y, 0, screenHeight - size.y);
            return (x, y);
        }

        public void AdjustBasedOnELocation(ELocation eLoc)
        {
            switch(eLoc)
            {
                case ELocation.TopLeft:
                {
                    _tips.horzGroup.childAlignment = TextAnchor.LowerRight;
                    _tips.rectRoot.pivot = new Vector2(1,0);
                }
                break;
                case ELocation.TopRight:
                {
                    _tips.horzGroup.childAlignment = TextAnchor.LowerLeft;
                    _tips.rectRoot.pivot = new Vector2(0,0);
                }
                break;
                case ELocation.BottomRight:
                {
                    _tips.horzGroup.childAlignment = TextAnchor.UpperLeft;
                    _tips.rectRoot.pivot = new Vector2(0,1);
                }
                break;
                case ELocation.BottomLeft:
                {
                    _tips.horzGroup.childAlignment = TextAnchor.UpperRight;
                    _tips.rectRoot.pivot = new Vector2(1,1);
                }
                break;
                default: Assert.IsTrue(false, "ATips.AdjustBasedOnELocation: only accept base 4 loc"); break;
            }
        }
    }
}
