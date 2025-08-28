using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDash : MonoBehaviour
{
    public float dashCooldown = 2f;
    public float preDashDelay = 0.5f;
    public float stopBeforeDistance = 2f;
    public float dashDuration = 0.5f;
    public LayerMask obstacleLayer;      // Walls layer

    // public Transform target;
    [SerializeField] private CircleCollider2D dashTrigger;

    private bool _isDashing;
    private bool _playerInRange;
    private bool _canDash = true;
    private Transform _player;

    private EnemyChaser _chaser;

    private void Awake()
    {
        _chaser = GetComponent<EnemyChaser>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (dashTrigger is null)
            Debug.LogWarning("DashTrigger not assigned! Assign the child trigger collider in Inspector.");
    }

    public void PlayerEnteredTrigger()
    {
        _playerInRange = true;
        TryDash();
    }

    public void PlayerExitedTrigger()
    {
        _playerInRange = false;
    }

    private void TryDash()
    {
        if (_isDashing || !_playerInRange || !_canDash || _player is null)
            return;

        var currentPos = transform.position;
        var targetPos = _player.position;

        var direction = (targetPos - currentPos).normalized;
        var distanceToTarget = Vector3.Distance(currentPos, targetPos);

        var dashDistance = Mathf.Max((distanceToTarget * 2f) - stopBeforeDistance, 0.1f);
        var dashEndPos = currentPos + direction * dashDistance;

        // Raycast to stop at walls
        var hit = Physics2D.Raycast(currentPos, direction, dashDistance, obstacleLayer);
        if (hit.collider)
        {
            dashEndPos = (Vector3)hit.point - direction * 0.1f;
        }
        
        Debug.DrawLine(currentPos, dashEndPos, Color.red, 1f);
        
        StartCoroutine(PerformDash(currentPos, dashEndPos));
    }

    private IEnumerator PerformDash(Vector3 dashStartPos, Vector3 dashEndPos)
    {
        _isDashing = true;
        _canDash = false;

        // Stop Chaser movement during pre-dash windup
        if (_chaser)
            _chaser.canMove = false;

        // Pre-dash windup
        yield return new WaitForSeconds(preDashDelay);

        // Resume Chaser movement during dash
        if (_chaser)
            _chaser.canMove = true;

        var elapsed = 0f;

        while (elapsed < dashDuration)
        {
            var t = elapsed / dashDuration;
            t = t * t * (3f - 2f * t); // smoothstep easing

            var newPos = Vector3.Lerp(dashStartPos, dashEndPos, t);
            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = dashEndPos;
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;

        if (_playerInRange)
            TryDash();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Optional: keep for debug
    }
}
