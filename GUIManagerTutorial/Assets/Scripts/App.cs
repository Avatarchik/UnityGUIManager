using UnityEngine;
using System.Collections;

namespace The_A_Drain.GUI
{

    public class App : MonoBehaviour
    {

        // Use this for initialization
        void Awake()
        {
            // Initialise the GUI Manager
            GUIManager.InitialiseGUI();

            // Show the first screen
            GUIManager.OpenPage(GUIManager.Screens.MAINMENU);
        }
    }
}
