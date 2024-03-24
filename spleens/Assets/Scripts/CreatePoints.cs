using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private int steps;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] GameObject followSpline;
    private float t;
    private List<Vector3> pointArr = new List<Vector3>();
    private List<Vector3> dirArr = new List<Vector3>();

    private void Start()
    {
        
        //Gathers all objects that have the Point tag and are children of this object
        foreach (Transform child in this.transform)
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
        
        int i = 0;
        while (i < points.Count - 1)
        {
            CreateSpline(steps, points[i], points[i + 1]);
            i++;
        }
        if(points.Count > 2)
         CreateSpline(steps, points[i], points[0]);

        if(Input.GetKeyUp(KeyCode.E))
            TraverseSpleen();
    }

    private void TraverseSpleen()
    {
        StartCoroutine(TraverseSpline());
    }

    IEnumerator TraverseSpline()
    {
        int i = 0;
        while (i < lineRenderer.positionCount)
        {
            followSpline.transform.forward = dirArr[i];
            followSpline.transform.position = lineRenderer.GetPosition(i);
            i++;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void CreateSpline(int steps, GameObject s1, GameObject s2)
    {
        GameObject so1 = s1.gameObject.GetComponent<SPoint>().otherPoint;
        GameObject so2 = s2.gameObject.GetComponent<SPoint>().otherPoint;
        float i = 0.0f;
        while (i <= 1.1f)
        {
            t = i;
            Vector3 temp = GetCurve(s1.transform.position, so1.transform.position, so2.transform.position, s2.transform.position);
            dirArr.Add(GetDirection(s1.transform.position, so1.transform.position, so2.transform.position, s2.transform.position));
            pointArr.Add(temp);
            i += (float)(1.0/steps);
        }

        lineRenderer.positionCount = pointArr.Count;
        lineRenderer.SetPositions(pointArr.ToArray());
    }

    //Polynomial Coefficient Curve
    private Vector3 GetCurve(Vector3 s0, Vector3 s1, Vector3 s2, Vector3 s3)
    {
        Vector3 a = Vector3.Lerp(s0, s1, t);
        Vector3 b = Vector3.Lerp(s1, s2, t);
        Vector3 c = Vector3.Lerp(s2, s3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(d, e, t);
    }

    Vector3 GetDirection(Vector3 s0, Vector3 s1, Vector3 s2, Vector3 s3)
    {
        float t2 = Mathf.Pow(t, 2);
        Vector3 dir = s0 * (-3 * (t2) + 6 * (t) - 3) +
            s1 * (9 * (t2) - 12 * (t) + 3) +
            s2 * (-9 * (t2) + 6 * t) +
            s3 * (3 * t2);
        Quaternion rot = Quaternion.Euler(dir.x, dir.y, dir.z);
        return dir;
    }
}
