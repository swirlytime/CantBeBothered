using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class XpBarUI : MonoBehaviour
    {
        public Text xpText;
        public Slider xpSlider;
    
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            var player =  GameObject.FindGameObjectWithTag("Player");
            var playerExperience = player.GetComponent<PlayerExperience>();

            playerExperience.OnXpChanged += UpdateUI;
            playerExperience.OnLevelChanged += ShowLevelUp;
        
            UpdateUI(playerExperience.currentXp, playerExperience.xpToNextLevel);
        }

        private void UpdateUI(float currentXp, float xpToNextLevel)
        {
            var ratio = currentXp / xpToNextLevel;
            xpSlider.value = ratio;
        
            xpText.text = $"{Mathf.CeilToInt(currentXp)} / {Mathf.CeilToInt(xpToNextLevel)}";
        }

        private void ShowLevelUp(int newLevel)
        {
            //Do Something
        }
    }
}
