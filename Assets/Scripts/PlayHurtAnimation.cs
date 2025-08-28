using Interfaces;
using UnityEngine;

public class PlayHurtAnimation : MonoBehaviour, IDamageReceived
{
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        public Animator animator;


        public void OnDamageTaken()
        {
                animator?.SetTrigger(Hurt);
        }
}