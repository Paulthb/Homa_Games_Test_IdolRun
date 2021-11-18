using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Do the spinning of the tourniquet, I try to see what is the good practice to rotate an object endlessly
/// </summary>
public class SpinningTourniquet : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //correct way to make a rotation (this is a memo for me later or maybe for other project)
        //we create a quaternion that keep info on our rotation, here the object will rotate by nearly 360Åã at each frame 
        Quaternion qu = Quaternion.AngleAxis(359, Vector3.up);
        float angle;
        Vector3 axis;

        //we can extract the angle(359) and the axis(up || Y) in the code with "ToAngleAxis"
        qu.ToAngleAxis(out angle, out axis);

        //and then apply it to the angular velocity
        rb.angularVelocity = axis * angle * Mathf.Deg2Rad;
        
        //So in fact we can directly write the angle velocity like these
        Vector3 av = Vector3.up * 359 * Mathf.Deg2Rad;
    }
}
