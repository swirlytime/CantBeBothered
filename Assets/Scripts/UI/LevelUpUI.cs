using DefaultNamespace.PlayerLevelUp;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpUI : MonoBehaviour
    {
        public GameObject panel;
        public Button choise1;
        public Button choise2;
        public Button choise3;

        private UpgradeHandler upgradeHandler;

        private void Start()
        {
            upgradeHandler = FindFirstObjectByType<UpgradeHandler>();
            panel.SetActive(false);
            
            choise1.onClick.AddListener(() => Choose(LevelUpgradeEnum.RangeIncrease)); // need to make them somewhat random
            choise2.onClick.AddListener(() => Choose(LevelUpgradeEnum.CooldownReduction));
            choise3.onClick.AddListener(() => Choose(LevelUpgradeEnum.DamageIncrease));
        }

        public void ShowChoises()
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }

        private void Choose(LevelUpgradeEnum type)
        {
            Debug.Log("Applying upgrade: " + type);
            upgradeHandler.ApplyUpgrade(type);
            panel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}