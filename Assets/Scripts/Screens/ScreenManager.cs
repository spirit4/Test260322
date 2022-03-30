using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Screens
{
    /** <summary>Canvas Script</summary> */
    public class ScreenManager : MonoBehaviour
    {
        /** <summary>Scene name in Editor</summary> */
        public void GoToScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                 Application.Quit();
            #endif
        }
    }
}