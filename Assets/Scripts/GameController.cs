using Assets.Scripts.Screens;
using Assets.Scripts.Units;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Config _config;
        [SerializeField]
        private TextMeshProUGUI _levelTMP;
        [SerializeField]
        private TextMeshProUGUI _movesTMP;
        [SerializeField]
        private GameObject _back;
        [SerializeField]
        private Canvas _canvas;

        private Creator _creator;
        private InputController _input;
        private Unit[,] _field;
        private Player _player;

        private int _level = 1;
        private int _moves = 0;

        void Awake()
        {
            DOTween.Init();
            ResData.Init();

            Init();
        }
        void Start()
        {
            //from 2x2 to 5x3 looks okay
            Debug.Log($"Field size {_config.FieldWidth} x {_config.FieldHeight}");
        }

        private void Init()
        {
            _canvas.GetComponent<ScreenManager>().OnClick += Destroy;

            _creator = new Creator(_config);

            //sole prefab on GameField in Editor
            _player = new Player(this.transform.GetChild(0).gameObject);

            _input = new InputController(_player, this.gameObject, _config);
            _input.OnMove += MakeAMove;

            Vector3 cardSize = _player.View.GetComponent<SpriteRenderer>().bounds.size;

            //click collider
            this.GetComponent<BoxCollider2D>().offset = new Vector2(cardSize.x * (_config.FieldWidth - 1) / 2, cardSize.y * (_config.FieldHeight - 1) / 2);
            this.GetComponent<BoxCollider2D>().size = new Vector2(cardSize.x * _config.FieldWidth, cardSize.y * _config.FieldHeight);

            //background
            _back.GetComponent<SpriteRenderer>().size = new Vector2(cardSize.x * _config.FieldWidth, cardSize.y * _config.FieldHeight);

            //game field
            this.transform.localPosition = new Vector3(-cardSize.x * (_config.FieldWidth - 1) / 2, -cardSize.y * (_config.FieldHeight - 1) / 2);
            _field = new Unit[_config.FieldWidth, _config.FieldHeight];

            UpdateUI();
            CreateField();
        }

        private void CreateField()
        {
            _player.Init(_config.PlayerHealth);
            int x = Random.Range(0, (int)_config.FieldWidth);
            int y = Random.Range(0, (int)_config.FieldHeight);
            _field[x, y] = _player;
            _player.OnDied += GameOver;

            Vector2Int position;
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    position = new Vector2Int(i, j);
                    Card card = _creator.GetCard(_player.View, _level);

                    if (x == i && y == j)//extra card
                    {
                        _creator.TakeItBack(card);
                        card.View.SetActive(false);
                    }
                    else
                        _field[position.x, position.y] = card;

                    StartCoroutine(SpawnUnit(_field[i, j], position, (_field.GetLength(1) - j) * 0.2f));
                }
            }
        }

        private IEnumerator SpawnUnit(Unit unit, Vector2Int position, float time)
        {
            yield return new WaitForSeconds(time);
            unit.Deploy(position, this.transform);
        }

        private void Update()
        {
            if (_player != null) //gameover
                _input.UpdateHandler();
        }

        private void OnMouseDown()
        {
            _input.MouseDownHandler();
        }
        private void MakeAMove(Vector2Int direction)
        {
            DOTween.KillAll(true);//previous moves
            
            Card card = _creator.GetCard(_player.View, _level);
            _field[_player.Position.x, _player.Position.y] = card;
            card.Deploy(_player.Position, this.transform);

            card = _field[direction.x, direction.y] as Card;
            _field[direction.x, direction.y] = _player;

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

        private void GameOver()
        {
            _player.OnDied -= GameOver;

            Saver.Save(_level, _moves);
            _canvas.GetComponent<ScreenManager>().GoToScene("GameOver");
        }

        private void Destroy()
        {
            Debug.Log("Game Destroy");
            _canvas.GetComponent<ScreenManager>().OnClick -= Destroy;
            DOTween.Clear();

            _player.OnDied -= GameOver;
            _player = null;

            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    if (_field[i, j] != null)//player's empty cell
                        _field[i, j].Destroy();
                }
            }
            _field = null;

            _input.Destroy();
            _input.OnMove -= MakeAMove;
            _input = null;

            _creator.Clear();
            _creator = null;
        }
    }
}
