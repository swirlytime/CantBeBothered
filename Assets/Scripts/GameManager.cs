using PlayerLevelUp;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private PlayerExperience _playerExperience;
    private LevelUpUI _levelUpUI;
    private void Awake()
    {
        if (_instance is null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => HookDelegates();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void HookDelegates()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _playerExperience = player.GetComponent<PlayerExperience>();
        _levelUpUI = FindFirstObjectByType<LevelUpUI>();

        if (_playerExperience is not null && _levelUpUI is not null)
            _playerExperience.OnLevelChanged += HandleLevelChanged;
    }

    private void HandleLevelChanged(int currentLevel)
    {
        _levelUpUI.ShowChoises();
    }

    private void OnDestroy()
    {
        if (_playerExperience is not null)
            _playerExperience.OnLevelChanged -= HandleLevelChanged;
    }
}
