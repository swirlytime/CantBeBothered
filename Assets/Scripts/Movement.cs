using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    private static readonly int MoveXAnim = Animator.StringToHash("MoveX");
    private static readonly int MoveYAnim = Animator.StringToHash("MoveY");
    private static readonly int SpeedAnim = Animator.StringToHash("Speed");
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
        _animator = GetComponentInChildren<Animator>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_dash && _dash.IsDashing())
            return;
        var moveVector = new Vector3(_input.MoveDirection.x, _input.MoveDirection.y, 0);
		transform.position += moveSpeed * Time.deltaTime * moveVector;

        _animator.SetFloat(MoveXAnim, _input.MoveDirection.x);
        _animator.SetFloat(MoveYAnim, _input.MoveDirection.y);
    }

    private void FixedUpdate()
    {
        if (_dash && _dash.IsDashing())
            return;
        
        var velocity = _input.MoveDirection * moveSpeed;
        _rb.linearVelocity = velocity;
        
        if (_animator)
            _animator.SetFloat(SpeedAnim, velocity.magnitude);
    }
}
