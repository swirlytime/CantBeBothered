using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public Vector3 offset;

    private bool _isFollowingPlayer = false;
    
    // Update is called once per frame
    void Update()
    {
        if (!_isFollowingPlayer && player.position.y >= 0f)
            _isFollowingPlayer = true;

        if (_isFollowingPlayer)
        {
            var targetPosition = player.position + offset;
            targetPosition.z = -10f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        }
    }
}
