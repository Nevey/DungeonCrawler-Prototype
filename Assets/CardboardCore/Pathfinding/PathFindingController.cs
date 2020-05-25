using System.Collections.Generic;
using System.Linq;
using CardboardCore.DI;
using CardboardCore.Utilities;
using UnityEngine;

namespace CardboardCore.PathFinding
{
    [Injectable(Singleton = true)]
    public class PathFindingController
    {
        private Vector2Int gridSize;
        private Node[,] grid;

        private Dictionary<Node, Node?> MapBreadcrumbs(Node startNode, Node endNode)
        {
            Dictionary<Node, Node?> visitedDictionary = new Dictionary<Node, Node?> { [startNode] = null };

            for (int i = 0; i < visitedDictionary.Count; i++)
            {
                Node node = visitedDictionary.Keys.ElementAt(i);

                foreach (Node surroundingNode in GetSurroundingTilesHexGrid(node))
                {
                    if (visitedDictionary.ContainsKey(surroundingNode))
                    {
                        continue;
                    }

                    visitedDictionary[surroundingNode] = node;

                    if (surroundingNode == endNode)
                    {
                        break;
                    }
                }
            }

            return visitedDictionary;
        }

        private IEnumerable<Node> GetSurroundingTilesHexGrid(Node node)
        {
            List<Node> nodes = new List<Node>();

            Vector2Int coords = new Vector2Int(node.x, node.y);

            int right = coords.x + 1;
            int left = coords.x - 1;
            int up = coords.y + 1;
            int down = coords.y - 1;

            if (up < gridSize.y)
            {
                // Find tile up
                nodes.Add(grid[coords.x, up]);
            }

            if (right < gridSize.x)
            {
                // Find tile right up
                int y = coords.y;
                if (coords.x % 2 == 0)
                {
                    y += 1;
                }

                if (y >= 0 && y < gridSize.y)
                {
                    nodes.Add(grid[right, y]);
                }

                // Find tile right down
                y = down;
                if (coords.x % 2 == 0)
                {
                    y += 1;
                }

                if (y >= 0 && y < gridSize.y)
                {
                    nodes.Add(grid[right, y]);
                }
            }

            if (down >= 0)
            {
                // Find tile down
                nodes.Add(grid[coords.x, down]);
            }

            if (left >= 0)
            {
                // Find tile left down
                int y = down;
                if (coords.x % 2 == 0)
                {
                    y += 1;
                }

                if (y >= 0 && y < gridSize.y)
                {
                    nodes.Add(grid[left, y]);
                }

                // Find tile left up
                y = coords.y;
                if (coords.x % 2 == 0)
                {
                    y += 1;
                }

                if (y >= 0 && y < gridSize.y)
                {
                    nodes.Add(grid[left, y]);
                }
            }

            return nodes;
        }

        public void CreateGrid(Vector2Int gridSize)
        {
            this.gridSize = gridSize;
            grid = new Node[gridSize.x, gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    grid[x, y] = new Node(new Vector2Int(x, y), 0);
                }
            }
        }

        public IEnumerable<Node> FindPath(Vector2Int from, Vector2Int to)
        {
            Node startNode = new Node(from, 0);
            Node endNode = new Node(to, 0);

            Dictionary<Node, Node?> breadcrumbs = MapBreadcrumbs(startNode, endNode);

            List<Node> path = new List<Node> { endNode };

            while (!path.Contains(startNode))
            {
                Node node = path[path.Count - 1];

                if (!breadcrumbs.ContainsKey(node))
                {
                    throw Log.Exception($"Node with <b>Coords X: {node.x}, Y: {node.y}</b> not available!");
                }

                Node? fromNode = breadcrumbs[node];

                // The start node is not pointing to any other node, so we know we're done creating the path
                if (fromNode == null)
                {
                    break;
                }

                path.Add(fromNode.Value);
            }

            return path;
        }
    }
}