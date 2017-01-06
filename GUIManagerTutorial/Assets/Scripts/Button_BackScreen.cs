using UnityEngine;
using UnityEngine.UI;

namespace The_A_Drain.GUI
{

    [RequireComponent(typeof(Button))]
    public class Button_BackScreen : MonoBehaviour
    {

        private Button _button;

        void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            GUIManager.GoBack();
        }
    }
}
