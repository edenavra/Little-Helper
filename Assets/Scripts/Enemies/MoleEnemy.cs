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

        [Header("Visual References")]
        [SerializeField] private GameObject moundVisual; 
        [SerializeField] private SpriteRenderer moleSprite;
        [SerializeField] private Collider2D moleCollider;

        private float attackTimer = 0f;
        private bool isAttacking = false;
        private GameObject player;

        private void Start()
        {
            player = GameManager.Instance.playerObject;
            if (moundVisual != null)
                moundVisual.SetActive(false);

            HideMole();
        }

        private void Update()
        {
            if (player == null) return;

            if (!isAttacking)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    StartCoroutine(AttackRoutine());
                    attackTimer = 0f;
                }
                else
                {
                    FollowPlayer();
                }
            }
        }

        private void FollowPlayer()
        {
            Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
            Vector2 targetPos = (Vector2)player.transform.position + randomOffset;
            transform.position = Vector2.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }

        private IEnumerator AttackRoutine()
        {
            isAttacking = true;

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

            isAttacking = false;
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
            if (isAttacking && moleCollider.enabled && collision.gameObject == player)
            {
                AttackPlayer(player);
            }
        }

        
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
            attackInterval = Mathf.Max(1f, attackInterval - level * 0.3f);
        }
    }
}
