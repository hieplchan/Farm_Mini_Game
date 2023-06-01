using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Shared;

public class SeedMenuModal : Modal
{
    [SerializeField]
    private Button _strawberryButton, _tomatoButton, _blueberryButton, _cowButton;

    FarmGamePresenter _farmGamePresenter;

    internal void Setup(FarmGamePresenter farmGamePresenter)
    {
        _farmGamePresenter = farmGamePresenter;
        _strawberryButton.onClick.AddListener(() => _farmGamePresenter.PlantCommodity((int)CommodityType.Strawberry));
        _tomatoButton.onClick.AddListener(() => _farmGamePresenter.PlantCommodity((int)CommodityType.Tomato));
        _blueberryButton.onClick.AddListener(() => _farmGamePresenter.PlantCommodity((int)CommodityType.Blueberry));
        _cowButton.onClick.AddListener(() => _farmGamePresenter.PlantCommodity((int)CommodityType.Cow));
    }

    public override async UniTask Cleanup()
    {
        _strawberryButton.onClick.RemoveAllListeners();
        _tomatoButton.onClick.RemoveAllListeners();
        _blueberryButton.onClick.RemoveAllListeners();
        _cowButton.onClick.RemoveAllListeners();
        await UniTask.CompletedTask;
    }
}
