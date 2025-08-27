using UnityEngine;

namespace Models
{
    public class Node
    {
        public readonly bool Walkable;
        public readonly int GridX, GridY;
        public Vector3 WorldPosition;
        public int GCost, HCost;
        public Node Parent;

        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridX = gridX;
            GridY = gridY;
        }

        public int FCost => GCost + HCost;
    }
}