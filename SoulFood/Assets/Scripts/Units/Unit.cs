using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public bool selected = false;
    
    private bool clickSelect = false;

	private void Update ()
    {
        if (GetComponent<Renderer>().isVisible && Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
        {
            if (!clickSelect)
            {
                Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
                camPos.y = GameManager.InvertMouseY(camPos.y);
                selected = GameManager.selection.Contains(camPos);
            }
        }

        if (selected)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
	}

    private void OnMouseOver()
    {
        if (GetComponent<Renderer>().isVisible && Input.GetMouseButtonDown(0))
        {
            clickSelect = true;
            selected = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (clickSelect)
            {
                selected = true;
            }

            clickSelect = false;
        }
    }
}
