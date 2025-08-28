using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SwordSwingCooldownUI : MonoBehaviour
    {
        private SwordSwing _swordScript;
        public Image cooldownImage;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _swordScript = player.GetComponentInChildren<SwordSwing>();
        }
    
        private void Update()
        {
            if (_swordScript is not null && cooldownImage is not null)
                cooldownImage.fillAmount = _swordScript.CooldownProgress;
        }
    }
}
