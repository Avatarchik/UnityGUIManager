using UnityEngine;
using DG.Tweening;
using System;

namespace The_A_Drain.GUI
{

    public class GameScreenPanel : ScreenPanel
    {

        public override void ShowScreen(float delay = 0f, float duration = 0.5f)
        {
            // override the show screen animation with one unique to this screen
            Vector3 startPosition = Vector3.zero;
            _rectTransform.localPosition = startPosition;

            // just use the tween to set a callback for done
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delay + duration);
            seq.AppendCallback(ShowScreenDone);

            // Inform any screen child objects that we are about to show the screen (screen NOT visible at this point!)
            gameObject.SetActive(true); // we need to enable the gameobject in order for it to recieve the event
            BroadcastMessage(_WillShowScreenEvent, SendMessageOptions.DontRequireReceiver);
            gameObject.SetActive(false); // we can safely disable it aftr
        }

        protected override void ShowScreenDone()
        {
            // show the screen now, as we wanted to delay it
            gameObject.SetActive(true);

            // Inform any screen child objects that the screen is now shown and visible
            BroadcastMessage(_ShowScreenDoneEvent, SendMessageOptions.DontRequireReceiver);
        }

        public override void HideScreen(float duration = 0.5f, Action onDoneCallback = null)
        {
            // Let our UI know how much time it has to get out of the way!
            BroadcastMessage(_WillHideScreenEvent, duration, SendMessageOptions.DontRequireReceiver);

            // Schedue our callback for the durations end
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(duration);
            seq.AppendCallback(HideScreenDone);

            // finally append the callback to let the GUIManager know it's safe to continue
            if (onDoneCallback != null)
            {
                seq.AppendCallback(() =>
                {
                    onDoneCallback();
                });
            }
        }
    }
}