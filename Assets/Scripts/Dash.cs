using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Dash : MonoBehaviour
{
    public float DashSpeed = 10f;
    public float DashDuration = 0.3f;

    private PlayerInput _input;
    private Vector2 _dashDirection;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isDashing;
    
    public bool IsDashing() => _isDashing;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (_input.DashPressed && !_isDashing && _input.MoveDirection != Vector2.zero)
            StartCoroutine(PerformDash(_input.MoveDirection));
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        _isDashing = true;
        _dashDirection = direction.normalized;
        //SetInvulnrable

        if (_animator)
        {
            _animator.SetTrigger("Dash");
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            var dashAnimLength = state.length;
            _animator.speed = dashAnimLength > 0 ? dashAnimLength / DashDuration : 1f;
        }

        var elapsed = 0f;
        while (elapsed < DashDuration)
        {
            _rb.linearVelocity = _dashDirection * DashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _isDashing = false;
        _animator.speed = 1f;
        //RemoveInvlunrable
    }
}
