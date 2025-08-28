using UnityEngine;

namespace PathFinding
{
    public static class DefaultGrids
    {
        public static GridManager Level1 => new GridManager(
            width: 500,
            height: 500,
            levelCenter: new Vector2(150,100),
            obstacleMask: 1 << LayerMask.NameToLayer("Wall"),
            cellSize: 1);
    }
}