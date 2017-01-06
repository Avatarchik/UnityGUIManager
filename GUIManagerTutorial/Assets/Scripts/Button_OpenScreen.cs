using UnityEngine;
using UnityEngine.UI;

namespace The_A_Drain.GUI
{

    [RequireComponent(typeof(Button))]
    public class Button_OpenScreen : MonoBehaviour
    {

        private Button _button;
        public GUIManager.Screens _target;

        void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            GUIManager.OpenPage(_target);
        }
    }
}
