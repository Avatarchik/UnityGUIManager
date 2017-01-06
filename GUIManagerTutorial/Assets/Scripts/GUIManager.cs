using UnityEngine;
using System.Collections.Generic;

namespace The_A_Drain.GUI
{

    public class GUIManager
    {

        public enum Screens
        {
            NONE,
            MAINMENU,
            SETTINGS,
            STORE,
            GAME
        }

        private static Dictionary<Screens, string> _requiredScreens = new Dictionary<Screens, string>() {
        // format: (Screens) screen type, (string) prefab location
        {Screens.MAINMENU, "screens/mainmenu_screen"},
        {Screens.SETTINGS, "screens/settings_screen"},
        {Screens.STORE, "screens/store_screen"},
        {Screens.GAME, "screens/game_screen"}
    };

        private static Dictionary<Screens, ScreenPanel> _availableScreens = new Dictionary<Screens, ScreenPanel>();
        private static bool initialised = false;

        private static Stack<Screens> _uiStack;
        private static GameObject _canvasRootObject;
        private static Screens _currentScreen = Screens.NONE;
        private static float _defaultDuration = 0.5f;

        public static void InitialiseGUI()
        {
            if (initialised == true)
            {
                Debug.LogError("Error: Cannot initialize GUI again, it is already setup.");
                return;
            }

            _canvasRootObject = GameObject.Instantiate(Resources.Load("Canvas") as GameObject);
            _canvasRootObject.name = "ui_root_canvas";

            if (_canvasRootObject == null)
            {
                Debug.LogError("Error: Could not find 'uicanvas' root");
                return;
            }

            foreach (KeyValuePair<Screens, string> entry in _requiredScreens)
            {
                CreateAndCatalogueScreen(entry.Key, entry.Value);
            }

            initialised = true;
        }

        private static void CreateAndCatalogueScreen(Screens screenType, string prefabLocation)
        {
            GameObject screenPrefab = Resources.Load<GameObject>(prefabLocation) as GameObject;
            GameObject instantiatedScreen = GameObject.Instantiate(screenPrefab);
            instantiatedScreen.transform.SetParent(_canvasRootObject.transform, false);
            ScreenPanel screenPanel = instantiatedScreen.GetComponent<ScreenPanel>();
            _availableScreens.Add(screenType, screenPanel);
        }

        public static bool CanGoBack()
        {
            return (_uiStack != null && _uiStack.Count > 1);
        }

        public static void OpenPage(Screens screen, bool clearHistory = false)
        {
            if (screen == _currentScreen)
            {
                Debug.LogWarning("Attempted to open the same screen, aborting.");
                return;
            }

            if (_uiStack == null)
            {
                _uiStack = new Stack<Screens>();
            }

            ScreenPanel panel = null;
            _availableScreens.TryGetValue(screen, out panel);

            if (panel != null)
            {
                if (_uiStack.Count > 0)
                {
                    // we may need to delay showing the new screen until the old one calls back
                    _availableScreens[_uiStack.Peek()].HideScreen(_defaultDuration, () =>
                    {

                        _availableScreens[screen].ShowScreen();

                        if (clearHistory)
                        {
                            _uiStack.Clear();
                        }

                        _uiStack.Push(screen);
                        _currentScreen = screen;
                    });
                }
                else // we don't need to wait
                {
                    _availableScreens[screen].ShowScreen();

                    if (clearHistory)
                    {
                        _uiStack.Clear();
                    }

                    _uiStack.Push(screen);
                    _currentScreen = screen;
                }
            }
            else
            {
                Debug.LogError("'" + screen.ToString() + "' is not a valid screen.");
            }
        }

        public static void GoBack()
        {
            if (!CanGoBack())
            {
                Debug.LogError(" _uiStack not initialized or empty, cannot go back.");
                return;
            }

            _availableScreens[_uiStack.Pop()].HideScreen(_defaultDuration, () =>
            {
                _availableScreens[_uiStack.Peek()].ShowScreen();
                _currentScreen = _uiStack.Peek();
            });
        }

        public static void PopToRoot()
        {
            if (!CanGoBack())
            {
                Debug.LogError(" _uiStack not initialized or empty, cannot go to root.");
                return;
            }

            _availableScreens[_uiStack.Pop()].HideScreen(_defaultDuration, () =>
            {

                while (_uiStack.Count > 1)
                {
                    _uiStack.Pop();
                }

                _availableScreens[_uiStack.Peek()].ShowScreen();
                _currentScreen = _uiStack.Peek();
            });
        }
    }
}
