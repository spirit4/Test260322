using TMPro;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _bestScore;

        private void Start()
        {
            Saver.Load(out _, out _, out int maxScore);

            _bestScore.text = maxScore.ToString();
        }
        
    }
}
