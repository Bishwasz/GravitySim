using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;
    private bool isDragging = false;
    private Vector3 initialMousePosition;
    public float dragThreshold = 0.05f; // Threshold to consider it as a drag

    public static bool IsCameraDragDisabled = false; // Static variable to disable camera dragging

    private void LateUpdate()
    {
        if (!IsCameraDragDisabled)
        {
            if (Input.GetMouseButtonDown(0)) // check if mous is clicked and initialize condition to check if drag is being activated
            {
                initialMousePosition = Input.mousePosition;
                drag = true;
            }

            if (Input.GetMouseButton(0)) // if hold 
            {
                if (drag) // if dragging is true
                {
                    Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;// Difference between current mouse pos and camera pos
                    if (!isDragging && Vector3.Distance(initialMousePosition, Input.mousePosition) > dragThreshold)// if for initiation 
                    {
                        isDragging = true;
                        Origin = Camera.main.ScreenToWorldPoint(initialMousePosition);
                    }
                }
            }
            else
            {
                drag = false;
                isDragging = false;
            }

            if (isDragging)
            {
                Camera.main.transform.position = Origin - Difference*1f;//change mous pos new pos = origin -difference
            }
        }
    }
}



