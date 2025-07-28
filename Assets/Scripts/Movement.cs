using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        var moveVector = new Vector3(_input.MoveDirection.x, _input.MoveDirection.y, 0);
		transform.position += moveSpeed * Time.deltaTime * moveVector;
    }
}
