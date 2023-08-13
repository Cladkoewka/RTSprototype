using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    SelectedUnits,
    Frame,
    Other
}
public class SelectManager : MonoBehaviour
{
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();
    public Image FrameImage;
    public SelectionState CurrentSelectionState;
    public Camera Camera;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;



    private void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>()) 
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (Hovered)
                {
                    if (Hovered != hitSelectable)
                    {
                        Hovered.OnUnhover();
                        Hovered = hitSelectable;
                        Hovered.OnHover();
                    }
                }
                else
                {
                    Hovered = hitSelectable;
                    Hovered.OnHover();
                }
            }
            else UnhoverCurrent();
        }
        else UnhoverCurrent();

        if (Input.GetMouseButtonUp(0))
        {
            if (Hovered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }
                Select(Hovered);
                CurrentSelectionState = SelectionState.SelectedUnits;
            }

            if (hit.collider.tag == "Ground" && CurrentSelectionState == SelectionState.SelectedUnits)
            {
                int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));
                for (int i = 0; i < ListOfSelected.Count; i++)
                {
                    int row = i / rowNumber;
                    int column = i % rowNumber;
                    Vector3 point = hit.point + new Vector3(row, 0, column);
                    ListOfSelected[i].WhenClickOnGround(point);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            UnselectAll();
            CurrentSelectionState = SelectionState.Other;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 scale = max - min;

            
            if (scale.magnitude > 10)
            {
                FrameImage.enabled = true;
                FrameImage.rectTransform.anchoredPosition = min;
                FrameImage.rectTransform.sizeDelta = scale;
                Rect rect = new Rect(min, scale);

                UnselectAll();
                Unit[] units = FindObjectsOfType<Unit>();
                for (int i = 0; i < units.Length; i++)
                {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(units[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(units[i]);
                    }
                }

                CurrentSelectionState = SelectionState.Frame;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            FrameImage.enabled = false;
            if (ListOfSelected.Count > 0)
            {
                CurrentSelectionState = SelectionState.SelectedUnits;
            }
            else
            {
                CurrentSelectionState = SelectionState.Other;
            }
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (!ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject) 
    {
        if (ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Remove(selectableObject);
        }
    }

    private void UnselectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].Unselect();
        }
        ListOfSelected.Clear();
    }

    private void UnhoverCurrent()
    {
        if (Hovered)
        {
            Hovered.OnUnhover();
            Hovered = null;
        }
    }
}
