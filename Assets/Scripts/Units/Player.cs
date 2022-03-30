using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Player : Unit, IInteractable
    {

        public Player(GameObject view) : base(view)
        {
            _view.SetActive(false);//wait for other cards
        }

        public void MoveTo(Vector2Int position, IInteractable unit)
        {
            _position = position;

            Vector3 pos = _view.GetComponent<SpriteRenderer>().bounds.size;

            _view.transform.DOLocalMove(new Vector3(pos.x * position.x, pos.y * position.y), 0.2f)
                .SetEase(Ease.OutQuad).OnComplete(() => MoveComplete(unit));
        }

        private void MoveComplete(IInteractable unit)
        {
            unit.Interact(this);
            Interact(unit);//player may die here
        }

        public event Action OnDied;
        public void Interact(IInteractable unit)
        {
            _health += (unit as Unit).Health;
            _ui.GetComponent<TextMeshPro>().text = _health.ToString();

            if (_health <= 0)
                OnDied?.Invoke();
        }

        public override void Deploy(Vector2Int position, Transform parent)
        {
            base.Deploy(position, parent);
            _ui.GetComponent<TextMeshPro>().text = _health.ToString();
        }

    }
}
