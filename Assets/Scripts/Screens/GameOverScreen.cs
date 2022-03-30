using TMPro;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _levelTMP;
        [SerializeField]
        private TextMeshProUGUI _movesTMP;

        private void Start()
        {
            Saver.Load(out int level, out int score, out _);

            _levelTMP.text = "Level: " + level.ToString();
            _movesTMP.text = score.ToString();
        }

    }
}
