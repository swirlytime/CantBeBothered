using System.Collections.Generic;
using Models;
using UnityEngine;

namespace PathFinding
{
    public class PathFinder
    {
        private readonly GridManager _grid;

        public PathFinder(GridManager grid)
        {
            _grid = grid;
            _grid.Initialize();
        }
    
        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var startNode = _grid.NodeFromWorldPoint(startPos);
            var targetNode = _grid.NodeFromWorldPoint(targetPos);

            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var currentNode = GetCheapestNode(openSet);
            
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                    return RetracePath(startNode, targetNode);

                AddPotentialNeighbours(currentNode, targetNode, openSet, closedSet);
            }
        
            return null;
        }

        private void AddPotentialNeighbours(Node currentNode, Node targetNode, List<Node> openSet, HashSet<Node> closedSet)
        {
            foreach (var neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                var newCost = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newCost >= neighbour.GCost && openSet.Contains(neighbour)) 
                    continue;
                neighbour.GCost = newCost;
                neighbour.HCost = GetDistance(neighbour, targetNode);
                neighbour.Parent = currentNode;

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }

        private static Node GetCheapestNode(List<Node> openSet)
        {
            var currentNode = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost ||
                    (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }
        
            return currentNode;
        }

        private static List<Node> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
        
            return path;
        }

        private static int GetDistance(Node a, Node b)
        {
            var dstX = Mathf.Abs(a.GridX - b.GridX);
            var dstY = Mathf.Abs(a.GridY - b.GridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
        
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
