using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class Dash : MonoBehaviour
{
    public float DashSpeed = 20f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;
    public Animator animator;

    public float DashCooldownProgress => Mathf.Clamp01((Time.time - _lastDashTime) / DashCooldown);
    
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private string _dashAnimationName = "Dash";
    
    private PlayerInput _input;
    private Vector2 _dashDirection;
    private Rigidbody2D _rb;
    private bool _isDashing;
    private float _lastDashTime = -Mathf.Infinity;
    
    public bool IsDashing() => _isDashing;
    
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (_input.DashPressed && !_isDashing && _input.MoveDirection != Vector2.zero && Time.time >= _lastDashTime + DashCooldown)
            StartCoroutine(PerformDash(_input.MoveDirection));
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        _isDashing = true;
        _dashDirection = direction.normalized;
        _lastDashTime = Time.time;
        //SetInvulnrable

        if (animator)
        {
            animator.SetTrigger(_dashAnimationName);
            var state = animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == _dashAnimationName);
            if (state is not null)
            {
                var dashAnimLength = state.length;
                animator.speed = dashAnimLength > 0 ? dashAnimLength / DashDuration : 1f;
            }
            else
            {
                animator.speed = 1f;
            }

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
        animator.speed = 1f;
        //RemoveInvlunrable
    }
}
