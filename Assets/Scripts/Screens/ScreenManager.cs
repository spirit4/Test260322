using System;
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
            OnClick?.Invoke();
            SceneManager.LoadScene(scene);
        }

        public void Quit()
        {
            OnClick?.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public event Action OnClick;
    }
}