using TMPro;
using UnityEngine;

public class UnitBuyButton : MonoBehaviour
{
    public GameObject UnitPrefab;
    public TMP_Text PriceText;
    public Barack Barack;

    private int _price;

    private void Start()
    {
        _price = UnitPrefab.GetComponent<Unit>().Price;
        PriceText.text = _price.ToString();
    }
    public void TryBuy()
    {
        Resources resources = FindObjectOfType<Resources>();
        if (_price <= resources.Money)
        {
            resources.Money -= _price;
            Barack.CreateUnit(UnitPrefab);
        }
    }
}
