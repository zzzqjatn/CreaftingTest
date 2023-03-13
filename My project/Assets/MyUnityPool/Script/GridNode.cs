using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    private GameObject originPosition;
    private int width;
    private int height;
    private float cellsize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public GridNode(int width_, int height_, float cellsize_, GameObject originPosition_)
    {
        this.width = width_;
        this.height = height_;
        this.cellsize = cellsize_;
        this.originPosition = originPosition_;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int z = 0; z < gridArray.GetLength(1); z++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                debugTextArray[x, z] = Customs.createWolrdText(gridArray[x, z].ToString(), Color.white, TextAnchor.MiddleCenter, TextAlignment.Center,
                0, originPosition.transform, GetWorldPosition(x, z) + new Vector3(cellsize, cellsize) * 0.5f, 20);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public void SetValue(int x, int z, int value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            debugTextArray[x, z].text = gridArray[x, z].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellsize + originPosition.transform.position;
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition.transform.position).x / cellsize);
        z = Mathf.FloorToInt((worldPosition - originPosition.transform.position).z / cellsize);
    }

    public int GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
