using Assets.Scripts.Units;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    //acting as MVC 3 in 1
    public class GameField : MonoBehaviour//TODO make Controller
    {
        [SerializeField]
        private Config _config;
        [SerializeField]
        private TextMeshProUGUI _levelTMP;
        [SerializeField]
        private TextMeshProUGUI _movesTMP;
        [SerializeField]
        private GameObject _back;

        private Creator _creator;
        private Card[,] _field;
        private Player _player;

        private int _level = 10;
        private int _moves = 0;

        void Awake()
        {
            _creator = new Creator(_config);
            ResData.Init();
        }
        void Start()
        {
            DOTween.Init();

            Debug.Log($"Field size {_config.FieldWidth} x {_config.FieldHeight}");
            Init();
        }

        private void Init()
        {
            _player = new Player(this.gameObject.transform.GetChild(0).gameObject);

            Vector3 cardSize = _player.View.GetComponent<SpriteRenderer>().bounds.size;
            this.GetComponent<BoxCollider2D>().offset = new Vector2(cardSize.x * (_config.FieldWidth - 1) / 2, cardSize.y * (_config.FieldHeight - 1) / 2);
            this.GetComponent<BoxCollider2D>().size = new Vector2(cardSize.x * _config.FieldWidth, cardSize.y * _config.FieldHeight);
            _back.GetComponent<SpriteRenderer>().size = new Vector2(cardSize.x * _config.FieldWidth, cardSize.y * _config.FieldHeight);
            this.transform.localPosition = new Vector3(-cardSize.x * (_config.FieldWidth - 1) / 2, -cardSize.y * (_config.FieldHeight - 1) / 2);


            UpdateUI();

            _field = new Card[_config.FieldWidth, _config.FieldHeight];

            //sole prefab on GameField in Editor

            _player.Init(_config.PlayerHealth);
            int x = Random.Range(0, (int)_config.FieldWidth);
            int y = Random.Range(0, (int)_config.FieldHeight);
            _player.Deploy(new Vector2Int(x, y), this.gameObject.transform);
            _player.OnDied += GameOver;

            //other cards
            Vector2Int position;
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    position = new Vector2Int(i, j);

                    AddCard(position, (_field.GetLength(0) - j) * 0.2f);
                    //extra card
                    if (_player.Position == position)
                    {
                        _creator.TakeItBack(_field[i, j]);
                        _field[i, j].View.SetActive(false);
                        _field[i, j] = null;
                        continue;
                    }

                    //StartCoroutine(AddCard(position, (_field.GetLength(0) - j) * 0.2f));
                    //StartCoroutine();
                }
            }
        }

        private void GameOver()
        {
            _player.OnDied -= GameOver;
            //Debug.Log("GameOver");

            Saver.Save(_level, _moves);
            SceneManager.LoadScene("GameOver");
            Destroy();
        }

        //private IEnumerator AddCard(Vector2Int position, float time) //TODO finish it
        private void AddCard(Vector2Int position, float time)
        {
            //yield return new WaitForSeconds(time );

            int health = Random.Range(1, _level + 2);//max inclusive
            //Debug.Log($"health Card {health}");
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
            //Debug.Log($"if true {p1} > {p2} then green card");
            if (p1 > p2)
                return new Greenness();

            return new Redness();
        }

        private void Update()
        {
            //TODO extract
            int x = _player.Position.x;
            int y = _player.Position.y;
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                y = _player.Position.y + 1;
                CheckKeyInput(x, y);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                y = _player.Position.y - 1;
                CheckKeyInput(x, y);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                x = _player.Position.x - 1;
                CheckKeyInput(x, y);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                x = _player.Position.x + 1;
                CheckKeyInput(x, y);
            }
        }
        private void CheckKeyInput(int x, int y)//TODO rename and delay
        {
            x = Mathf.Clamp(x, 0, (int)_config.FieldWidth - 1);
            y = Mathf.Clamp(y, 0, (int)_config.FieldHeight - 1);
            var direction = new Vector2Int(x, y);

            Debug.Log($"x y {direction}");
            if (_player.Position != direction)
                Move(direction);
        }

        private void OnMouseDown()
        {
            DOTween.KillAll(true);

            Vector3 cardSize = _player.View.GetComponent<SpriteRenderer>().bounds.size;
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.localPosition + cardSize / 2;
            Vector2Int direction = new Vector2Int((int)(point.x / cardSize.x), (int)(point.y / cardSize.y));
            //Debug.Log($"x y {point}");

            if (CanMove(_player.Position, direction))
                Move(direction);
        }
        private void Move(Vector2Int direction)
        {
            AddCard(_player.Position, 0f);
            //StartCoroutine(AddCard(_player.Position, 0f));

            Card card = _field[direction.x, direction.y];
            _field[direction.x, direction.y] = null;

            _player.MoveTo(direction, card);//player may die here
            _creator.TakeItBack(card);

            _moves++;
            _level = _moves / 10 + 1;

            UpdateUI();
        }

        private void UpdateUI()
        {
            _movesTMP.GetComponent<TextMeshProUGUI>().text = "Moves: " + _moves;
            _levelTMP.GetComponent<TextMeshProUGUI>().text = "Level: " + _level;
        }

        private bool CanMove(Vector2Int point1, Vector2Int point2)
        {
            if (Vector2Int.Distance(point1, point2) == 1)
                return true;

            return false;
        }

        //TODO add equivalent OnDestroy for gui exit
        private void Destroy()
        {
            DOTween.Clear();

            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    if (_field[i, j] != null)//player's empty cell
                        _field[i, j].Destroy();
                }
            }
            _field = null;

            _player.Destroy();
            _player = null;

            _creator.Clear();
            _creator = null;
        }
    }
}
