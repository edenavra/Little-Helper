using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform targetPosition;
    [SerializeField] private RoomTransition _transition;

    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerInside && other.CompareTag("Player"))
        {
            other.transform.position = targetPosition.position;
            _transition.SwitchCamera();
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
