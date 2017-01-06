using UnityEngine;
using DG.Tweening;

namespace The_A_Drain.GUI
{

    public class GameGUIExample : MonoBehaviour
    {

        public RectTransform _topPanel;
        public RectTransform _bottomPanel;

        private Vector3 _startPos_TopPanel;
        private Vector3 _startPos_BottomPanel;

        private Vector3 _destPos_TopPanel;
        private Vector3 _destPos_BottomPanel;

        void Awake()
        {
            // get all our initial positions 
            _startPos_TopPanel = _topPanel.localPosition + Vector3.up * 140f;
            _startPos_BottomPanel = _bottomPanel.localPosition + Vector3.right * 320f;

            // get our destination positions from the current positions
            _destPos_TopPanel = _topPanel.localPosition;
            _destPos_BottomPanel = _bottomPanel.localPosition;
        }

        public void OnWillShowScreen()
        {
            Debug.Log("HIDING GAME GUI ELEMENTS");
            // set our gui objects to their off-screen starting positions
            _topPanel.localPosition = _startPos_TopPanel;
            _bottomPanel.localPosition = _startPos_BottomPanel;
        }

        public void OnShowScreenDone()
        {
            Sequence seq = DOTween.Sequence();

            // move the top panel in from above   
            seq.Append(_topPanel.DOLocalMove(_destPos_TopPanel, 0.5f));

            // move the bottom panel in from the right-hand side
            seq.Append(_bottomPanel.DOLocalMove(_destPos_BottomPanel, 0.2f));
        }

        public void OnWillHideScreen(float duration)
        {
            Sequence seq = DOTween.Sequence();

            // move the top panel in from above   
            seq.Join(_topPanel.DOLocalMove(_startPos_TopPanel, 0.5f));

            // move the bottom panel in from the right-hand side
            seq.Join(_bottomPanel.DOLocalMove(_startPos_BottomPanel, 0.3f));
        }
    }
}