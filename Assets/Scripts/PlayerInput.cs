using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls _controls;
    public Vector2 MoveDirection { get; private set; }
    public bool DashPressed { get; private set; }

    private void Awake()
    {
        _controls = new PlayerControls();
        
        _controls.Player.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => MoveDirection = Vector2.zero;
        
        _controls.Player.Dash.performed += ctx => DashPressed = true;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void LateUpdate()
    {
        DashPressed = false;
    }
}
