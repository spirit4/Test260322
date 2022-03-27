using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    /** <summary>Canvas Script</summary> */
    public class ScreenManager : MonoBehaviour
    {
        //TODO It's possible to get scene.name from UnityEditor.SceneAsset
        //and attach SceneAsset instead of writing string name
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