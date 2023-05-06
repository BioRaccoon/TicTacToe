using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;
    private bool drag = false;

    private void Start()
    {
        ResetCamera = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (!drag)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
        }

        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.position = ResetCamera;
        }
    }

}
