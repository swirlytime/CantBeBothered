using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int currentLevel = 0;
    public float currentXp = 0;
    public float xpToNextLevel = 1;

    public delegate void XpChanged(float currentXp, float xpToNextLevel);
    public event XpChanged OnXpChanged;

    public delegate void LevelChanged(int newLevel);
    public event LevelChanged OnLevelChanged;
    

    public void AddXp(float amount)
    {
        currentXp += amount;

        while (currentXp >= xpToNextLevel)
        {
            currentXp -= xpToNextLevel;
            LevelUp();
        }
        
        OnXpChanged?.Invoke(currentXp, xpToNextLevel);
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel = GetXpNeededForLevelUp(currentLevel);
        OnLevelChanged?.Invoke(currentLevel);
        //Add rewards
    }

    private static float GetXpNeededForLevelUp(int level) => 
        LevelUpTable.GetXpNeededForLevelUp(level);
}
