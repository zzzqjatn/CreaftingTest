using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    private List<GameObject> buildObjs;
    private List<Material> buildMats;

    private float hitDistance = 20.0f;
    private RaycastHit hit;
    private Ray ray;

    private Transform cameraTrans;
    private GameObject prevObj;
    private PrevObjInfo prevInfo;
    private Vector3 prevPos;
    private Vector3 prevRot;
    private Vector3 prevScale;
    private buildType prevType;
    private buildTypeMat prevMat;
    private int layermask;
    private bool IsBuildTime;

    private float offset = 0.2f;
    private float gridSize = 0.1f;
    private bool debugMode = false;

    void Awake()
    {
        buildObjs = new List<GameObject>();
        buildMats = new List<Material>();

        GameObject[] loadObjs = Resources.LoadAll<GameObject>("PBS/BuildPreFab/prevBuild");
        Material[] loadMats = Resources.LoadAll<Material>("PBS/BuildPreFab/Materials");

        for (int i = 0; i < loadObjs.GetLength(0); i++)
        {
            buildObjs.Add(loadObjs[i]);
        }

        for (int i = 0; i < loadMats.GetLength(0); i++)
        {
            buildMats.Add(loadMats[i]);
        }

        cameraTrans = Camera.main.transform;

        //초기값
        IsBuildTime = false;
        prevType = buildType.floor;
        prevMat = buildTypeMat.green;

        prevRot = Vector3.zero;
        layermask = (-1) - (1 << LayerMask.NameToLayer("buildThings")); //해당 레이어만 제외

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {

    }

    void Update()
    {
        ControlKey();
        if (IsBuildTime) { RaycastUpdate(); }
    }

    private void ControlKey()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!debugMode) debugMode = true;
            else if (debugMode) debugMode = false;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (IsBuildTime == true)
            {
                D_prevObj();
                IsBuildTime = false;
            }
            else if (IsBuildTime == false)
            {
                IsBuildTime = true;
                C_prevObj(prevType, 1);
            }
        }

        if (IsBuildTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                D_prevObj();
                prevType++;
                if ((int)prevType > 7) prevType = 0;
                C_prevObj(prevType, 1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                prevRot.y += 45.0f;
                if (prevRot.y > 360.0f) prevRot.y = 0.0f;
                prevObj.transform.localRotation = Quaternion.Euler(prevRot);
            }

            if (Input.GetMouseButtonDown(1))
            {
                //설치
                BuildObj();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.point != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("build"))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void RaycastUpdate()
    {
        //정가운데 화면 레이 쏘기
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit, hitDistance, layermask))
        {
            if (hit.point != null)
            {
                prevSet(hit);   //자리 세팅
                if (debugMode) Debug.DrawLine(ray.origin, hit.point, Color.green);
                if (prevInfo != null || prevInfo != default)
                {
                    if (prevInfo.isBuildAble == false)
                    {
                        prevMat = buildTypeMat.red;
                    }
                    else
                    {
                        prevMat = buildTypeMat.green;
                    }
                    prevObj.GetComponent<Renderer>().material = buildMats[(int)prevMat];
                }
            }
        }
        else
        {
            if (prevMat != buildTypeMat.red)
            {
                prevMat = buildTypeMat.red;
                prevObj.GetComponent<Renderer>().material = buildMats[(int)prevMat];
            }
            prevObj.transform.position = ray.direction * hitDistance;
            if (debugMode) Debug.DrawLine(ray.origin, ray.direction * hitDistance, Color.red);
        }
    }

    private void prevSet(RaycastHit hit2)
    {
        if (prevObj != null || prevObj != default)
        {
            prevPos = hit2.point;
            prevPos -= Vector3.one * offset;
            prevPos /= gridSize;
            prevPos = new Vector3(Mathf.Round(prevPos.x), Mathf.Round(prevPos.y), Mathf.Round(prevPos.z));
            // prevPos += prevScale / 2;
            prevPos *= gridSize;
            prevPos += Vector3.one * offset;
            prevObj.transform.position = prevPos;
        }
    }

    private void C_prevObj(buildType buildtype)
    {
        prevObj = Instantiate(buildObjs[(int)buildtype]);
    }

    private void D_prevObj()
    {
        if (prevObj != null || prevObj != default)
            Destroy(prevObj);
    }

    private void C_prevObj(buildType buildtype, int type)
    {
        if (type == 0)
        {
            prevObj = Instantiate(buildObjs[(int)buildtype]);
            prevObj.layer = LayerMask.NameToLayer("build");
            prevObj.GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.none];
            prevObj.GetComponent<MeshCollider>().isTrigger = false;
        }
        else if (type == 1)
        {
            prevObj = Instantiate(buildObjs[(int)buildtype]);
            prevObj.name = "prevObj";
            prevObj.layer = LayerMask.NameToLayer("buildThings");
            prevObj.GetComponent<MeshCollider>().convex = true;
            prevObj.GetComponent<MeshCollider>().isTrigger = true;
            prevObj.GetComponent<Renderer>().material = buildMats[(int)buildTypeMat.green];
            prevObj.FindChildObj("BuildObj").AddComponent<PrevObjInfo>();
            prevInfo = prevObj.FindChildObj("BuildObj").GetComponent<PrevObjInfo>();
            // prevScale = prevObj.transform.localScale;
            prevRot = prevObj.transform.rotation.eulerAngles;
        }
    }

    private void BuildObj()
    {
        if (prevInfo != null && prevInfo != default && prevInfo.isBuildAble == true)
        {
            GameObject buildObj = Instantiate(buildObjs[(int)prevType], prevPos, Quaternion.Euler(prevRot));
            buildObj.layer = LayerMask.NameToLayer("build");
        }
    }
}

public enum buildType
{
    beam, cut, door, floor, roof, stairs, wall, windowswall
}

public enum buildTypeMat
{
    none, green, red
}