using Assets.Scripts.Units;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputController
    {
        private Player _player;
        private Config _config;
        private GameObject _field;
        public InputController(Player player, GameObject field, Config config)
        {
            _player = player;
            _field = field;
            _config = config;
        }

        public void UpdateHandler()
        {
            int x = _player.Position.x;
            int y = _player.Position.y;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                y = _player.Position.y + 1;

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                y = _player.Position.y - 1;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                x = _player.Position.x - 1;

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                x = _player.Position.x + 1;

            if (_player.Position.x != x || _player.Position.y != y)
                MoveIfCorrect(x, y);
        }

        private void MoveIfCorrect(int x, int y)
        {
            x = Mathf.Clamp(x, 0, (int)_config.FieldWidth - 1);
            y = Mathf.Clamp(y, 0, (int)_config.FieldHeight - 1);
            var direction = new Vector2Int(x, y);

            if (_player.Position != direction)
                OnMove?.Invoke(direction);
        }

        public event Action<Vector2Int> OnMove;
        public void MouseDownHandler()
        {
            Vector3 cardSize = _player.View.GetComponent<SpriteRenderer>().bounds.size;
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _field.transform.localPosition + cardSize / 2;
            Vector2Int direction = new Vector2Int((int)(point.x / cardSize.x), (int)(point.y / cardSize.y));
            //Debug.Log($"x y {point}");

            if (CanMove(_player.Position, direction))
                OnMove?.Invoke(direction);
        }

        private bool CanMove(Vector2Int point1, Vector2Int point2)
        {
            if (Vector2Int.Distance(point1, point2) == 1)
                return true;

            return false;
        }

        public void Destroy()
        {
            _player = null;
            _field = null;
            _config = null;
        }
    }
}
