using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour, IDeathHandler
{ 
    [SerializeField] private string startSceneName;
        
    public void OnDeath()
    {
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);
        
        var health = GetComponent<Health>();
        if (health is not null)
            health.RestoreFullHealth();
            
        SceneManager.LoadScene(startSceneName);
    }
}