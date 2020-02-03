using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    G = Walking Cost from the start Node
    H = Heuristic Cost to reach End Node
    F = G + H

    Open List - Nodes queed up for searching
    Closed List - Nodes that have already been searched

    Keep going until
    Current Node == End Node
    or
    Open List is Empty
 */
[CreateAssetMenu(menuName = "Game/Path", fileName = "Pathways")]
public class Path : ScriptableObject
{

    [SerializeField] private const int MOVE_STRAIGHT_COST = 10;
    [SerializeField] private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Grid<PathNode> grid;
    [SerializeField] private List<PathNode> openList;
    [SerializeField] private List<PathNode> closeList;


    public Path(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, .5f, new Vector3(-7f, -4f),
                        (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        openList = new List<PathNode>() { startNode };
        closeList = new List<PathNode>();

        // Initialize Grid
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.PriorNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.GetPosition(), endNode.GetPosition());
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                //neighbourNode.gCost = CalculateDistanceCost(currentNode.GetPosition(), neighbourNode.GetPosition());

                int tentativeGCost = currentNode.gCost +
                    CalculateDistanceCost(currentNode.GetPosition(), neighbourNode.GetPosition());

                if (neighbourNode.gCost > tentativeGCost)
                {
                    neighbourNode.PriorNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode.GetPosition(), endNode.GetPosition());
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.X - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            // Left Down
            if (currentNode.Y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            }
            // Left Up
            if (currentNode.Y + 1 < grid.Height)
            {
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
        }
        if (currentNode.X + 1 < grid.Width)
        {
            // Right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
            // Right Down
            if (currentNode.Y - 1 >= 0)
            {
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            }
            // Right Up
            if (currentNode.Y + 1 < grid.Height)
            {
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
        }
        // Down
        if (currentNode.Y - 1 >= 0)
        {
            neighbourList.Add(new PathNode(grid, currentNode.X, currentNode.Y - 1));
        }
        // Up
        if (currentNode.Y + 1 >= 0)
        {
            neighbourList.Add(new PathNode(grid, currentNode.X, currentNode.Y + 1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.PriorNode != null)
        {
            path.Add(currentNode.PriorNode);
            currentNode = currentNode.PriorNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Vector3 a, Vector3 b)
    {
        int xDistance = (int)Mathf.Abs(a.x - b.x);
        int yDistance = (int)Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        int straightCost = MOVE_STRAIGHT_COST * remaining;
        int diagonalCost = MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance);

        return straightCost + diagonalCost;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodeList)
    {
        //if (nodeList.Count <= 0) return new PathNode(grid, 0, 0);

        PathNode lowestFCostNode = nodeList[0];

        foreach (PathNode node in nodeList)
        {
            if (node.fCost < lowestFCostNode.fCost)
                lowestFCostNode = node;
        }

        return lowestFCostNode;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

}
