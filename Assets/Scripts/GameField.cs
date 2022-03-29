using Assets.Scripts.Units;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameField : MonoBehaviour
    {
        [SerializeField]
        private Config _config;
        [SerializeField]
        private TextMeshProUGUI _levelTMP;
        [SerializeField]
        private TextMeshProUGUI _movesTMP;

        private Creator _creator;
        private Card[,] _field;

        private Player _player;
        private int _level = 1;
        private int _moves = 0;

        void Awake()
        {
            _creator = new Creator(_config);
            ResData.Init();
        }
        void Start()
        {
            Debug.Log($"Field size {_config.FieldWidth} x {_config.FieldHeight}");
            Init();
        }

        private void Init()
        {
            //sole prefab on GameField in Editor
            _player = new Player(this.gameObject.transform.GetChild(0).gameObject);
            _player.Init(_config.PlayerHealth);
            int x = Random.Range(0, (int)_config.FieldWidth);
            int y = Random.Range(0, (int)_config.FieldHeight);
            _player.Deploy(new Vector2Int(x, y), this.gameObject.transform);

            //other cards
            _field = new Card[_config.FieldWidth, _config.FieldHeight];
            Vector2Int position;
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    position = new Vector2Int(i, j);
                    AddCard(position);

                    //extra card
                    // while one is disappearing, another will be shown
                    if (_player.Position == position)
                    {
                        _creator.TakeItBack(_field[i, j]);
                        _field[i, j] = null;
                        continue;
                    }
                }
            }
        }

        private void AddCard(Vector2Int position)
        {
            int health = Random.Range(1, _level + 2);//max inclusive
            Debug.Log($"health Card {health}");
            Card card = _field[position.x, position.y] = _creator.GetCard(_player.View);
            card.Init(health);
            card.AddTrait(GetTrait()); //red or green
            card.Deploy(position, this.gameObject.transform);
        }

        //extract logic
        private ITrait GetTrait()
        {
            float p1 = Random.Range(0, 1f);
            float p2 = _level * 0.1f < 0.5f ? _level * 0.1f : 0.5f;
            Debug.Log($"if true {p1} > {p2} then green card");
            if (p1 > p2)
                return new Greenness();

            return new Redness();
        }

        private void OnMouseDown()
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position * 1.5f;
            Vector2Int vector = new Vector2Int((int)(point.x / 4.67f), (int)(point.y / 6.7f)); //TODO make consts at least
            //Debug.Log($"x y {point}");

            //extract
            if (CanMove(_player.Position, vector))
            {
                AddCard(_player.Position);

                _player.MoveTo(vector);
                _player.Interact(_field[vector.x, vector.y]);
                _field[vector.x, vector.y].Interact(_player);

                _creator.TakeItBack(_field[vector.x, vector.y]);
                _field[vector.x, vector.y] = null;

                _moves++;
                _movesTMP.GetComponent<TextMeshProUGUI>().text = "Moves: " + _moves;

                _level = _moves / 10 + 1;
                _levelTMP.GetComponent<TextMeshProUGUI>().text = "Level: " + _level; //TODO update 1/10
            }
        }

        private bool CanMove(Vector2Int point1, Vector2Int point2)
        {
            if (Vector2Int.Distance(point1, point2) == 1)
                return true;

            return false;
        }

        void Update()
        {

        }

        void OnDestroy()
        {
            _creator.Clear();
            _creator = null;
        }
    }
}
