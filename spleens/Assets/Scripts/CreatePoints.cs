using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private int steps;
    [SerializeField] private LineRenderer lineRenderer;
    private float t;
    private List<Vector3> pointArr = new List<Vector3>();

    private void Start()
    {
        //Gathers all objects that have the Point tag and are children of this object
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.tag == "Point")
            {
                Debug.Log(child.gameObject.transform.position);
                points.Add(child.gameObject);
            }
        }
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }

    private void Update()
    {
        pointArr.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            if(i != points.Count - 1)
                CreateSpline(steps, points[i], points[i + 1]);
            //else
            //    CreateSpline(steps, points[i].gameObject.GetComponent<SPoint>(), points[0].gameObject.GetComponent<SPoint>());
        }
    }

    //Polynomial Coefficient Curve
    private Vector3 GetCurve(Vector3 s1, Vector3 s2, Vector3 s3, Vector3 s4)
    {
        //Vector3 a = Vector3.Lerp(s1, s2, t);
        //Vector3 b = Vector3.Lerp(s2, s3, t);
        //Vector3 c = Vector3.Lerp(s3, s4, t);
        //Vector3 d = Vector3.Lerp(a, b, t);
        //Vector3 e = Vector3.Lerp(b, c, t);
        //Vector3 curve = Vector3.Lerp(d, e, t);
        float sqd = Mathf.Pow(t, 2);
        float cud = Mathf.Pow(t, 3);
        Vector3 curve = s1 + t*(-3*s1 + 3*s2)+ sqd*(3*s1 - 6*s2 + 3*s3) + cud*(-s1 + 3*s2 - 3*s3 + s4);
        return curve;
    }

    private void CreateSpline(int steps, GameObject s1, GameObject s2)
    {
        GameObject so1 = s1.gameObject.GetComponent<SPoint>().otherPoint;
        GameObject so2 = s2.gameObject.GetComponent<SPoint>().otherPoint;

        for (int i = 1; i < steps + 1; i++)
        {
            if (i != 0)
                t = 1.0f / i;
            else
                t = 0.0f;
            pointArr.Add(GetCurve(s1.transform.position, so1.transform.position, so2.transform.position, s2.transform.position));
        }
        lineRenderer.positionCount = pointArr.Count;
        lineRenderer.SetPositions(pointArr.ToArray());
    }
}
