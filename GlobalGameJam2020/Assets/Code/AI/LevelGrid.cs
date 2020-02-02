using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private Camera camera;
    Grid grid;
    PathFinding pathFinding;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Start()
    {
        //grid = new Grid(24, 16, .5f, new Vector3(-6f,-4f));
        pathFinding = new PathFinding(24, 16);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(pos);
            //grid.SetValue(pos, 56);           
            pathFinding.GetGrid().GetXY(pos, out int x, out int y);
            List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * 10f + Vector3.one * 5f,
                        new Vector3(path[i + 1].X, path[i + 1].Y) * 10f + Vector3.one * 5f,
                        Color.green, 100f);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(grid.GetValue(pos).ToString());
        }
    }
}
