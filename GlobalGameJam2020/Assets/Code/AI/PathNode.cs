using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;

    /*
        G = Walking Cost from the start Node
        H = Heuristic Cost to reach End Node
        F = G + H
    */
    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode PriorNode;
    public int X { get; private set; }
    public int Y { get; private set; }

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        X = x;
        Y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(X, Y);
    }

    public override string ToString()
    {
        return $"{X} , {Y}";
    }
}
