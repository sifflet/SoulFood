using UnityEngine;
using System.Collections;

public class CameraOperator : MonoBehaviour
{
    public float zoomMaxY = 0;
    public float zoomMinY = 0;
    public float zoomSpeed = 0.05f;
    public float zoomTime = 0.25f;
    public Vector3 zoomDestination = Vector3.zero;
    public float moveSpeed = 2f;

	void Update ()
    {
        MoveCamera();
        ZoomCamera();
	}

    private void MoveCamera()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0)
        {
            zoomDestination = Vector3.zero;
        }

        transform.position += new Vector3(moveX * moveSpeed * Time.deltaTime, 0, moveZ * moveSpeed * Time.deltaTime);
    }

    private void ZoomCamera()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue != 0)
        {
            zoomDestination = transform.position + (transform.forward * scrollValue) * zoomSpeed;
        }

        if (zoomDestination != Vector3.zero && zoomDestination.y < zoomMaxY && zoomDestination.y > zoomMinY)
        {
            transform.position = Vector3.Lerp(transform.position, zoomDestination, zoomTime);

            if (transform.position == zoomDestination)
            {
                zoomDestination = Vector3.zero;
            }
        }
        if (transform.position.y > zoomMaxY)
        {
            transform.position = new Vector3(transform.position.x, zoomMaxY, transform.position.z);
        }
        if (transform.position.y < zoomMinY)
        {
            transform.position = new Vector3(transform.position.x, zoomMinY, transform.position.z);
        }
    }
}
