using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject player_camera;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Control();
    }

    private void Control()
    {
        float x = 0, y = 0, speed = 5f;

        Vector3 p_dir = default;

        if (Input.GetKey(KeyCode.W))
        {
            x = 1;
            p_dir = player_camera.transform.forward;
            transform.Translate(p_dir.normalized.x * x * Time.deltaTime * speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            x = -1;
            p_dir = player_camera.transform.forward;
            transform.Translate(-p_dir.normalized.x * x * Time.deltaTime * speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            y = 1;
            p_dir = player_camera.transform.right;
            transform.Translate(0, 0, p_dir.normalized.z * y * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            y = -1;
            p_dir = player_camera.transform.right;
            transform.Translate(0, 0, p_dir.normalized.z * y * Time.deltaTime * speed);
        }
    }
}
