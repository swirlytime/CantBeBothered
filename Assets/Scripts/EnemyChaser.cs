using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    public float attackRange = 0.5f;
    public float detectionRange = 15f;         // How far the enemy can detect the player
    public float surroundStartRange = 8f;      // Start surrounding from this distance
    public float surroundRadius = 2.5f;        // Circle around the player
    public float moveSpeed = 3f;

    private Rigidbody2D _rb;
    private Transform _player;
    private float surroundAngleOffset;
    private bool _isFollowing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Unique angle for surrounding
        surroundAngleOffset = Random.Range(0f, 360f);
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        float distance = Vector2.Distance(transform.position, _player.position);

        if (distance <= detectionRange)
            _isFollowing = true;

        if (_isFollowing)
        {
            Vector2 targetPos;

            if (distance > surroundStartRange)
            {
                // Move directly toward player until close enough
                targetPos = _player.position;
            }
            else
            {
                // Start surrounding from farther away
                targetPos = GetSurroundPoint();
            }

            Vector2 toTarget = targetPos - (Vector2)transform.position;

            if (toTarget.magnitude >= attackRange)
                _rb.linearVelocity = toTarget.normalized * moveSpeed;
            else
                _rb.linearVelocity = Vector2.zero;
        }
    }

    private Vector2 GetSurroundPoint()
    {
        float angleRad = surroundAngleOffset * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * surroundRadius;
        return (Vector2)_player.position + offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (_player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_player.position, surroundRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_player.position, surroundStartRange);
        }
    }
}
