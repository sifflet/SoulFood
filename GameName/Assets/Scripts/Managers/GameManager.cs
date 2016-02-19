using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject unitPrefab;
    public int numUnits = 1;
    public RectTransform selectionHighlight;
    public static Rect selection = new Rect(0, 0, 0, 0);

    private Vector3 startClick;
    private List<UnitManager> units;
    private static Vector3 targetPosition;
    private static List<string> passables;

    private void Start()
    {
        startClick = -Vector3.one;
        units = new List<UnitManager>();
        targetPosition = Vector3.zero;
        passables = new List<string>() { "Floor" };
        selectionHighlight.gameObject.SetActive(false);
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < numUnits; ++i)
        {
            units.Add(new UnitManager());
            units[i].instance = Instantiate(unitPrefab, new Vector3(i, 0, i), new Quaternion()) as GameObject;
            units[i].Setup();
        }
    }

    private void Update()
    {
        CheckSelection();
        CheckMovement();
        Cleanup();
    }

    private void FixedUpdate()
    {
        UpdateUnits();
    }

    private void CheckSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionHighlight.gameObject.SetActive(true);
            startClick = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectionHighlight.gameObject.SetActive(false);
            startClick = -Vector3.one;
            selectionHighlight.sizeDelta = new Vector2(0, 0);
            selectionHighlight.position = new Vector2(0, 0);
        }

        if (Input.GetMouseButton(0))
        {
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));

            if (selection.width < 0)
            {
                selection.x = Input.mousePosition.x;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y = InvertMouseY(Input.mousePosition.y);
                selection.height = -selection.height;
            }

            float selectWidth = Input.mousePosition.x - startClick.x;
            float selectHeight = startClick.y - Input.mousePosition.y;
            selectionHighlight.position = new Vector2(startClick.x + selectWidth / 2, Input.mousePosition.y + selectHeight / 2);
            selectionHighlight.sizeDelta = new Vector2(selection.width, selection.height);
        }
    }

    private void UpdateUnits()
    {
        foreach (UnitManager unit in units)
        {
            unit.Update();
        }
    }

    private void CheckMovement()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Vector3 destination = GetDestination();

            foreach (UnitManager unit in units)
            {
                if (unit.IsSelected())
                {
                    unit.Move(destination);
                }
            }
        }
    }

    public static float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    private void Cleanup()
    {
        if (!Input.GetMouseButtonUp(1))
        {
            targetPosition = Vector3.zero;
        }
    }

    private Vector3 GetDestination()
    {
        if (targetPosition == Vector3.zero)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                while (!passables.Contains(hit.transform.gameObject.name))
                {
                    if (!Physics.Raycast(hit.point + ray.direction * 0.1f, ray.direction, out hit))
                    {
                        break;
                    }
                }
            }

            if (hit.transform != null)
            {
                targetPosition = hit.point;
            }
        }

        return targetPosition;
    }
}