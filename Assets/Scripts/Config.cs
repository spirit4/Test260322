using UnityEngine;

namespace Assets.Scripts
{
    //[CreateAssetMenu(menuName = "Config")]
    public sealed class Config : ScriptableObject
    {
        [SerializeField]
        private uint _fieldWidth;//TODO box colider
        [SerializeField]
        private uint _fieldHeight;
        [SerializeField]
        private int _playerHealth;
        public uint FieldWidth { get { return _fieldWidth; } }
        public uint FieldHeight { get { return _fieldHeight; } }
        public int PlayerHealth { get { return _playerHealth; } }
    }
}