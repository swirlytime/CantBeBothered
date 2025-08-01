using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    
    private PlayerInput _input;
    private Rigidbody2D _rb;
    private Dash _dash;
    private Animator _animator;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _dash = GetComponent<Dash>();
        _animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_dash && _dash.IsDashing())
            return;
        var moveVector = new Vector3(_input.MoveDirection.x, _input.MoveDirection.y, 0);
		transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void FixedUpdate()
    {
        if (_dash && _dash.IsDashing())
            return;
        
        var velocity = _input.MoveDirection * moveSpeed;
        _rb.linearVelocity = velocity;
        
        if (_animator)
            _animator.SetFloat("Speed", velocity.magnitude);
    }
}
