using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    public Camera RaycastCamera;
    public Building CurrentBuilding;
    public Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();

    private Plane _plane;
    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);

    }
    private void Update()
    {
        if (CurrentBuilding == null)
        {
            return;
        }

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        if (CheckAllow(x, z, CurrentBuilding))
        {
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0))
            {
                InstallBuilding(x, z, CurrentBuilding);
                CurrentBuilding = null;
            }
        }
        else
        {
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    private bool CheckAllow(int xPos, int zPos, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int position = new Vector2Int(xPos + x, zPos + z);
                if (BuildingsDictionary.ContainsKey(position))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void InstallBuilding(int xPos, int zPos, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int position = new Vector2Int(xPos + x,zPos + z);
                BuildingsDictionary.Add(position, building);
            }
        }
    }

    public void CreateBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
    }
}
