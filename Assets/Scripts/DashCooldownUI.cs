using UnityEngine;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    public Dash playerDash;
    public Image cooldownImage;
    
    private void Update()
    {
        if (playerDash is not null && cooldownImage is not null)
            cooldownImage.fillAmount = playerDash.DashCooldownProgress;
    }
}
