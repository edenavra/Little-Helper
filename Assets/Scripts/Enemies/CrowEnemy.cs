namespace Enemies
{
    using UnityEngine;
    using System.Collections;

    public class CrowEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private float flightHeight = 3f;
        [SerializeField] private float followSpeed = 2f;
        [SerializeField] private float diveSpeed = 6f;
        [SerializeField] private float timeBetweenDives = 5f;
        [SerializeField] private float returnSpeed = 4f;
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private Vector3 shadowMinScale = new Vector3(0.5f, 0.2f, 1f);
        [SerializeField] private Vector3 shadowMaxScale = new Vector3(1.2f, 0.5f, 1f);

        private GameObject player;
        private GameObject shadow;
        private Vector3 originalOffset;
        private float diveCooldown;
        private bool isDiving = false;
        private bool isReturning = false;
        private Vector3 diveShadowPosition;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().freezeRotation = true;
            originalOffset = new Vector3(0, flightHeight, 0);
            diveCooldown = timeBetweenDives;

            shadow = Instantiate(shadowPrefab, transform.position - originalOffset, Quaternion.identity);
            shadow.transform.localScale = shadowMinScale;
        }

        private void Update()
        {
            if (player == null) return;

            diveCooldown -= Time.deltaTime;

            if (!isDiving && !isReturning)
            {
                FollowPlayer();

                if (diveCooldown <= 0f)
                {
                    StartCoroutine(DiveAttack());
                }

                // עדכון צל מתחת לעורב
                Vector3 shadowPos = new Vector3(transform.position.x, transform.position.y - flightHeight, shadow.transform.position.z);
                shadow.transform.position = shadowPos;
                shadow.transform.localScale = shadowMinScale;
            }
            else
            {
                // צל נשאר במקום, משתנה רק בגודל
                shadow.transform.position = diveShadowPosition;

                float distance = Vector2.Distance(transform.position, diveShadowPosition);
                float t = 1f - Mathf.Clamp01(distance / flightHeight);
                shadow.transform.localScale = Vector3.Lerp(shadowMinScale, shadowMaxScale, t);
            }

            // תיקון מיקום צל מהפינה (אם צריך)
            Vector3 scaleCorrection = new Vector3(
                (shadow.transform.localScale.x - shadowMinScale.x) / 2f,
                0,
                0
            );
            shadow.transform.position -= scaleCorrection;
        }

        private void FollowPlayer()
        {
            Vector3 target = player.transform.position + originalOffset;
            transform.position = Vector3.MoveTowards(transform.position, target, followSpeed * Time.deltaTime);
        }

        private IEnumerator DiveAttack()
        {
            isDiving = true;
            diveShadowPosition = new Vector3(player.transform.position.x, player.transform.position.y, shadow.transform.position.z);

            while (Vector3.Distance(transform.position, diveShadowPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, diveShadowPosition, diveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            isDiving = false;
            isReturning = true;
        }

        private void FixedUpdate()
        {
            if (isReturning)
            {
                Vector3 returnTarget = player.transform.position + originalOffset;
                transform.position = Vector3.MoveTowards(transform.position, returnTarget, returnSpeed * Time.fixedDeltaTime);

                if (Vector3.Distance(transform.position, returnTarget) < 0.1f)
                {
                    isReturning = false;
                    diveCooldown = timeBetweenDives;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AttackPlayer(other.gameObject);
            }
            if (other.gameObject.CompareTag("Wall"))
            {
                // שינוי כיוון לפי מצב תנועה
                if (!isDiving && !isReturning)
                {
                    Vector3 collisionNormal = new Vector3(other.contacts[0].normal.x, other.contacts[0].normal.y, 0f);
                    Vector3 awayFromWall = transform.position - collisionNormal;
                    transform.position = awayFromWall + collisionNormal * 0.1f;

                }
                else if (isDiving)
                {
                    // אם העורב מתנגש בקיר במהלך הדאייה – נפסיק את הדאייה ונחזיר אותו
                    StopAllCoroutines();
                    isDiving = false;
                    isReturning = true;
                }
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
            timeBetweenDives = Mathf.Max(1f, timeBetweenDives - level * 0.5f);
        }
    }
}
