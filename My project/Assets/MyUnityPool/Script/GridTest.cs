using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    private GridNode gird;

    void Start()
    {
        gird = new GridNode(4, 2, 10f, gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gird.SetValue(Customs.GetMouseWorldPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(gird.GetValue(Customs.GetMouseWorldPosition()));
        }
    }
}
