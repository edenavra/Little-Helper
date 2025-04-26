using System;
using System.Collections;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class MoleEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private float moveRadius = 2f;
        [SerializeField] private float followSpeed = 2f;
        [SerializeField] private float attackInterval = 5f;
        [SerializeField] private float visibleDuration = 2f;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("Visual References")]
        [SerializeField] private GameObject moundVisual; 
        [SerializeField] private SpriteRenderer moleSprite;
        [SerializeField] private Collider2D moleCollider;

        private float _attackTimer = 0f;
        private bool _isAttacking = false;
        private GameObject _player;
        private float _currentAttackInterval;

        private void Start()
        {
            _player = GameManager.Instance.playerObject;
            if (moundVisual != null)
                moundVisual.SetActive(false);

            HideMole();
        }

        private void Update()
        {
            if (_player == null) return;

            if (!_isAttacking)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _currentAttackInterval)
                {
                    StartCoroutine(AttackRoutine());
                    _attackTimer = 0f;
                }
                else
                {
                    FollowPlayer();
                }
            }
        }
        
        private void FollowPlayer()
        {
            Vector2 targetPos = transform.position;

            for (int i = 0; i < 10; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
                Vector2 possiblePos = (Vector2)_player.transform.position + randomOffset;

                if (!Physics2D.OverlapCircle(possiblePos, 0.3f, obstacleLayer))
                {
                    targetPos = possiblePos;
                    break;
                }
            }

            transform.position = Vector2.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }

        private IEnumerator AttackRoutine()
        {
            _isAttacking = true;

            //TODO: ADD ANIMATION INSTEAD OF ENABLE MOUND
            if (moundVisual != null)
                moundVisual.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            ShowMole();

            yield return new WaitForSeconds(visibleDuration);

            HideMole();

            yield return new WaitForSeconds(0.5f); 

            //TODO: ADD ANIMATION INSTEAD OF DISABLE MOUND
            if (moundVisual != null)
                moundVisual.SetActive(false);

            _isAttacking = false;
        }

        private void HideMole()
        {
            //TODO: ADD ANIMATION INSTEAD OF DISABLE SPRITE
            if (moleSprite != null)
                moleSprite.enabled = false;
            if (moleCollider != null)
                moleCollider.enabled = false;
        }

        private void ShowMole()
        {
            //TODO: ADD ANIMATION INSTEAD OF ENABLE SPRITE
            if (moleSprite != null)
                moleSprite.enabled = true;
            if (moleCollider != null)
                moleCollider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isAttacking && moleCollider.enabled && collision.gameObject == _player)
            {
                AttackPlayer(_player);
            }
        }
        
       /* private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
            {
                Vector3 collisionNormal = other.offset;
                Vector3 reflectDirection = Vector3.Reflect((player.transform.position - transform.position).normalized, collisionNormal);
                Vector3 bounceTarget = transform.position + reflectDirection * 0.5f;

                transform.position = Vector3.MoveTowards(transform.position, bounceTarget, followSpeed * Time.deltaTime);
            }
        }*/

        
        public void AttackPlayer(GameObject player)
        {
            var health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
        }

        public void OnRoundStarted(int level)
        {
            _currentAttackInterval = Mathf.Max(1f, attackInterval - level * 0.3f);
        }
    }
}
