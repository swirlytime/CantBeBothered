using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBarUI : MonoBehaviour
    {
        public Text hpText;
        public Slider healthSlider;
    
        private Health _playerHealth;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            var player =  GameObject.FindGameObjectWithTag("Player");
            _playerHealth = player.GetComponent<Health>();
            _playerHealth.OnHealthChanged += UpdateUI;
            
            UpdateUI(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }

        private void UpdateUI(float currentHealth, float maxHealth)
        {
            if (_playerHealth is null) return;
        
            var ratio = _playerHealth.CurrentHealth / _playerHealth.MaxHealth;
            healthSlider.value = ratio;

            hpText.text = $"{Mathf.CeilToInt(_playerHealth.CurrentHealth)} / {_playerHealth.MaxHealth}";
        }
    }
}
