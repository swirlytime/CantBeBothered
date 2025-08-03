using UnityEngine;

public class EnemyDashTrigger : MonoBehaviour
{
    private EnemyDash enemyDash;

    private void Awake()
    {
        enemyDash = GetComponentInParent<EnemyDash>();
        if (enemyDash == null)
        {
            Debug.LogWarning($"{name}: Could not find EnemyDash in parent.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{name}: Triggered by player.");
            enemyDash?.PlayerEnteredTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{name}: Player exited trigger.");
            enemyDash?.PlayerExitedTrigger();
        }
    }
}
