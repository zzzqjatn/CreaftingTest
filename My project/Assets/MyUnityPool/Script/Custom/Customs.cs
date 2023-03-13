using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Customs
{
    public static TextMesh createWolrdText(
        string text, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder,
         Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40)
    {
        if (color == null) color = Color.white;
        return createWolrdText(parent, text, (Color)color, localPosition, fontSize, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh createWolrdText(
        Transform parent, string text, Color color, Vector3 localPosition, int fontSize,
         TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.alignment = textAlignment;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    // public static Vector3 GetMouseWorldPosition()
    // {
    //     Vector3 t_mousePosition = Input.mousePosition;
    //     t_mousePosition.z = 50.0f;
    //     Vector3 vec = GetMouseWorldPositionWithZ(t_mousePosition, Camera.main);
    //     vec.z = 0f;
    //     Debug.Log(vec);
    //     return vec;
    // }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 t_mousePosition = Input.mousePosition;
        t_mousePosition.z = t_mousePosition.y;
        t_mousePosition.y = 0;
        Vector3 vec = GetMouseWorldPositionWithY(t_mousePosition, Camera.main);
        Debug.Log(vec);
        return vec;
    }

    // public static Vector3 GetMouseWorldPositionWithZ()
    // {
    //     return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    // }

    // public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    // {
    //     return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    // }

    public static Vector3 GetMouseWorldPositionWithY(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
