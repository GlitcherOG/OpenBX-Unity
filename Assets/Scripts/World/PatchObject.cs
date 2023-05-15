using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;

public class PatchObject : MonoBehaviour
{
    NURBS.Surface surface;

    public string PatchName;
    public Vector4 LightMapPoint; 
    [Space(10)]
    public Vector2 UVPoint1;
    public Vector2 UVPoint2;
    public Vector2 UVPoint3;
    public Vector2 UVPoint4;

    [Space(10)]
    public Vector3 RawControlPoint;
    public Vector3 RawR1C2;
    public Vector3 RawR1C3;
    public Vector3 RawR1C4;
    [Space(5)]
    public Vector3 RawR2C1;
    public Vector3 RawR2C2;
    public Vector3 RawR2C3;
    public Vector3 RawR2C4;
    [Space(5)]
    public Vector3 RawR3C1;
    public Vector3 RawR3C2;
    public Vector3 RawR3C3;
    public Vector3 RawR3C4;
    [Space(5)]
    public Vector3 RawR4C1;
    public Vector3 RawR4C2;
    public Vector3 RawR4C3;
    public Vector3 RawR4C4;

    [Space(10)]
    public int PatchStyle;
    public bool TrickOnlyPatch;
    public int TextureAssigment;
    public int LightmapID;

    [Space(10)]
    public Material MainMaterial;
    public Renderer Renderer;
    public GameObject PointPrefab;

    public void LoadPatch(PatchesJsonHandler.PatchJson import)
    {
        PatchName = import.PatchName;
        LightMapPoint = JsonUtil.ArrayToVector4(import.LightMapPoint);

        UVPoint1 = JsonUtil.ArrayToVector4(import.UVPoint1);
        UVPoint2 = JsonUtil.ArrayToVector4(import.UVPoint2);
        UVPoint3 = JsonUtil.ArrayToVector4(import.UVPoint3);
        UVPoint4 = JsonUtil.ArrayToVector4(import.UVPoint4);

        RawR4C4 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R4C4));
        RawR4C3 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R4C3));
        RawR4C2 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R4C2));
        RawR4C1 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R4C1));
        RawR3C4 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R3C4));
        RawR3C3 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R3C3));
        RawR3C2 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R3C2));
        RawR3C1 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R3C1));
        RawR2C4 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R2C4));
        RawR2C3 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R2C3));
        RawR2C2 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R2C2));
        RawR2C1 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R2C1));
        RawR1C4 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R1C4));
        RawR1C3 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R1C3));
        RawR1C2 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R1C2));
        RawControlPoint = MathTools.FixYandZ(JsonUtil.ArrayToVector3(import.R1C1));

        PatchStyle = import.PatchStyle;
        TrickOnlyPatch = import.TrickOnlyPatch;
        TextureAssigment = import.TextureAssigment;
        LightmapID = import.LightmapID;

        transform.position = RawControlPoint;

        LoadNURBSpatch();
        GetComponent<Renderer>().material = MainMaterial;
        UpdateTexture(TextureAssigment);
    }


    public void LoadNURBSpatch()
    {
        Vector3[,] vertices = new Vector3[4, 4];

        //Vertices
        vertices[0, 0] = transform.InverseTransformPoint(RawControlPoint);
        vertices[1, 0] = transform.InverseTransformPoint(RawR1C2);
        vertices[2, 0] = transform.InverseTransformPoint(RawR1C3);
        vertices[3, 0] = transform.InverseTransformPoint(RawR1C4);

        vertices[0, 1] = transform.InverseTransformPoint(RawR2C1);
        vertices[1, 1] = transform.InverseTransformPoint(RawR2C2);
        vertices[2, 1] = transform.InverseTransformPoint(RawR2C3);
        vertices[3, 1] = transform.InverseTransformPoint(RawR2C4);

        vertices[0, 2] = transform.InverseTransformPoint(RawR3C1);
        vertices[1, 2] = transform.InverseTransformPoint(RawR3C2);
        vertices[2, 2] = transform.InverseTransformPoint(RawR3C3);
        vertices[3, 2] = transform.InverseTransformPoint(RawR3C4);

        vertices[0, 3] = transform.InverseTransformPoint(RawR4C1);
        vertices[1, 3] = transform.InverseTransformPoint(RawR4C2);
        vertices[2, 3] = transform.InverseTransformPoint(RawR4C3);
        vertices[3, 3] = transform.InverseTransformPoint(RawR4C4);

        //Control points
        NURBS.ControlPoint[,] cps = new NURBS.ControlPoint[4, 4];
        int c = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 pos = vertices[i, j];
                cps[i, j] = new NURBS.ControlPoint(pos.x, pos.y, pos.z, 1);
                c++;
            }
        }

        int degreeU = 3;
        int degreeV = 3;

        int resolutionU = 4; //TrickyMapInterface.Instance.settings.PatchResolution; //7;
        int resolutionV = 4; //TrickyMapInterface.Instance.settings.PatchResolution; //7; ()

        surface = new NURBS.Surface(cps, degreeU, degreeV);

        //Update degree
        surface.DegreeU(degreeU);
        surface.DegreeV(degreeV);

        //Update control points
        surface.controlPoints = cps;

        //Build mesh (reusing Mesh to save GC allocation)
        var mesh=surface.BuildMesh(resolutionU, resolutionV);

        //Build UV Points

        cps = new NURBS.ControlPoint[2, 2];

        List<Vector2> vector2s = new List<Vector2>();
        vector2s.Add(UVPoint1);
        vector2s.Add(UVPoint2);
        vector2s.Add(UVPoint3);
        vector2s.Add(UVPoint4);

        vector2s = PointCorrection(vector2s);

        cps[0, 0] = new NURBS.ControlPoint(vector2s[0].x, vector2s[0].y, 0, 1);
        cps[0, 1] = new NURBS.ControlPoint(vector2s[1].x, vector2s[1].y, 0, 1);
        cps[1, 0] = new NURBS.ControlPoint(vector2s[2].x, vector2s[2].y, 0, 1);
        cps[1, 1] = new NURBS.ControlPoint(vector2s[3].x, vector2s[3].y, 0, 1);

        surface = new NURBS.Surface(cps, 1, 1);

        Vector3[] UV = surface.ReturnVertices(resolutionU, resolutionV);

        Vector2[] UV2 = new Vector2[UV.Length];

        for (int i = 0; i < UV.Length; i++)
        {
            UV2[i] = new Vector2(UV[i].x, UV[i].y);
        }
        mesh.uv = UV2;

        //Build Lightmap Points

        cps = new NURBS.ControlPoint[2, 2];

        cps[0, 0] = new NURBS.ControlPoint(0.1f, 0.1f, 0, 1);
        cps[0, 1] = new NURBS.ControlPoint(0.1f, 0.9f, 0, 1);
        cps[1, 0] = new NURBS.ControlPoint(0.9f, 0.1f, 0, 1);
        cps[1, 1] = new NURBS.ControlPoint(0.9f, 0.9f, 0, 1);

        surface = new NURBS.Surface(cps, 1, 1);

        UV = surface.ReturnVertices(resolutionU, resolutionV);

        UV2 = new Vector2[UV.Length];

        for (int i = 0; i < UV.Length; i++)
        {
            UV2[i] = new Vector2(UV[i].x, UV[i].y);
        }

        mesh.uv2 = UV2;


        //Set material
        GetComponent<MeshFilter>().mesh= mesh;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = true;
    }


    public List<Vector2> PointCorrection(List<Vector2> NewList)
    {
        for (int i = 0; i < NewList.Count; i++)
        {
            var TempPoint = NewList[i];
            TempPoint.y = -TempPoint.y;
            NewList[i] = TempPoint;
        }
        return NewList;
    }


    public void UpdateUVPoints()
    {
        LoadNURBSpatch();
    }


    public void UpdateTexture(int a)
    {
        //try
        //{
        //    Renderer.material.SetTexture("_MainTexture", TrickyMapInterface.Instance.textures[TextureAssigment]);
        //    Renderer.material.SetTexture("_Lightmap", TrickyMapInterface.Instance.GrabLightmapTexture(LightMapPoint, LightmapID));
        //    TextureAssigment = a;
        //}
        //catch
        //{
        //    Renderer.material.SetTexture("_MainTexture", TrickyMapInterface.Instance.ErrorTexture);;
        //}
    }
}
