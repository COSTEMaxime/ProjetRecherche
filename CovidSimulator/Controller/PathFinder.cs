using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    class PathFinder
    {
        public List<Entity> open { get; private set; } = new List<Entity>();
        public List<Entity> closed { get; private set; } = new List<Entity>();
        public List<List<Entity>> grid { get; private set; } = new List<List<Entity>>();
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

        public PathFinder(List<List<Entity>> grid)
        {
            this.grid = grid;
        }

        public List<Entity> Pathfinding(Point from, Point destination)
        {
            closed.Clear();
            open.Clear();

            Entity goalNode = grid[destination.Y][destination.X];
            Entity startNode = grid[from.Y][from.X];
            startNode.F = ManhattanDistance(from, destination);

            open.Add(startNode);

            while (open.Count > 0)
            {
                // node with lowest F
                Entity node = getBestNode();
                if (node.position == goalNode.position)
                {
                    Console.WriteLine("Goal reached");
                    return getPath(node);
                }

                open.Remove(node);
                closed.Add(node);

                List<Entity> neighbors = getNeighbors(node);
                foreach (Entity n in neighbors)
                {
                    if (!node.CanWalk(n) || closed.Contains(n)) { continue; }

                    float g_score = node.G + 1;
                    float h_score = ManhattanDistance(n.position, goalNode.position);
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
        private List<Entity> getPath(Entity node)
        {
            List<Entity> path = new List<Entity>();
            path.Add(node);

            while (node.parent != null)
            {
                path.Add(node.parent);
                node = node.parent;
            }

            path.Add(node);
            return path;
        }

        private Entity getBestNode()
        {
            Entity minNode = open[0];

            foreach (Entity curr in open)
            {
                if (curr.F < minNode.F)
                {
                    minNode = curr;
                }
            }
            return minNode;
        }

        private List<Entity> getNeighbors(Entity node)
        {
            List<Entity> neighbors = new List<Entity>();

            int row = node.position.Y;
            int col = node.position.X;

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
