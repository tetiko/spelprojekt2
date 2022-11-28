using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject player;

    bool rotateCamera = false;

    [Header("Camera Rotation 1")]
    Vector3 newRotation;
    Vector3 newPosition;

    //Camera rotation selector dropdown
    public rotations ChooseRotation = rotations.RotateRight;  // this public var should appear as a drop down

    [SerializeField] float transitionSpeed = 1f;

    [SerializeField] RotateMode rotateMode;

    public enum rotations // your custom enumeration
    {
        RotateRight,
        RotateLeft,

    }

    //private void Update()
    //{
    //    if (rotateCamera)
    //    {
    //        CameraPositioning();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraPositioning();
        }
    }

    private void CameraPositioning()
    {
        if (ChooseRotation == rotations.RotateRight)
        {
            print("Rotation1");
            //newRotation.eulerAngles = new Vector3(transform.rotation.x, 90, transform.rotation.z); 
            newRotation = new Vector3(player.transform.rotation.x, player.transform.rotation.x + 90, player.transform.rotation.z);
            //newPosition = new Vector3(playerCamera.transform.position.x - 6.3f, playerCamera.transform.position.y, 0);

            player.transform.DORotate(newRotation, transitionSpeed, rotateMode).SetEase(Ease.OutQuint);
            //playerCamera.transform.rotation = newRotation;
            //player.transform.localRotation = newRotation;
            //playerCamera.transform.position = newPosition;

            //playerCamera.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, transitionSpeed * Time.deltaTime);
            //playerCamera.transform.position = Vector3.Slerp(playerCamera.transform.position, newPosition, transitionSpeed * Time.deltaTime);
        }
    }
}
