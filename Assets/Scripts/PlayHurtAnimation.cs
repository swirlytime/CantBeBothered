using Interfaces;
using UnityEngine;

public class PlayHurtAnimation : MonoBehaviour, IDamageReceived
{
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private Animator _animator;

        private void Awake()
        {
                _animator = GetComponent<Animator>();
        }

        public void OnDamageTaken()
        {
                _animator?.SetTrigger(Hurt);
        }
}