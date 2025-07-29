using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaser : MonoBehaviour
{
    public float attackRange = 0.1f;
    public float detectionRange = 10f;
    public float moveSpeed = 3f;
    
    private Rigidbody2D _rb;
    private Transform _player;
    private bool _isFollowing = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>(); 
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void FixedUpdate()
    {
        if (_player is null) return;
        
        var distance = Vector2.Distance(transform.position, _player.position);

        if (_isFollowing && distance >= attackRange)
        {
            var direction = (_player.position - transform.position).normalized;
            _rb.linearVelocity = direction * moveSpeed;
        }
        else
            _rb.linearVelocity = Vector2.zero;
        
        if (distance <= detectionRange)
            _isFollowing = true;
    }
}
