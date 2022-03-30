using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Config")]
    public sealed class Config : ScriptableObject
    {
        [SerializeField]
        private uint _fieldWidth;
        [SerializeField]
        private uint _fieldHeight;
        [SerializeField]
        private int _playerHealth;

        //[Range(0.1f, 5.0f), SerializeField]
        //private float _tweenDuration = 0.3f;

        public uint FieldWidth { get { return _fieldWidth; } }
        public uint FieldHeight { get { return _fieldHeight; } }
        public int PlayerHealth { get { return _playerHealth; } }
    }
}