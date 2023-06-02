using Cysharp.Threading.Tasks;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Screen;
using UnityScreenNavigator.Runtime.Core.Shared;

using Screen = UnityScreenNavigator.Runtime.Core.Screen.Screen;

public class ShopModal : Modal
{
    [SerializeField] private Button _buyPlot, _hireWorker, _upgradeEquipMent;
    [SerializeField] private Button _strawberryButton, _tomatoButton, _blueberryButton, _cowButton;

    [SerializeField] private TMP_Text _buyPlotPrice, _hireWorkerPrice, _upgradeEquipMentPrice;
    [SerializeField] private TMP_Text _strawberryPrice, _tomatoPrice, _blueberryPrice, _cowPrice;

    [SerializeField] private TMP_Text _totalGold;

    private FarmGamePresenter _farmGamePresenter;

    internal void Setup(FarmGamePresenter farmGamePresenter)
    {
        _farmGamePresenter = farmGamePresenter;

        UIBinding();

        Messenger.Default.Subscribe<GoldChangedPayLoad>(OnGoldChanged);
    }

    private void UIBinding()
    {
        _buyPlot.onClick.AddListener(() => _farmGamePresenter.BuyFarmPlot());
        _hireWorker.onClick.AddListener(() => _farmGamePresenter.HireWorker());
        _upgradeEquipMent.onClick.AddListener(() => _farmGamePresenter.UpgradeEquipment());

        _strawberryButton.onClick.AddListener(() => _farmGamePresenter.BuyCommoditySeed((int)CommodityType.Strawberry));
        _tomatoButton.onClick.AddListener(() => _farmGamePresenter.BuyCommoditySeed((int)CommodityType.Tomato));
        _blueberryButton.onClick.AddListener(() => _farmGamePresenter.BuyCommoditySeed((int)CommodityType.Blueberry));
        _cowButton.onClick.AddListener(() => _farmGamePresenter.BuyCommoditySeed((int)CommodityType.Cow));

        _buyPlotPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.farmPlotPrice.ToString();
        _hireWorkerPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.hireWorkerPrice.ToString();
        _upgradeEquipMentPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.equipUpgradePrice.ToString();

        _strawberryPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.seedPrices[(int)CommodityType.Strawberry].ToString();
        _tomatoPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.seedPrices[(int)CommodityType.Tomato].ToString();
        _blueberryPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.seedPrices[(int)CommodityType.Blueberry].ToString();
        _cowPrice.text = _farmGamePresenter.FarmGameConfig.storeConfig.seedPrices[(int)CommodityType.Cow].ToString();

        _totalGold.text = _farmGamePresenter.Farm.Gold.ToString();
    }

    public override async UniTask Cleanup()
    {
        _buyPlot.onClick.RemoveAllListeners();
        _hireWorker.onClick.RemoveAllListeners();
        _upgradeEquipMent.onClick.RemoveAllListeners();

        Messenger.Default.Unsubscribe<GoldChangedPayLoad>(OnGoldChanged);

        await UniTask.CompletedTask;
    }

    private void OnGoldChanged(GoldChangedPayLoad obj)
    {
        _totalGold.text = obj.TotalGold.ToString();
    }
}
