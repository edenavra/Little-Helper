using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStats stats;

        [SerializeField] private GameObject foxFront;
        [SerializeField] private GameObject foxSide;
        [SerializeField] private GameObject foxBack;

        private Rigidbody2D _rb;
        private Animator _animFront, _animSide, _animBack;

        private GameObject _activeModel;
        private Animator _activeAnim;
        private Vector2 _movement;

        // Hide upgrade variables
        private bool isHiding = false;
        private bool canHide = true;
        private float currentHideCooldown = 0f;
        private SpriteRenderer spriteRenderer;
        private PlayerHealth playerHealth;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animFront = foxFront.GetComponent<Animator>();
            _animSide = foxSide.GetComponent<Animator>();
            _animBack = foxBack.GetComponent<Animator>();

            ActivateModel(foxFront, _animFront);

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            playerHealth = GetComponentInChildren<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError("[PlayerController] PlayerHealth not found in children!");
            }
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            if (_movement.magnitude > 0.1f)
            {
                if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
                {
                    ActivateModel(foxSide, _animSide);
                    foxSide.transform.localScale = new Vector3(
                        _movement.x < 0 ? -0.4f : 0.4f,
                        0.4f, 0.4f);
                }
                else
                {
                    ActivateModel(_movement.y > 0 ? foxBack : foxFront,
                                  _movement.y > 0 ? _animBack : _animFront);
                }
            }

            _activeAnim.speed = _movement.magnitude > 0.01f ? 1 : 0;

            // HIDE input
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (canHide)
                {
                    Debug.Log($"[Time: {Time.time:F2}] Pressed H — starting HideRoutine (Duration: {stats.hideDuration} seconds)");
                    StartCoroutine(HideRoutine());
                }
                else
                {
                    Debug.Log($"[Time: {Time.time:F2}] Tried to hide but still in cooldown! Time left: {currentHideCooldown:F2}s");
                }
            }


            // Cooldown update
            if (!canHide)
            {
                currentHideCooldown -= Time.deltaTime;
                if (currentHideCooldown <= 0f)
                {
                    canHide = true;
                    Debug.Log($"[{Time.time:F2}] Hide is ready again.");
                }
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _movement.normalized * stats.moveSpeed;
        }

        private void ActivateModel(GameObject model, Animator anim)
        {
            if (_activeModel == model) return;

            if (_activeModel != null) _activeModel.SetActive(false);
            model.SetActive(true);
            _activeModel = model;
            _activeAnim = anim;
        }

        public void IncreaseMoveSpeed(float amount)
        {
            stats.moveSpeed += amount;
            Debug.Log($"Move Speed increased by {amount}. New speed: {stats.moveSpeed}");
        }

        private IEnumerator HideRoutine()
        {
            if (isHiding)
            {
                Debug.Log("Hide is already active — ignoring repeated activation.");
                yield break;
            }

            Debug.Log("Started HideRoutine");

            isHiding = true;

            Debug.Log($"[{Time.time:F2}] Hide activated for {stats.hideDuration} seconds!");

            // שקיפות
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0.4f;
                spriteRenderer.color = color;
            }

            // חוסר פגיעות
            if (playerHealth != null)
            {
                playerHealth.SetInvincible(true);
            }
            else
            {
                Debug.LogError("[PlayerController] PlayerHealth reference is null during Hide!");
            }

            yield return new WaitForSeconds(stats.hideDuration);

            // החזרת שקיפות
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }

            if (playerHealth != null)
            {
                playerHealth.SetInvincible(false);
            }

            Debug.Log("Hide effect has ended — player is visible and vulnerable again.");

            // רק עכשיו מתחיל הקול־דאון
            canHide = false;
            currentHideCooldown = stats.hideCooldown;
            isHiding = false;
        }
        
        
        
    }
}


// using UnityEngine;
//
// namespace Player
// {
//     public class PlayerController : MonoBehaviour
//     {
//         public PlayerStats stats;
//
//         [SerializeField] private GameObject foxFront;
//         [SerializeField] private GameObject foxSide;
//         [SerializeField] private GameObject foxBack;
//         
//         private Rigidbody2D _rb;
//         private Animator _animFront, _animSide, _animBack;
//         
//         private GameObject  _activeModel;
//         private Animator    _activeAnim;
//         
//         private Vector2 _movement;
//         //private string currentDirection = "";
//
//         private void Awake()
//         {
//             //_rb        = GetComponentInParent<Rigidbody2D>();
//             _rb = GetComponent<Rigidbody2D>();
//             _animFront = foxFront.GetComponent<Animator>();
//             _animSide  = foxSide .GetComponent<Animator>();
//             _animBack  = foxBack .GetComponent<Animator>();
//
//             ActivateModel(foxFront, _animFront);
//         }
//
//         private void Update()
//         {
//             _movement.x = Input.GetAxisRaw("Horizontal");
//             _movement.y = Input.GetAxisRaw("Vertical");
//
//             // pick which model to show
//             if (_movement.magnitude < 0.1f)
//             {
//                 // no movement, keep whatever was active (or default to front)
//             }
//             else if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
//             {
//                 // horizontal → side
//                 ActivateModel(foxSide, _animSide);
//                 // flip left/right
//                 foxSide.transform.localScale = new Vector3(
//                     _movement.x < 0 ? -0.4f : 0.4f,
//                     0.4f, 0.4f);
//             }
//             else
//             {
//                 // vertical → up or down
//                 if (_movement.y > 0)
//                     ActivateModel(foxBack, _animBack);
//                 else
//                     ActivateModel(foxFront, _animFront);
//             }
//
//             var speed = _movement.magnitude;
//             _activeAnim.speed = speed > 0.01f ? 1 : 0;
//         }
//
//
//         private void FixedUpdate()
//         {
//             _rb.linearVelocity = _movement.normalized * stats.moveSpeed;
//             Debug.Log($"[PlayerController] MoveSpeed: {stats.moveSpeed}, Velocity: {_rb.linearVelocity}");
//         }
//         
//         
//         private void ActivateModel(GameObject model, Animator anim)
//         {
//             if (_activeModel == model) return;
//
//             // turn off old
//             if (_activeModel != null) _activeModel.SetActive(false);
//
//             // turn on new
//             model.SetActive(true);
//             _activeModel = model;
//             _activeAnim  = anim;
//         }
//         
//         public void IncreaseMoveSpeed(float amount)
//         {
//             stats.moveSpeed += amount;
//             Debug.Log($"Move Speed increased by {amount}. New speed: {stats.moveSpeed}");
//         }
//
//
//         // private void HandleDirection()
//         // {
//         //     if (movement.x > 0 && currentDirection != "Right")
//         //     {
//         //         animator.SetTrigger("GoRight");
//         //         currentDirection = "Right";
//         //     }
//         //     else if (movement.x < 0 && currentDirection != "Left")
//         //     {
//         //         animator.SetTrigger("GoLeft");
//         //         currentDirection = "Left";
//         //     }
//         //     else if (movement.y > 0 && currentDirection != "Up")
//         //     {
//         //         animator.SetTrigger("GoUp");
//         //         currentDirection = "Up";
//         //     }
//         //     else if (movement.y < 0 && currentDirection != "Down")
//         //     {
//         //         animator.SetTrigger("GoDown");
//         //         currentDirection = "Down";
//         //     }
//         //     else if (movement == Vector2.zero)
//         //     {
//         //         currentDirection = "";
//         //     }
//         // }
//     }
// }