using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    [SerializeField] private float rotCamXAxisSpeed = 5f; // 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f; // 카메라 y축 회전속도

    private float limitMinX = -80; // 카메라 x축 회전 범위 (최소)
    private float limitMaxX = 50; // 카메라 x축 회전 범위 (최대)

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우 위 / 아래 이동으로 카메라 x축 회전

    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    // 카메라 x축 회전의 경우 회전 범위를 설정
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }

    void Update()
    {
        UpdateRotate();
    }

    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        CalculateRotation(mouseX, mouseY);
    }
}
