using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public float cellSize = 1f;
    public LayerMask obstacleMask;
    public Node[,] grid;

    private void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[width, height];
        Vector3 worldBottomLeft = transform.position - new Vector3(width / 2f, height / 2f, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPoint = worldBottomLeft + new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2, 0);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, cellSize / 2 * 0.5f, obstacleMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + width / 2f) / width);
        float percentY = Mathf.Clamp01((worldPosition.y + height / 2f) / height);

        int x = Mathf.RoundToInt((width - 1) * percentX);
        int y = Mathf.RoundToInt((height - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = node.gridX + dx;
                int checkY = node.gridY + dy;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
}

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;
    public int gCost, hCost;
    public Node parent;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost => gCost + hCost;
}
