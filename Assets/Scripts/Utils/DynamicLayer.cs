using System;
using Managers;
using UnityEngine;

namespace Utils
{
    public class DynamicLayer : MonoBehaviour
    {
        private SpriteRenderer _sprite; 
        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = GameManager.Instance.playerObject.transform;
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_playerTransform.position.y > transform.position.y)
            {
                if (_sprite.sortingOrder != 100)
                {
                    _sprite.sortingOrder = 100;
                }
            }
            else if (_sprite.sortingOrder != 0)
            {
                _sprite.sortingOrder = 0;
            }
        }
    }
}