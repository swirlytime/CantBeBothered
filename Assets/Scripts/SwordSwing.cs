using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SwordSwing : MonoBehaviour
    {
        public float swingCooldown = 3f;
        public float enemyDetectionRange = 2.5f;
        public float coneAngle = 90f;
        public float swingDistance = 1f;
        public float minDamage = 1f;
        public float maxDamage = 3f;
        public float swingDuration = 0.5f;
        public Animator animator;
        public Transform playerModel;
        
        private Collider2D _swordCollider;
        private bool _isSwinging = false;
        private float _lastSwingTime = -Mathf.Infinity;
        private PlayerInput _playerInput;

        private static readonly Vector2[] SwordOffsets =
        {
            //moving -> sword position
            new Vector2(0, -1),     // Right → Bottom
            new Vector2(0.7f, -0.7f), // UpRight → Bottom Left
            new Vector2(1, 0),     // Up → Left
            new Vector2(0.7f, 0.7f),  // UpLeft → Top Left
            new Vector2(0, -1),      // Left → Bottom
            new Vector2(-0.7f, 0.7f),  // DownLeft → Top Right
            new Vector2(-1, 0),      // Down → Right
            new Vector2(-0.7f, -0.7f), // DownRight → Bottom Right
        };

        public float CooldownProgress => Mathf.Clamp01((Time.time - _lastSwingTime) / swingCooldown);
        
        public void Awake()
        {
            playerModel = transform.parent;
            _swordCollider = GetComponent<Collider2D>();
            _swordCollider.enabled = false;
            _playerInput = playerModel.GetComponent<PlayerInput>();
            animator.enabled = false;
        }
        
        private void Update()
        {
            if (_isSwinging) return;
            
            var moveDir = _playerInput.MoveDirection;
                
            if (Time.time >= _lastSwingTime + swingCooldown && TryFindClosestEnemy(out var target))
                StartCoroutine(SwingRoutine(target.position, 90)); //get from player upgrades/level thing.
            else
            {
                if (moveDir.sqrMagnitude < 0.01f) return;
                    
                moveDir.Normalize();
                var angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                if (angle < 0)
                    angle += 360;

                var sector = Mathf.RoundToInt(angle / 45f) % 8;
                var offset = SwordOffsets[sector];

                var newRotation = 90f + angle;
                transform.position = playerModel.position + (Vector3)(offset * 0.8f);//not sure about this float
                transform.rotation = Quaternion.Euler(0, 0, newRotation);
            }
        }

        [CanBeNull]
        private bool TryFindClosestEnemy(out Transform closest)
        {
            closest = null;
            var playerPos = new Vector2(playerModel.transform.position.x, playerModel.transform.position.y);
            var hits = Physics2D.OverlapCircleAll(playerPos, enemyDetectionRange);
            if (hits.Length == 0)
                return false;
            
            var closestDist = Mathf.Infinity;
            
            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Enemy")) continue;
                var toTarget = Vector2.Distance(playerPos, hit.ClosestPoint(playerPos));

                if (closestDist <= toTarget)
                    continue;
                
                closestDist = toTarget;
                closest = hit.transform;
            }

            return closest is not null;
        }

        private IEnumerator SwingRoutine(Vector2 targetPosition, float swingAngle)
        {
            _isSwinging = true;
            _lastSwingTime = Time.time;
            _swordCollider.enabled = true;
            
            var direction = (targetPosition - (Vector2)playerModel.position).normalized;
            // transform.position = (Vector2)playerModel.position + direction * swingDistance;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            
            var startAngle = angle - (swingAngle / 2f);
            var endAngle = angle + (swingAngle / 2f);

            var elapsed = 0f;
            while (elapsed < swingDuration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / swingDuration;
                
                var currentAngle = Mathf.Lerp(startAngle, endAngle, t);
                var offset = new Vector2(
                    Mathf.Cos((currentAngle+90f) * Mathf.Deg2Rad),
                    Mathf.Sin((currentAngle+90f) * Mathf.Deg2Rad)) * swingDistance;
                
                transform.position = (Vector2)playerModel.position + offset;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                yield return null;
            }
            
            if (animator is not null)    
                animator.enabled = false;
            // _swordSprite.enabled = true;
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
            if (playerModel is null) return;

            if (_isSwinging)
            {
                var angle = 90f; //Get from player level thing
                var halfFOV = angle / 2f;
                var rayRange = 5f;

                var coneDirection = 180f;
                
                var upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
                var downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

                var upRayDirection = upRayRotation * transform.right * rayRange;
                var downRayDirection = downRayRotation * transform.right * rayRange;

                Gizmos.DrawRay(transform.position, upRayDirection);
                Gizmos.DrawRay(transform.position, downRayDirection);
                Gizmos.DrawLine(transform.position + downRayDirection, transform.position + upRayDirection);
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerModel.position, enemyDetectionRange);
        }
    }
}