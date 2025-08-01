using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SwordSwing : MonoBehaviour
    {
        public float swingCooldown = 2f;
        public float enemyDetectionRange = 2.5f;
        public float coneAngle = 90f;
        public float swingDistance = 1f;
        public float minDamage = 1f;
        public float maxDamage = 3f;
        public float swingDuration = 0.3f;
        public Vector2 idleOffset = new Vector2(0.7f, -0.9f);
        public Animator animator;

        private Transform _playerModel;
        private Collider2D _swordCollider;
        private bool _isSwinging = false;
        private float _lastSwingTime = -Mathf.Infinity;
        private PlayerInput _playerInput;
        
        private static readonly Vector2[] SwordOffsets =
        {
            new Vector2(0, -1),     // Right → Bottom
            new Vector2(0.7f, -0.7f), // UpRight → Bottom Left
            new Vector2(1, 0),     // Up → Left
            new Vector2(0.7f, 0.7f),  // UpLeft → Top Left
            new Vector2(0, 1),      // Left → Top
            new Vector2(-0.7f, 0.7f),  // DownLeft → Top Right
            new Vector2(-1, 0),      // Down → Right
            new Vector2(-0.7f, -0.7f), // DownRight → Bottom Right
        };

        public void Awake()
        {
            _playerModel = transform.parent;
            _swordCollider = GetComponent<Collider2D>();
            _swordCollider.enabled = false;
            _playerInput = _playerModel.GetComponent<PlayerInput>();
            
        }
        
        private void Update()
        {
            if (!_isSwinging)
            {
                var target = FindEnemyInCone();
                if (target is not null && Time.time >= _lastSwingTime + swingCooldown)
                    StartCoroutine(SwingRoutine(target.position));
                else
                {
                    var moveDir = _playerInput.MoveDirection;
                    if (moveDir.sqrMagnitude < 0.01f) return;
                    
                    moveDir.Normalize();
                    var angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                    if (angle < 0)
                        angle += 360;

                    var sector = Mathf.RoundToInt(angle / 45f) % 8;
                    var offset = SwordOffsets[sector];

                    transform.position = _playerModel.position + (Vector3)(offset * 0.8f);//not sure about this float
                    var rotAngle = Mathf.Atan2(-moveDir.y, -moveDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, rotAngle - 90f);
                    
                    // transform.position = _playerModel.position + (Vector3)idleOffset;
                    // transform.rotation = Quaternion.Euler(0f, 0f, idleRotation);
                }
            }
        }

        [CanBeNull]
        private Transform FindEnemyInCone()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRange);
            Transform closest = null;
            var closestDist = Mathf.Infinity;

            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Enemy")) continue;

                var toTarget = hit.transform.position - _playerModel.position;
                var angle = Vector2.Angle(_playerModel.right, toTarget.normalized);

                if (angle <= coneAngle / 2f)
                {
                    var dist = toTarget.sqrMagnitude;
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = hit.transform;
                    }
                }
            }
            
            return closest;
        }

        private IEnumerator SwingRoutine(Vector2 targetPosition)
        {
            _isSwinging = true;
            _lastSwingTime = Time.time;

            
            var direction = (targetPosition - (Vector2)_playerModel.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.position = _playerModel.position + (Vector3)(direction * swingDistance);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            _swordCollider.enabled = true;
            
            if (animator is not null)
                animator.SetTrigger("Swing");
            
            yield return new WaitForSeconds(swingDuration);

            _swordCollider.enabled = false;
            _isSwinging = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isSwinging) return;

            if (other.CompareTag("Enemy"))
            {
                var health = other.GetComponent<Health>();
                var damage = Random.Range(minDamage, maxDamage);
                health?.TakeDamage(damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_playerModel is null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_playerModel.position, enemyDetectionRange);
        }
    }
}