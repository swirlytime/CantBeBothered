using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Text healthText;
    public Slider healthSlider;
    
    private Health _playerHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var player =  GameObject.FindGameObjectWithTag("Player");
        _playerHealth = player.GetComponent<Health>();

        UpdateUI();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_playerHealth is null) return;
        
        var ratio = _playerHealth.CurrentHealth / _playerHealth.MaxHealth;
        healthSlider.value = ratio;

        healthText.text = $"{Mathf.CeilToInt(_playerHealth.CurrentHealth)} / {_playerHealth.MaxHealth}";
    }
}
