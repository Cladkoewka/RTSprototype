using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barack : Building
{
    public Transform SpawnPoint;

    public void CreateUnit(GameObject unit)
    {
        GameObject newUnit = Instantiate(unit, SpawnPoint.position, Quaternion.identity);
        Vector3 newUnitStartPosition = SpawnPoint.position + new Vector3(Random.Range(-2f,2f), 0f , Random.Range(-2f, 2f));
        newUnit.GetComponent<Unit>().WhenClickOnGround(newUnitStartPosition);
    }
}
