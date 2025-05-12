using System;
using System.Collections;
using System.Linq;
using Managers;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class MoleEnemy : MonoBehaviour, IEnemy
    {
        private static readonly int MoleUp = Animator.StringToHash("MoleUp");
        private static readonly int MoleDown = Animator.StringToHash("MoleDown");
        private static readonly int MoleHide = Animator.StringToHash("MoleHide");
        [SerializeField] private float moveRadius = 2f;
        [SerializeField] private float followSpeed = 2f;
        [SerializeField] private float attackInterval = 4f;
        [SerializeField] private float visibleDuration = 1f;
        [SerializeField] private LayerMask obstacleLayer;

        //[Header("Visual References")]
        //[SerializeField] private GameObject moundVisual; 
        //[SerializeField] private SpriteRenderer moleSprite;
        private Collider2D moleCollider;

        private float _attackTimer = 0f;
        private bool _isAttacking = false;
        private GameObject _player;
        private float _currentAttackInterval;
        public float pauseDuration = 0.3f;
        private Animator _animator;
        private bool _isMoleOut = false;
        private bool _waitingForContinue = false;
        private string _lastFinishedAnimation;
        private Collider2D _childTriggerCollider;

        private void Awake()
        {
            _childTriggerCollider = GetComponentsInChildren<Collider2D>().FirstOrDefault(c => c.isTrigger);
            moleCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _attackTimer = 0f;
            _isAttacking = false;
            _waitingForContinue = false;
            _lastFinishedAnimation = null;
            _childTriggerCollider.enabled = true;
            moleCollider.enabled = false;
            attackInterval = Random.Range(3f, 4.5f);
            _isMoleOut = false;
            if (_animator != null)
            {
                _animator.Rebind(); 
                _animator.Update(0f); 
            }
            UpdateSortingOrder();
            HideMole(); 
        }

        private void Start()
        {
            _player = GameManager.Instance.playerObject;
            _animator = GetComponentInChildren<Animator>();
           
            /*if (moundVisual != null)
                moundVisual.SetActive(false);*/

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

                _childTriggerCollider.enabled = false;
                bool blocked = Physics2D.OverlapCircle(possiblePos, 0.3f, obstacleLayer);
                _childTriggerCollider.enabled = true;
                
                if (!blocked)
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

            if (this == null || gameObject == null) yield break;
            _animator.SetTrigger(MoleUp);
            
            yield return new WaitUntil(() => _waitingForContinue);
            _waitingForContinue = false;
            
            yield return new WaitForSeconds(pauseDuration);
            
            if (this == null || gameObject == null) yield break;
            SoundManager.Instance.PlayMolePop(); //sound effect mole pop
            
            ShowMole();
            
            _animator.speed = 1f;
            yield return WaitForAnimationToEnd("MoleUp");

            yield return new WaitForSeconds(visibleDuration);

            _animator.SetTrigger(MoleDown);
            yield return new WaitUntil(() => _waitingForContinue);
            _waitingForContinue = false;
            HideMole();
            yield return new WaitForSeconds(pauseDuration);
            _animator.speed = 1f;
            yield return WaitForAnimationToEnd("MoleDown");

            _animator.SetTrigger(MoleHide);

            _isAttacking = false;
        }


        private void HideMole()
        {
            if (moleCollider != null)
                moleCollider.enabled = false;
        }

        private void ShowMole()
        {
            if (moleCollider != null)
                moleCollider.enabled = true;
            
            UpdateSortingOrder();
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
        
        public IEnumerator PauseAnimation()
        {
            _animator.speed = 0f;
            _waitingForContinue = true;
            yield break; 
        }
        
        private IEnumerator WaitForAnimationToEnd(string stateName)
        {
            _lastFinishedAnimation = null;

            yield return new WaitUntil(() => _lastFinishedAnimation == stateName);
        }
        
        public void NotifyAnimationEnded(string animationName)
        {
            _lastFinishedAnimation = animationName;
        }
        
        private void UpdateSortingOrder()
        {
            var sortingGroup = GetComponentInChildren<SortingGroup>();
            if (sortingGroup != null)
            {
                sortingGroup.sortingOrder = Mathf.RoundToInt(- transform.position.y * 1000f + Random.Range(-5f, 5f));

            }
        }

    }
}
