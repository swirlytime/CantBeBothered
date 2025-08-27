using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _controls;
    public Vector2 MoveDirection { get; private set; }

    private void Awake()
    {
        _controls = new PlayerControls();
        
        _controls.Player.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => MoveDirection = Vector2.zero;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     var x = Input.GetAxisRaw("Horizontal");
    //     var y = Input.GetAxisRaw("Vertical");
    //     MoveDirection = new Vector2(x, y).normalized;
    // }
}
