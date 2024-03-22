using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPoint : MonoBehaviour
{
    [SerializeField] public GameObject otherPoint;
    [SerializeField] private Material material;

    private void Start()
    {
        otherPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        otherPoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        otherPoint.gameObject.GetComponent<MeshRenderer>().material = material;
        otherPoint.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z);
        otherPoint.transform.parent = this.transform;
    }
}
