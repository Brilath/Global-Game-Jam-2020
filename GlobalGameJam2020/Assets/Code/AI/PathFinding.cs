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

public class PathFinding
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid gameGrid;
    private List<PathNode> openList;
    private List<PathNode> closeList;


    public PathFinding(int width, int height)
    {
        gameGrid = new Grid(width, height, .5f, new Vector3(-6f, -4f));
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = new PathNode(gameGrid, startX, startY);
        PathNode endNode = new PathNode(gameGrid, endX, endY);
        openList = new List<PathNode>() { startNode };
        closeList = new List<PathNode>();

        for(int x = 0; x < gameGrid.Width; x++)
        {
            for(int y = 0; y < gameGrid.Height; y++)
            {
                PathNode pathNode = new PathNode(gameGrid, x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.PriorNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.GetPosition(), endNode.GetPosition());
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;

                neighbourNode.gCost = CalculateDistanceCost(startNode.GetPosition(), neighbourNode.GetPosition());

                int tentativeGCost = currentNode.gCost + 
                    CalculateDistanceCost(currentNode.GetPosition(), neighbourNode.GetPosition());

                if(neighbourNode.gCost > tentativeGCost)
                {
                    neighbourNode.PriorNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode.GetPosition(), neighbourNode.GetPosition());
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

        if(currentNode.X - 1 >= 0 )
        {
            // Left
            neighbourList.Add(new PathNode(gameGrid, currentNode.X - 1, currentNode.Y));
            // Left Down
            if(currentNode.Y - 1 >= 0)
            {
                neighbourList.Add(new PathNode(gameGrid, currentNode.X - 1, currentNode.Y -1 ));
            }
            // Left Up
            if (currentNode.Y + 1 < gameGrid.Height)
            {
                neighbourList.Add(new PathNode(gameGrid, currentNode.X - 1, currentNode.Y + 1));
            }
        }
        if (currentNode.X + 1 < gameGrid.Width)
        {
            // Right
            neighbourList.Add(new PathNode(gameGrid, currentNode.X + 1, currentNode.Y));
            // Right Down
            if(currentNode.Y - 1 >= 0)
            {
                neighbourList.Add(new PathNode(gameGrid, currentNode.X + 1, currentNode.Y - 1));
            }
            // Right Up
            if (currentNode.Y + 1 < gameGrid.Height)
            {
                neighbourList.Add(new PathNode(gameGrid, currentNode.X + 1, currentNode.Y + 1));
            }            
        }
        // Down
        if (currentNode.Y - 1 >= 0)
        {
            neighbourList.Add(new PathNode(gameGrid, currentNode.X, currentNode.Y - 1));
        }
        // Up
        if (currentNode.Y + 1 >= 0)
        {
            neighbourList.Add(new PathNode(gameGrid, currentNode.X, currentNode.Y + 1));
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.PriorNode != null)
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

        int straightCost = MOVE_STRAIGHT_COST * Mathf.Min(xDistance, yDistance);
        int diagonalCost = MOVE_DIAGONAL_COST * remaining;

        return straightCost + diagonalCost;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodeList)
    {
        if (nodeList.Count <= 0) return new PathNode(gameGrid, 0, 0);

        PathNode lowestFCostNode = nodeList[0];

        foreach(PathNode node in nodeList)
        {
            if (lowestFCostNode.fCost < node.fCost)
                lowestFCostNode = node;
        }

        return lowestFCostNode;
    }

    public Grid GetGrid()
    {
        return gameGrid;
    }

}
