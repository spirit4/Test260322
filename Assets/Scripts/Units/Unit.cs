using TMPro;
using UnityEngine;


namespace Assets.Scripts.Units
{
    public abstract class Unit
    {

        protected GameObject _view;
        protected Vector2Int _position;// position in array
        protected int _health;


        public Unit(GameObject view)
        {
            _view = view;
        }
        public virtual void Deploy(Vector2Int position, Transform parent)
        {
            //Debug.Log($"Deploy position {position.x}, {position.y}");
            _position = position;

            Vector3 pos = _view.GetComponent<SpriteRenderer>().bounds.size;
            _view.transform.SetParent(parent);
            _view.transform.localPosition = new Vector3(pos.x * position.x, pos.y * position.y);

            _view.SetActive(true);
        }

        public virtual void Init(int health)
        {
            _health = health;
        }


        public GameObject View
        {
            get
            {
                return _view;
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }

        }
        public Vector2Int Position
        {
            get
            {
                return _position;
            }
        }
    }
}