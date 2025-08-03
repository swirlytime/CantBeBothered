using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDash : MonoBehaviour
{
    public float dashCooldown = 2f;
    public float stopBeforeDistance = 2f;
    public float dashDuration = 0.5f;
    public float damageRadius = 0.5f;
    public float damageAmount = 1f;
    public LayerMask playerLayer;

    public Transform target;
    [SerializeField] private CircleCollider2D dashTrigger;

    private bool _isDashing = false;
    private bool playerInRange = false;
    private bool canDash = true;
    private bool hasDealtDamage = false;

    private void Awake()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
                target = playerObj.transform;
            else
                Debug.LogWarning("Player not found; assign manually!");
        }

        if (dashTrigger == null)
            Debug.LogWarning("DashTrigger not assigned! Assign the child trigger collider in Inspector.");
    }

    public void PlayerEnteredTrigger()
    {
        playerInRange = true;
        TryDash();
    }

    public void PlayerExitedTrigger()
    {
        playerInRange = false;
    }

    public void TryDash()
    {
        if (_isDashing || !playerInRange || !canDash || target == null)
            return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;

        Vector3 direction = (targetPos - currentPos).normalized;
        float distanceToTarget = Vector3.Distance(currentPos, targetPos);

        float dashDistance = Mathf.Max((distanceToTarget * 2f) - stopBeforeDistance, 0.1f);
        Vector3 dashEndPos = currentPos + direction * dashDistance;

        Debug.DrawLine(currentPos, dashEndPos, Color.red, 1f);

        StartCoroutine(PerformDash(currentPos, dashEndPos));
    }

    private IEnumerator PerformDash(Vector3 dashStartPos, Vector3 dashEndPos)
    {
        _isDashing = true;
        canDash = false;
        hasDealtDamage = false;

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            t = t * t * (3f - 2f * t); // smoothstep easing

            Vector3 newPos = Vector3.Lerp(dashStartPos, dashEndPos, t);
            transform.position = newPos;

            if (!hasDealtDamage)
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadius, playerLayer);
                if (hit != null && hit.CompareTag("Player"))
                {
                    var health = hit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage(damageAmount);
                        Debug.Log($"{name} damaged player during dash!");
                        hasDealtDamage = true;
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = dashEndPos;
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        if (playerInRange)
            TryDash();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
