using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    public int Price;
    public int XSize;
    public int ZSize;
    public Renderer Renderer;
    public GameObject BuildingMenu;

    private Color _startColor;
    private void Awake()
    {
        _startColor = Renderer.material.color;
    }

    public override void Start()
    {
        base.Start();
        BuildingMenu.SetActive(false);
    }

    public override void Select()
    {
        base.Select();
        BuildingMenu.SetActive(true);
    }

    public override void Unselect()
    {
        base.Unselect();
        BuildingMenu.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;
        for (int i = 0; i < XSize; i ++)
        {
            for (int j = 0; j < ZSize; j++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(i, 0, j) * cellSize, new Vector3(1, 0, 1) * cellSize);
            }
        }
    }

    public void DisplayUnacceptablePosition()
    {
        Renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        Renderer.material.color = _startColor;
    }
}
