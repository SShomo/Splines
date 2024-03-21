using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private float t;

    private void Start()
    {
        //Gathers all objects that have the Point tag and are children of this object
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.tag == "Point")
            {
                points.Add(child.gameObject);
            }
        }
    }

    private void Update()
    {
        GetCurve(points[0].transform.position, points[1].transform.position, points[2].transform.position, points[3].transform.position);
    }

    //Polynomial Coefficient Curve
    private Vector3 GetCurve(Vector3 s1, Vector3 s2, Vector3 s3, Vector3 s4)
    {
        float sqd = Mathf.Pow(t, 2);
        float cud = Mathf.Pow(t, 3);
        Vector3 curve = s1 + t*(-3*s1 + 3*s2)+ sqd*(3*s1 - 6*s2 + 3*s3) + cud*(-s1 + 3*s2 - 3*s3 + s4);
        Debug.Log(curve);
        return curve;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < points.Count; i++)
        {
            Gizmos.color = Color.red;

            //Gizmos.DrawSphere(,)
        }
    }

}
