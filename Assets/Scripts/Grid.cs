using UnityEngine;
using System.Collections.Generic;
using Models;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public float cellSize = 1f;
    public LayerMask obstacleMask;
    
    private Node[,] _grid;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[width, height];
        var worldBottomLeft = transform.position - new Vector3(width / 2f, height / 2f, 0);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var worldPoint = worldBottomLeft + new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2, 0);
                var walkable = !Physics2D.OverlapCircle(worldPoint, cellSize / 2 * 0.5f, obstacleMask);
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        var percentX = Mathf.Clamp01((worldPosition.x + width / 2f) / width);
        var percentY = Mathf.Clamp01((worldPosition.y + height / 2f) / height);

        var x = Mathf.RoundToInt((width - 1) * percentX);
        var y = Mathf.RoundToInt((height - 1) * percentY);

        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) 
                    continue;

                var checkX = node.GridX + dx;
                var checkY = node.GridY + dy;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                    neighbours.Add(_grid[checkX, checkY]);
            }
        }
        
        return neighbours;
    }
}
