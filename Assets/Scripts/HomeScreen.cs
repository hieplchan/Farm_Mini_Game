using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Screen;
using UnityScreenNavigator.Runtime.Core.Shared;

using Screen = UnityScreenNavigator.Runtime.Core.Screen.Screen;

public class HomeScreen : Screen
{
    [SerializeField] private Button _saveButton, _loadButton;
    [SerializeField] private Button _shopButton, _inventoryButton;
    [SerializeField] private Button _collectAllButton;
    [SerializeField] private Button _plantButton, _buyButton;

    public override async UniTask Initialize()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _loadButton.onClick.AddListener(OnLoadButtonClicked);
        _shopButton.onClick.AddListener(OnShopButtonClicked);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        _collectAllButton.onClick.AddListener(OnCollectAllButtonClicked);
        _plantButton.onClick.AddListener(OnPlantButtonClicked);
        _buyButton.onClick.AddListener(OnBuyButtonClicked);

        // Simulate loading time
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }
    public override UniTask Cleanup()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        _shopButton.onClick.RemoveListener(OnShopButtonClicked);
        _inventoryButton.onClick.RemoveListener(OnInventoryButtonClicked);
        _collectAllButton.onClick.RemoveListener(OnCollectAllButtonClicked);
        _plantButton.onClick.RemoveListener(OnPlantButtonClicked);
        _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        return UniTask.CompletedTask;
    }

    private void OnCollectAllButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnLoadButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnSaveButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnInventoryButtonClicked()
    {
        var option = new WindowOption("prefab_page_inventory", true);
        ScreenContainer.Of(transform).Push(option);
    }

    private void OnShopButtonClicked()
    {
        var option = new WindowOption("prefab_page_shop", true);
        ScreenContainer.Of(transform).Push(option);
    }

    private async void OnPlantButtonClicked()
    {
        var modalContainer = ModalContainer.Find("Modal_Container");
        var pushOption = new WindowOption("prefab_modal_plant_menu", true);
        modalContainer.Push(pushOption);

        var modal = await pushOption.WindowCreated.WaitAsync();
        var plantMenuModal = (PlantMenuModal)modal;
    }

    private async void OnBuyButtonClicked()
    {
        var modalContainer = ModalContainer.Find("Modal_Container");
        var pushOption = new WindowOption("prefab_modal_plant_menu", true);
        modalContainer.Push(pushOption);

        var modal = await pushOption.WindowCreated.WaitAsync();
        var plantMenuModal = (PlantMenuModal)modal;
    }

    private void OnSellAllButtonClicked()
    {
        throw new NotImplementedException();
    }
}
