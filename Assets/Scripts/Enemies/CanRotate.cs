using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRotate : MonoBehaviour
{
    //Rotate enemy by setting:
    //canRotate.rotate = true;
    //canRotate.getTargetRotation = true;

    [HideInInspector] public bool rotate = false;
    public int turnSpeed = 10;
    Quaternion targetRotation;
    [HideInInspector] public bool getTargetRotation = false;

    // Update is called once per frame
    void Update()
    {
        if(rotate)
        {
            if(getTargetRotation)
            {
                targetRotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
                getTargetRotation = false;
            }
            //Call the rotate function
            Rotate180();
        }
    }

    //Change direction of the transform with a 180 rotation
    public void Rotate180()
    {
        if(rotate)
        {
            //Slerp to new angles
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            //Flag the direction change as over once the rotation is finished
            if (Quaternion.Angle(transform.rotation, targetRotation) <= 0.05f)
            {      
                rotate = false;
            }     
        } 
    }
}
