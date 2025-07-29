using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDeathHandler : MonoBehaviour, IDeathHandler
{ 
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}