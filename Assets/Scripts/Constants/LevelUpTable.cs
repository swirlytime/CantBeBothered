using System.Collections.Generic;
using UnityEngine;

public class LevelUpTable
{
    private static readonly Dictionary<int, float> Table = new()
    {
        { 0, 10 },
        { 1, 20 },
        { 2, 30 },
        { 3, 40 },
    };

    public static float GetXpNeededForLevelUp(int level) =>
        Table.GetValueOrDefault(level, Mathf.Infinity);
}

