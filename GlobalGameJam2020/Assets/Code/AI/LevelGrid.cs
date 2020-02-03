using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private Camera camera;
    //Grid<bool> grid;
    PathFinding pathFinding;

    private void Awake()
    {
        camera = Camera.main;
        pathFinding = new PathFinding(28, 14);
    }

    private void Start()
    {
        // grid = new Grid<bool>(30, 14, .5f, new Vector3(-7f, -4f), 
        //                     (Grid<PathNode> g, int x, int y) => new Bool(g , x, y));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(pos);
            // if (grid.GetGridObject(pos) != null)
            //     grid.SetGridObject(pos, true);
            //grid.SetValue(pos, 56);           
            // pathFinding.GetGrid().GetXY(pos, out int x, out int y);
            // List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
            // if (path != null)
            // {
            //     for (int i = 0; i < path.Count; i++)
            //     {
            //         // Vector3 from = new Vector3(path[i].X, path[i].Y);
            //         // Vector3 to;
            //         // if (i != path.Count - 1)
            //         //     to = new Vector3(path[i + 1].X, path[i + 1].Y);
            //         // else
            //         //     to = from;
            //         // Debug.DrawLine(from * 10f + Vector3.one * 5f,
            //         //     to * 10f + Vector3.one * 5f,
            //         //     Color.green, 100f);
            //     }
            // }

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(grid.GetGridObject(pos).ToString());
        }
    }
}
