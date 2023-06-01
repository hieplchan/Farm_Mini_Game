using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Screen;
using UnityScreenNavigator.Runtime.Core.Shared;
using UnityScreenNavigator.Runtime.Core.Sheet;
using Screen = UnityScreenNavigator.Runtime.Core.Screen.Screen;

public class InventoryModal : Modal
{
    [SerializeField] private SheetContainer _itemGridContainer;
    [SerializeField] private Button _seedButton, _productButton;

    private int _seedGridSheetId, _productGridSheetId;

    private FarmGamePresenter _farmGamePresenter;

    public override UniTask Initialize()
    {
        _seedButton.onClick.AddListener(OnSeedButtonClicked);
        _productButton.onClick.AddListener(OnProductButtonClicked);

        var seedHandle = _itemGridContainer.Register("prefab_sheet_seed_item_grid", x =>
        {
            var id = x.sheetId;
            _seedGridSheetId = id;
        });

        var productHandle = _itemGridContainer.Register("prefab_sheet_product_item_grid", x =>
        {
            var id = x.sheetId;
            _productGridSheetId = id;
        });

        return UniTask.CompletedTask;
    }

    public override UniTask Cleanup()
    {
        _seedButton.onClick.RemoveListener(OnSeedButtonClicked);
        _productButton.onClick.RemoveListener(OnProductButtonClicked);
        return UniTask.CompletedTask;
    }

    private async void OnProductButtonClicked()
    {
        await _itemGridContainer.Show(_productGridSheetId, true);
    }

    private async void OnSeedButtonClicked()
    {
        await _itemGridContainer.Show(_seedGridSheetId, true);
    }

    internal void Setup(FarmGamePresenter farmGamePresenter)
    {
        _farmGamePresenter = farmGamePresenter;
    }
}
