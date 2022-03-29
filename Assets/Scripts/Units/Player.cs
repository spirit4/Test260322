using TMPro;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Player : Unit, IInteractable
    {
        private bool _isMoving = false;

        public Player(GameObject view) : base(view)
        {

        }

        public void MoveTo(Vector2Int position)
        {
            if (!_isMoving)//TODO tween check, do tween
            {
                _position = position;
                Vector3 pos = _view.GetComponent<SpriteRenderer>().bounds.size;
                _view.transform.localPosition = new Vector3(pos.x * position.x, pos.y * position.y);
            }
        }

        public void Interact(IInteractable unit)
        {
            _health += (unit as Unit).Health;
            _view.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _health.ToString();
        }

        public override void Deploy(Vector2Int position, Transform parent)
        {
            base.Deploy(position,parent);
            _view.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _health.ToString();
        }
    }
}
