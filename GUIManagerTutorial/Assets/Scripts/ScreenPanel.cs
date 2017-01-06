using UnityEngine;
using DG.Tweening;
using System;

namespace The_A_Drain.GUI
{

    public class ScreenPanel : MonoBehaviour
    {

        protected RectTransform _rectTransform;

        protected const string _WillShowScreenEvent = "OnWillShowScreen";
        protected const string _ShowScreenDoneEvent = "OnShowScreenDone";

        protected const string _WillHideScreenEvent = "OnWillHideScreen";
        protected const string _HideScreenDoneEvent = "OnHideScreenDone";

        // Use this for initialization
        public virtual void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                Debug.LogError("Error: No Rect Transform found on GameObject ' " + gameObject.name + "'");
            }

            HideScreenImmediate();
        }

        public virtual void ShowScreen(float delay = 0f, float duration = 0.5f)
        {
            gameObject.SetActive(true);
            float widthOffset = _rectTransform.rect.width;
            Vector3 startPosition = new Vector3(widthOffset, 0, 0);
            _rectTransform.localPosition = startPosition;

            // tween the panel to the desired position
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delay);
            seq.Append(transform.DOLocalMoveX(0, duration));
            seq.AppendCallback(ShowScreenDone);

            // Inform any screen child objects that we are about to show the screen (screen NOT visible at this point!)
            BroadcastMessage(_WillShowScreenEvent, SendMessageOptions.DontRequireReceiver);
        }

        protected virtual void ShowScreenDone()
        {
            Debug.Log("ShowScreenDone: ");

            // Inform any screen child objects that the screen is now shown and visible
            BroadcastMessage(_ShowScreenDoneEvent, SendMessageOptions.DontRequireReceiver);
        }

        public virtual void ShowScreenImmediate()
        {
            // Inform any screen child objects that we are about to show the screen (screen NOT visible at this point!)
            BroadcastMessage(_WillShowScreenEvent, SendMessageOptions.DontRequireReceiver);

            _rectTransform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            ShowScreenDone();
        }

        public virtual void HideScreen(float duration = 0.5f, Action onDoneCallback = null)
        {
            if (onDoneCallback != null)
            {
                onDoneCallback(); // we're done as far as the GUIManager is concerned
            }

            float widthOffset = _rectTransform.rect.width;
            _rectTransform.localPosition = Vector3.zero;

            BroadcastMessage(_WillHideScreenEvent, SendMessageOptions.DontRequireReceiver);

            // tween the panel to the desired position
            transform.DOLocalMoveX(-widthOffset, duration).OnComplete(HideScreenDone);
        }

        public virtual void HideScreenDone()
        {

            BroadcastMessage(_HideScreenDoneEvent, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false);
        }

        public virtual void HideScreenImmediate()
        {
            gameObject.SetActive(false);
            HideScreenDone();
        }
    }
}