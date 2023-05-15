using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;

public class InstanceObject : MonoBehaviour
{
    public string InstanceName;

    bool IsLoaded = false;
    public bool UpdateXYZ = false;

    public Vector3 rotation;
    public Vector3 scale;
    public Vector3 InstancePosition;

    public int ModelID;
    public int PrevInstance; //Next Connected Model 
    public int NextInstance; //Prev Connected Model

    public int LTGState;

    public Vector3 Oldrotation;
    public Vector3 Oldscale;
    public Vector3 Oldposition;

    public GameObject Prefab;

    public List<MeshCollider> colliders;

    public MaterialJsonHandler.MaterialsJson Mat = new MaterialJsonHandler.MaterialsJson(); 

    public void LoadInstance(InstanceJsonHandler.InstanceJson instance)
    {
        InstanceName = instance.InstanceName;

        rotation = JsonUtil.ArrayToQuaternion(instance.Rotation).eulerAngles;
        scale = JsonUtil.ArrayToVector3(instance.Scale);
        InstancePosition = JsonUtil.ArrayToVector3(instance.Location);

        ModelID = instance.ModelID;
        PrevInstance = instance.PrevInstance;
        NextInstance = instance.NextInstance;

        var TempPos = InstancePosition;
        var TempScale = scale;

        transform.localPosition = InstancePosition;
        transform.localEulerAngles = rotation;
        transform.localScale = scale;

        LTGState = instance.LTGState;

        LoadPrefabs();

        transform.localPosition = TempPos;
        transform.localEulerAngles = rotation;
        transform.localScale = TempScale;

        Oldposition = transform.localPosition;
        Oldrotation = transform.localEulerAngles;
        Oldscale = transform.localScale;

        IsLoaded = true;

    }

    public void LoadPrefabs()
    {
        //Prefab = TrickyMapInterface.Instance.modelObjects[ModelID].GeneratePrefab();
        Prefab.transform.parent = transform;
        Prefab.transform.localRotation = new Quaternion(0, 0, 0, 0);
        Prefab.transform.localPosition = new Vector3(0, 0, 0);
        Prefab.transform.localScale = new Vector3(1, 1, 1);

        //Generate Collisions

    }

    public void SetUpdateMeshes(int NewMeshID)
    {
        int Test = ModelID;
        try 
        {
            ModelID = NewMeshID;
            LoadPrefabs();
            UpdateXYZ = true;
        }
        catch
        {
            ModelID = Test;
        }
    }
}
