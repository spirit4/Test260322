using Assets.Scripts.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Creator
    {
        private Stack<Card> _cards;

        public Creator(Config config)
        {
            _cards = new Stack<Card>((int)(config.FieldWidth * config.FieldHeight));//TODO 2 cards in pool

        }

        public Card GetCard(GameObject prefab)
        {
            Card card;
            if (_cards.Count > 0)
                card = _cards.Pop();
            else
                card = new Card(GameObject.Instantiate<GameObject>(prefab));

            return card;
        }

        public void TakeItBack(Card card)
        {
            card.View.SetActive(false);
            _cards.Push(card);
        }

        public void Clear()
        {
            foreach (var card in _cards)
            {
                UnityEngine.Object.Destroy(card.View);
            }
            _cards.Clear();
            _cards = null;
        }
    }
}
