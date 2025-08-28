using System.Collections.Generic;
using Models;
using UnityEngine;

namespace PathFinding
{
    public class GridManager
    {
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        public Vector2 Center { get; }
        public LayerMask ObstacleMask { get; }
    
        private Node[,] _grid;

        public GridManager(int width, int height, Vector2 levelCenter, LayerMask obstacleMask, float cellSize = 1f)
        {
            Width = width;
            Height = height;
            Center = levelCenter;
            ObstacleMask = obstacleMask;
            CellSize = cellSize;
        }

        public void Initialize()
        {
            _grid = new Node[Width, Height];
            var worldBottomLeft = Center - new Vector2(Width / 2f, Height / 2f);

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var worldPoint = worldBottomLeft + new Vector2(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2);
                    var walkable = !Physics2D.OverlapCircle(worldPoint, CellSize / 2 * 0.5f, ObstacleMask);
                    _grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            var percentX = Mathf.Clamp01((worldPosition.x + Width / 2f) / Width);
            var percentY = Mathf.Clamp01((worldPosition.y + Height / 2f) / Height);

            var x = Mathf.RoundToInt((Width - 1) * percentX);
            var y = Mathf.RoundToInt((Height - 1) * percentY);

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

                    if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height)
                        neighbours.Add(_grid[checkX, checkY]);
                }
            }
        
            return neighbours;
        }
    }
}
