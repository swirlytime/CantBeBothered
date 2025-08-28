using Interfaces;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour, IDeathHandler
{
    public Animator animator;
    
    private static readonly int DeathTrigger = Animator.StringToHash("DeathTrigger");

    private Collider2D _collider;
    private EnemyChaser _chaserScript;
    private bool _isDying = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _chaserScript = GetComponent<EnemyChaser>();
    }

    public void OnDeath()
    {
        if (_isDying)
            return;
        
        _isDying = true;
        var player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerExperience>()?.AddXp(1); // should be in enemy character sheet of sorts}

        if (_chaserScript)
            _chaserScript.enabled = false;
        if (_collider)
            _collider.enabled = false;

        animator.SetTrigger(DeathTrigger);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}