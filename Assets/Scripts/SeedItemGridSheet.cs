using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Sheet;

public class SeedItemGridSheet : Sheet
{
    [SerializeField] private TMP_Text _strawberryQuantity, _tomatoQuantity, _blueberryQuantity, _cowQuantity;
    private FarmGamePresenter _farmGamePresenter;

    internal void Setup(FarmGamePresenter farmGamePresenter)
    {
        _farmGamePresenter = farmGamePresenter;

        _strawberryQuantity.text = _farmGamePresenter.Farm.Inventory.Seeds[(int)CommodityType.Strawberry].ToString();
        _tomatoQuantity.text = _farmGamePresenter.Farm.Inventory.Seeds[(int)CommodityType.Tomato].ToString();
        _blueberryQuantity.text = _farmGamePresenter.Farm.Inventory.Seeds[(int)CommodityType.Blueberry].ToString();
        _cowQuantity.text = _farmGamePresenter.Farm.Inventory.Seeds[(int)CommodityType.Cow].ToString();
    }
}
