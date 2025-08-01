using Interfaces;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour, IDeathHandler
{ 
    public void OnDeath()
    {
        var player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerExperience>()?.AddXp(1); // should be in enemy character sheet of sorts
        Destroy(gameObject);
    }
}