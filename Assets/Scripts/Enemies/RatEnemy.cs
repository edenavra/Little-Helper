using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatEnemy : MonoBehaviour, IEnemy
{
    public float moveSpeed = 2f;
    private Vector2 randomDirection;
    private float changeDirectionTime = 2f;
    private float timeUntilChange;

    private void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        PickNewDirection();
    }

    private void Update()
    {
        MoveRandomly();

        timeUntilChange -= Time.deltaTime;
        if (timeUntilChange <= 0f)
        {
            PickNewDirection();
        }
    }

    private void MoveRandomly()
    {
        transform.Translate(randomDirection * moveSpeed * Time.deltaTime);
    }

    private void PickNewDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized;
        timeUntilChange = changeDirectionTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackPlayer(other.gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))
        {
            randomDirection *= -1; 
            timeUntilChange = changeDirectionTime;
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
        moveSpeed = 2f + level * 0.5f;
    }
}