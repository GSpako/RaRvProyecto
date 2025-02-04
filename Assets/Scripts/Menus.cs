using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class Menus : MonoBehaviour
{
    public void QuitApplication()
    {
        // If running in the Unity editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If running as a built application, quit the app
            Application.Quit();
#endif
    }

    public void Restart()
    {
        LoadScene(0);
    }

}
