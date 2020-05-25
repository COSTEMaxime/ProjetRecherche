using Model;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Controller
{
    class PathFinder
    {
        public List<Node> open { get; private set; } = new List<Node>();
        public List<Node> closed { get; private set; } = new List<Node>();
        public List<List<Node>> grid { get; private set; } = new List<List<Node>>();
        int gridRows
        {
            get
            {
                return grid[0].Count;
            }
        }
        int gridCols
        {
            get
            {
                return grid.Count;
            }
        }

        public PathFinder(List<List<Node>> grid)
        {
            this.grid = grid;
        }

        public Stack<Point> Pathfinding(Point from, Point destination)
        {
            foreach (List<Node> list in grid)
            {
                foreach (Node node in list)
                {
                    node.Reset();
                }
            }

            closed.Clear();
            open.Clear();

            Node goalNode = grid[destination.Y][destination.X];
            Node startNode = grid[from.Y][from.X];
            startNode.F = ManhattanDistance(from, destination);

            open.Add(startNode);

            while (open.Count > 0)
            {
                // node with lowest F
                Node node = getBestNode();
                if (node.Position == goalNode.Position)
                {
                    return getPath(node);
                }

                open.Remove(node);
                closed.Add(node);

                List<Node> neighbors = getNeighbors(node);
                foreach (Node n in neighbors)
                {
                    if (!node.CanWalk(n) || closed.Contains(n)) { continue; }

                    float g_score = node.G + 1;
                    float h_score = ManhattanDistance(n.Position, goalNode.Position);
                    float f_score = g_score + h_score;

                    if (closed.Contains(n) && f_score >= n.F)
                        continue;

                    if (!open.Contains(n) || f_score < n.F)
                    {
                        n.G = g_score;
                        n.H = h_score;
                        n.F = f_score;
                        n.parent = node;

                        if (!open.Contains(n))
                        {
                            open.Add(n);
                        }
                    }
                }
            }

            throw new Exception("No path has been found");
        }

        private int ManhattanDistance(Point start, Point goal)
        {
            int dx = goal.X - start.X;
            int dy = goal.Y - start.Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

        // Walk parent nodes to find the path
        private Stack<Point> getPath(Node node)
        {
            Stack<Point> path = new Stack<Point>();
            path.Push(node.Position);

            while (node.parent != null)
            {
                path.Push(node.parent.Position);
                node = node.parent;
            }

            return path;
        }

        private Node getBestNode()
        {
            Node minNode = open[0];

            foreach (Node curr in open)
            {
                if (curr.F < minNode.F)
                {
                    minNode = curr;
                }
            }
            return minNode;
        }

        private List<Node> getNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            int row = node.Position.Y;
            int col = node.Position.X;

            if (col + 1 < gridRows)
            {
                neighbors.Add(grid[row][col + 1]);
            }
            if (col - 1 >= 0)
            {
                neighbors.Add(grid[row][col - 1]);
            }
            if (row - 1 >= 0)
            {
                neighbors.Add(grid[row - 1][col]);
            }
            if (row + 1 < gridCols)
            {
                neighbors.Add(grid[row + 1][col]);
            }

            return neighbors;
        }
    }
}
