using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid grid;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode PriorNode;
    public int X { get; private set; }
    public int Y { get; private set; }

    public PathNode(Grid grid, int x, int y)
    {
        this.grid = grid;
        X = x;
        Y = y;
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
