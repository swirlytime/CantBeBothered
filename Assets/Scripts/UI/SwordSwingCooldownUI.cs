using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SwordSwingCooldownUI : MonoBehaviour
{
    public SwordSwing _swordScript;
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
