using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStats stats;

        [SerializeField] private GameObject foxFront;
        [SerializeField] private GameObject foxSide;
        [SerializeField] private GameObject foxBack;
        
        private Rigidbody2D rb;
        private Animator animFront, animSide, animBack;
        
        private GameObject  activeModel;
        private Animator    activeAnim;
        
        private Vector2 movement;
        //private string currentDirection = "";

        void Awake()
        {
            rb        = GetComponent<Rigidbody2D>();
            animFront = foxFront.GetComponent<Animator>();
            animSide  = foxSide .GetComponent<Animator>();
            animBack  = foxBack .GetComponent<Animator>();

            // ensure only front is active at start
            ActivateModel(foxFront, animFront);
        }

        void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // pick which model to show
            if (movement.magnitude < 0.1f)
            {
                // no movement, keep whatever was active (or default to front)
            }
            else if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // horizontal → side
                ActivateModel(foxSide, animSide);
                // flip left/right
                foxSide.transform.localScale = new Vector3(
                    movement.x < 0 ? -1 : 1,
                    1, 1);
            }
            else
            {
                // vertical → up or down
                if (movement.y > 0)
                    ActivateModel(foxBack, animBack);
                else
                    ActivateModel(foxFront, animFront);
            }

            // drive the Blend Tree on the active Animator
            float speed = movement.magnitude;
            activeAnim.SetFloat("MoveX", movement.x);
            activeAnim.SetFloat("MoveY", movement.y);
            activeAnim.SetFloat("Speed", speed);
            activeAnim.speed = speed > 0.01f ? 1 : 0;
        }


        private void FixedUpdate()
        {
            rb.linearVelocity = movement.normalized * stats.moveSpeed;
        }
        
        
        /// <summary>
        /// Enables one model and disables the others, and updates activeAnim.
        /// </summary>
        private void ActivateModel(GameObject model, Animator anim)
        {
            if (activeModel == model) return;

            // turn off old
            if (activeModel != null) activeModel.SetActive(false);

            // turn on new
                
                model.SetActive(true);
            activeModel = model;
            activeAnim  = anim;
        }
        
        public void IncreaseMaxHealth(int amount)
        {
            stats.maxHealth += amount;
            Debug.Log($"Max Health increased by {amount}. New max health: {stats.maxHealth}");
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