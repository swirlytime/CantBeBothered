using UnityEngine;

public class EnemyDashTrigger : MonoBehaviour
{
    private EnemyDash _enemyDash;

    private void Awake()
    {
        _enemyDash = GetComponentInParent<EnemyDash>();
        if (_enemyDash == null)
        {
            Debug.LogWarning($"{name}: Could not find EnemyDash in parent.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyDash?.PlayerEnteredTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyDash?.PlayerExitedTrigger();
        }
    }
}
