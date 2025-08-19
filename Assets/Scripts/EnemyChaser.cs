using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    public float attackRange = 0.5f;
    public float detectionRange = 15f;         // How far the enemy can detect the player
    public float surroundStartRange = 8f;      // Start surrounding from this distance
    public float surroundRadius = 2.5f;        // Circle around the player
    public float moveSpeed = 3f;
    public float repathRate = 0.5f;            // How often to recalc path (seconds)

    [Header("References")]
    public Pathfinding pathfinder;             // Assign in Inspector
    public Transform target;                   // Assign Player in Inspector

    [HideInInspector] public bool canMove = true; // NEW: Dash can temporarily stop movement

    private Rigidbody2D _rb;
    private float _surroundAngleOffset;
    private bool _isFollowing = false;

    private List<Node> _currentPath;
    private int _pathIndex;
    private float _lastRepathTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (target == null)
            Debug.LogWarning("EnemyChaser: No target assigned in Inspector!");

        if (pathfinder == null)
            Debug.LogWarning("EnemyChaser: No Pathfinding assigned in Inspector!");

        _surroundAngleOffset = Random.Range(0f, 360f);
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            _rb.linearVelocity = Vector2.zero; // stop rigidbody completely
            return;
        }

        if (target == null || pathfinder == null) return;

        var distance = Vector2.Distance(transform.position, target.position);

        if (distance <= detectionRange)
            _isFollowing = true;

        if (!_isFollowing)
            return;

        Vector2 targetPos = (distance > surroundStartRange) ? target.position : GetSurroundPoint();

        if (Time.time - _lastRepathTime > repathRate)
        {
            _currentPath = pathfinder.FindPath(transform.position, targetPos);
            _pathIndex = 0;
            _lastRepathTime = Time.time;
        }

        FollowPath();
    }

    private void FollowPath()
    {
        if (_currentPath == null || _currentPath.Count == 0) return;

        if (_pathIndex >= _currentPath.Count)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector3 targetPos = _currentPath[_pathIndex].worldPosition;
        Vector2 toTarget = targetPos - transform.position;

        if (toTarget.magnitude < 0.5f)
        {
            _pathIndex++;
            return;
        }

        if (toTarget.magnitude >= attackRange)
            _rb.linearVelocity = toTarget.normalized * moveSpeed;
        else
            _rb.linearVelocity = Vector2.zero;
    }

    private Vector2 GetSurroundPoint()
    {
        var angleRad = _surroundAngleOffset * Mathf.Deg2Rad;
        var offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * surroundRadius;
        return (Vector2)target.position + offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(target.position, surroundRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, surroundStartRange);

        if (_currentPath != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < _currentPath.Count; i++)
            {
                Gizmos.DrawSphere(_currentPath[i].worldPosition, 0.1f);
                if (i < _currentPath.Count - 1)
                    Gizmos.DrawLine(_currentPath[i].worldPosition, _currentPath[i + 1].worldPosition);
            }
        }
    }
}
