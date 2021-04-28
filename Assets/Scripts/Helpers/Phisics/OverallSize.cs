using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallSize : MonoBehaviour
{
    public string Name;
    public float length;
    public RaycastHit2D raycast;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        raycast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), length,mask);
    }
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.down) * length;
        Gizmos.DrawRay(transform.position, direction);
    }
}
