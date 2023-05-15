using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;

public class SplineSegmentObject : MonoBehaviour
{
    [Space(10)]
    public Vector3 Point1;
    public Vector3 Point2;
    public Vector3 Point3;
    public Vector3 Point4;
    [Space(10)]
    public Vector4 ScalingPoint;
    
    public GameObject PointPrefab;

    //private int curveCount = 0;
    private int SEGMENT_COUNT = 50;

    public void LoadSplineSegment(SplineJsonHandler.SegmentJson segments)
    {
        Point1 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(segments.Point1));
        Point2 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(segments.Point2));
        Point3 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(segments.Point3));
        Point4 = MathTools.FixYandZ(JsonUtil.ArrayToVector3(segments.Point4));

        ScalingPoint = JsonUtil.ArrayToVector4(segments.Unknown);

    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
