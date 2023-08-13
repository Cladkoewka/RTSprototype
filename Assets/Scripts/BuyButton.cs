using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public GameObject BuildingPrefab;
    private BuildingPlacer _buildingPlacer;

    private void Awake()
    {
        _buildingPlacer = FindObjectOfType<BuildingPlacer>();
    }
    public void TryBuy()
    {
        int price = BuildingPrefab.GetComponent<Building>().Price;
        if (FindObjectOfType<Resources>().Money >= price) 
        {
            FindObjectOfType<Resources>().Money -= price;
            _buildingPlacer.CreateBuilding(BuildingPrefab);
        }
    }
}
