using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Sheet;

public class ProductItemGridSheet : Sheet
{
    [SerializeField] private TMP_Text _strawberryQuantity, _tomatoQuantity, _blueberryQuantity, _milkQuantity;
    [SerializeField] private Button _sellAll;
    
    private FarmGamePresenter _farmGamePresenter;

    internal void Setup(FarmGamePresenter farmGamePresenter)
    {
        _farmGamePresenter = farmGamePresenter;

        UpdateQuantity();

        _sellAll.onClick.AddListener(SellAllProduct);
    }

    private void SellAllProduct()
    {
        _farmGamePresenter.SellAllProduct();
        UpdateQuantity();
    }

    public override UniTask Cleanup()
    {
        _sellAll.onClick.RemoveAllListeners();
        return UniTask.CompletedTask;
    }

    private void UpdateQuantity()
    {
        _strawberryQuantity.text = _farmGamePresenter.Farm.Inventory.Products[(int)CommodityProductType.Strawberry].ToString();
        _tomatoQuantity.text = _farmGamePresenter.Farm.Inventory.Products[(int)CommodityProductType.Tomato].ToString();
        _blueberryQuantity.text = _farmGamePresenter.Farm.Inventory.Products[(int)CommodityProductType.Blueberry].ToString();
        _milkQuantity.text = _farmGamePresenter.Farm.Inventory.Products[(int)CommodityProductType.Milk].ToString();
    }
}
