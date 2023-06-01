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
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _sellAllProductButton;
    [SerializeField] private Button _plantButton;

    public override async UniTask Initialize()
    {
        _shopButton.onClick.AddListener(OnShopButtonClicked);
        _inventoryButton.onClick.AddListener(OnInventoryButtonClicked);
        _sellAllProductButton.onClick.AddListener(OnSellAllButtonClicked);
        _plantButton.onClick.AddListener(OnPlantButtonClicked);

        // Simulate loading time
        await UniTask.Delay(TimeSpan.FromSeconds(1));
    }

    public override UniTask Cleanup()
    {
        _shopButton.onClick.RemoveListener(OnShopButtonClicked);
        _inventoryButton.onClick.RemoveListener(OnInventoryButtonClicked);
        return UniTask.CompletedTask;
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

    private void OnSellAllButtonClicked()
    {
        throw new NotImplementedException();
    }
}
