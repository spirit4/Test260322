using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Card : Unit, IInteractable
    {
        private ITrait _trait;
        public Card(GameObject view) : base(view)
        {

        }

        public void Interact(IInteractable unit)
        {
            _view.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InQuad).OnComplete(() => _view.SetActive(false));//TODO add tween time from Config

            _ui.transform.DOLocalMove(Vector3.up * 3.5f, 0.2f)
                .SetEase(Ease.OutQuad);
            _ui.transform.DOScale(Vector3.one *0.6f, 0.2f)
                .SetEase(Ease.OutQuad);
        }

        public void AddTrait(ITrait trait)
        {
            //Debug.Log(trait.Trait);
            _trait = trait;
            _health *= trait.SignInt;
            _view.GetComponent<SpriteRenderer>().sprite = ResData.sprites[trait.GetType()];
        }

        public override void Deploy(Vector2Int position, Transform parent)
        {
            _view.transform.localScale = Vector3.one;//after tween
            base.Deploy(position, parent);

            _view.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _view.transform.localScale = Vector3.zero;
            _view.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InQuad);

            _ui.transform.localPosition = Vector3.zero;
            _ui.transform.localScale = Vector3.one;
            _ui.GetComponent<TextMeshPro>().text = _trait.SignString + _health;
        }

        public override void Destroy()
        {
            _trait = null;
            base.Destroy();
        }
    }
}
