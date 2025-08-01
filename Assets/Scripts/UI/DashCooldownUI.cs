using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashCooldownUI : MonoBehaviour
    {
        private Dash _dashScript;
        public Image cooldownImage;

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _dashScript = player.GetComponent<Dash>();
        }
    
        private void Update()
        {
            if (_dashScript is not null && cooldownImage is not null)
                cooldownImage.fillAmount = _dashScript.DashCooldownProgress;
        }
    }
}
