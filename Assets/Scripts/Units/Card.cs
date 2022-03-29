using TMPro;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Card : Unit, IInteractable
    {
        public Card(GameObject view) : base(view)
        {

        }

        public void Interact(IInteractable unit)
        {
            _view.SetActive(false);
        }

        public void AddTrait(ITrait trait)
        {
            //Debug.Log(trait.Trait);
            _health *= trait.Trait;
            _view.GetComponent<SpriteRenderer>().sprite = ResData.sprites[trait.GetType()];
        }

        public override void Deploy(Vector2Int position, Transform parent)
        {
            base.Deploy(position,parent);
            _view.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = (_health < 0) ? "" + _health : "+" + _health;
        }
    }
}
