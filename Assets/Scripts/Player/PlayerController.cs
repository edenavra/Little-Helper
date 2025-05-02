using Managers;
using UnityEngine;

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
        
        private GameObject  _activeModel;
        private Animator    _activeAnim;
        
        private Vector2 _movement;
        //private string currentDirection = "";

        private void Awake()
        {
            GameManager.Instance.playerObject = gameObject;
            //_rb        = GetComponentInParent<Rigidbody2D>();
            _rb = GetComponent<Rigidbody2D>();
            _animFront = foxFront.GetComponent<Animator>();
            _animSide  = foxSide .GetComponent<Animator>();
            _animBack  = foxBack .GetComponent<Animator>();

            ActivateModel(foxFront, _animFront);
        }

        private void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // pick which model to show
            if (_movement.magnitude < 0.1f)
            {
                // no movement, keep whatever was active (or default to front)
            }
            else if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
            {
                // horizontal → side
                ActivateModel(foxSide, _animSide);
                // flip left/right
                foxSide.transform.localScale = new Vector3(
                    _movement.x < 0 ? -0.4f : 0.4f,
                    0.4f, 0.4f);
            }
            else
            {
                // vertical → up or down
                if (_movement.y > 0)
                    ActivateModel(foxBack, _animBack);
                else
                    ActivateModel(foxFront, _animFront);
            }

            var speed = _movement.magnitude;
            _activeAnim.speed = speed > 0.01f ? 1 : 0;
        }


        private void FixedUpdate()
        {
            _rb.linearVelocity = _movement.normalized * stats.moveSpeed;
            Debug.Log($"[PlayerController] MoveSpeed: {stats.moveSpeed}, Velocity: {_rb.linearVelocity}");
        }
        
        
        private void ActivateModel(GameObject model, Animator anim)
        {
            if (_activeModel == model) return;

            // turn off old
            if (_activeModel != null) _activeModel.SetActive(false);

            // turn on new
            model.SetActive(true);
            _activeModel = model;
            _activeAnim  = anim;
        }
        
        public void IncreaseMoveSpeed(float amount)
        {
            stats.moveSpeed += amount;
            Debug.Log($"Move Speed increased by {amount}. New speed: {stats.moveSpeed}");
        }


        // private void HandleDirection()
        // {
        //     if (movement.x > 0 && currentDirection != "Right")
        //     {
        //         animator.SetTrigger("GoRight");
        //         currentDirection = "Right";
        //     }
        //     else if (movement.x < 0 && currentDirection != "Left")
        //     {
        //         animator.SetTrigger("GoLeft");
        //         currentDirection = "Left";
        //     }
        //     else if (movement.y > 0 && currentDirection != "Up")
        //     {
        //         animator.SetTrigger("GoUp");
        //         currentDirection = "Up";
        //     }
        //     else if (movement.y < 0 && currentDirection != "Down")
        //     {
        //         animator.SetTrigger("GoDown");
        //         currentDirection = "Down";
        //     }
        //     else if (movement == Vector2.zero)
        //     {
        //         currentDirection = "";
        //     }
        // }
    }
}