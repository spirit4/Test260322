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
            _cards = new Stack<Card>((int)(config.FieldWidth * config.FieldHeight));
        }

        public Card GetCard(GameObject prefab, int level)
        {
            Card card;
            if (_cards.Count > 0)
                card = _cards.Pop();
            else
                card = new Card(GameObject.Instantiate<GameObject>(prefab));

            int health = Random.Range(1, level + 2);//max inclusive

            card.Init(health);
            card.AddTrait(GetTrait(level)); //red or green

            return card;
        }

        private ITrait GetTrait(int level)
        {
            float p1 = Random.Range(0, 1f);
            float p2 = level * 0.1f < 0.5f ? level * 0.1f : 0.5f;
            //Debug.Log($"if true {p1} > {p2} then it's a green card");

            if (p1 > p2)
                return new Greenness();

            return new Redness();
        }

        public void TakeItBack(Card card)
        {
            _cards.Push(card);
        }

        public void Clear()
        {
            foreach (var card in _cards)
            {
                card.Destroy();
            }
            _cards.Clear();
            _cards = null;
        }
    }
}
