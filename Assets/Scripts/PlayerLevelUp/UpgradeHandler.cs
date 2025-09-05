using System;
using UnityEngine;

namespace DefaultNamespace.PlayerLevelUp
{
    public class UpgradeHandler : MonoBehaviour
    {
        [Header("Sword Stats")]
        public float swordRange = 1f;

        public float swordDamage = 1f;
        public float swordCooldown = 1f;

        [Header("Cooldown Stats")] public float dashCooldown = 1f;

        public void ApplyUpgrade(LevelUpgradeEnum type)
        {
            switch (type)
            {
                case LevelUpgradeEnum.RangeIncrease:
                    swordRange += 1f;
                    break;
                case LevelUpgradeEnum.CooldownReduction:
                    swordCooldown *= 0.9f;
                    dashCooldown *= 0.9f;
                    break;
                case LevelUpgradeEnum.DamageIncrease:
                    swordDamage += 1f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}