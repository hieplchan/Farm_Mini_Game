using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public partial class FarmGamePresenterTests
{
    private FarmGameView _view;
    private FarmGamePresenter _presenter;
    private Random _rand;
    private int _commodityTypeCount;

    #region Buy Farm Plot
    [Test]
    public void WhenBuyFarmPlotGoldDecreaseCorrectAmount()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        int quantity = _rand.Next(0, 100);
        WhenBuyFarmPlot(quantity);

        Assert.IsTrue(_presenter.Farm.Gold.Equals(initGold -
            quantity * ConfigManager.GetStoreFarmPlotPrice()));
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedGold()
    {
        GivenAFarmGameWithMaxGold();

        WhenBuyFarmPlot();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedPlotList()
    {
        GivenAFarmGameWithMaxGold();

        WhenBuyFarmPlot();

        ThenShowsUpdatedPlotList();
    }

    [Test]
    public void WhenBuyFarmPlotPlotListCountIncrease()
    {
        GivenAFarmGameWithMaxGold();
        int plotCount = _presenter.Farm.Plots.Count;

        WhenBuyFarmPlot();

        Assert.Less(plotCount, _presenter.Farm.Plots.Count);
    }
    #endregion

    #region Buy Commodity Seed
    [Test]
    public void WhenBuyCommoditySeedSuccessShowUpdatedInventorySeeds()
    {
        GivenAFarmGameWithMaxGold();

        WhenBuyRandomCommoditySeed();

        ThenShowUpdatedInventorySeeds();
    }

    [Test]
    public void WhenBuyCommoditySeedGoldDecreaseCorrectAmount()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        int quantity = _rand.Next(0, 100);
        CommodityType type = WhenBuyRandomCommoditySeed(quantity);

        Assert.IsTrue(_presenter.Farm.Gold.Equals(initGold -
            quantity * ConfigManager.GetStoreSeedPrice(type)));
    }

    [Test]
    public void WhenBuyCommoditySeedSuccessShowsUpdatedGold()
    {
        GivenAFarmGameWithMaxGold();

        WhenBuyRandomCommoditySeed();

        ThenShowsUpdatedGold();
    }
    #endregion

    #region Plant Commodity
    [Test]
    public void WhenPlantSuccessFreePlotDecrease()
    {
        GivenAFarmGameInventoryHaveSeed();

        // Buy some plot
        for (int i = 0; i < _rand.Next(1, 10); i++)
        {
            WhenBuyFarmPlot();
        }
        int freePlotCount = _presenter.Farm.CountFreePlot();
        WhenPlantRandomCommodity();

        Assert.IsTrue(_presenter.Farm.CountFreePlot().Equals(freePlotCount - 1));
    }

    [Test]
    public void WhenPlantSuccessShowsUpdatedPlotList()
    {
        GivenAFarmGameWithMaxGold();

        // Buy some plot
        for (int i = 0; i < _rand.Next(1, 10); i++)
        {
            WhenBuyFarmPlot();
        }
        WhenPlantRandomCommodity();

        ThenShowsUpdatedPlotList();
    }
    #endregion

    #region Collect Commodity Product
    [Test]
    public void WhenCollectProductShowUpdatedInvetoryProducts()
    {
        GivenAFarmGameInventoryHaveSeed();

        WhenPlantRandomCommodityWaitForProduct();

        ThenShowUpdatedInventoryProducts();
    }

    #endregion

    #region Sell Commodity Product
    [Test]
    public void WhenSellProductShowUpdatedInvetoryProducts()
    {
        GivenAFarmGameInventoryHaveProduct();

        WhenSellRandomProduct();

        ThenShowUpdatedInventoryProducts();
    }

    [Test]
    public void WhenSellProductShowUpdatedGold()
    {
        GivenAFarmGameInventoryHaveProduct();

        WhenSellRandomProduct();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenSellProductGoldIncreaseCorrectAmount()
    {
        GivenAFarmGameInventoryHaveProduct();
        int initGold = _presenter.Farm.Gold;

        CommodityProductType type =
            (CommodityProductType)_rand.Next(1, _commodityTypeCount - 1);
        int quantity = _presenter.Inventory.Products[(int)type];
        _presenter.SellCommodityProduct((int)type);

        Assert.IsTrue(_presenter.Farm.Gold == 
            initGold + quantity * ConfigManager.GetStoreProductPrice(type));
    }
    #endregion

    #region Hire Worker
    [Test]
    public void WhenHireWorkerGoldDecreaseCorrectAmount()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        int quantity = _rand.Next(0, 100);
        for (int i = 0; i < quantity; i++)
        {
            WhenHireWorker();
        }

        Assert.IsTrue(_presenter.Farm.Gold.Equals(initGold -
            quantity * ConfigManager.GetStoreHireWorkerPrice()));
    }

    [Test]
    public void WhenHireWorkerShowsUpdatedGold()
    {
        GivenAFarmGameWithMaxGold();

        WhenHireWorker();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenHireWorkerWorkerListCountIncrease()
    {
        GivenAFarmGameWithMaxGold();
        int count = _presenter.Farm.Workers.Count;

        int quantity = _rand.Next(0, 100);
        for (int i = 0; i < quantity; i++)
        {
            WhenHireWorker();
        }

        Assert.IsTrue(_presenter.Farm.Workers.Count.Equals(count + quantity));
    }
    #endregion

    #region Upgrade Equipment
    [Test]
    public void WhenUpgradeEquipmentGoldDecreaseCorrectAmount()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        int upgradeCount = _rand.Next(1, 100);
        for (int i = 0; i < upgradeCount; i++)
            UpgradeEquipment();

        Assert.IsTrue(_presenter.Farm.Gold.Equals(initGold -
            upgradeCount * ConfigManager.GetStoreEquipUpgradePrice()));
    }

    [Test]
    public void WhenUpgradeEquipmentShowsUpdatedGold()
    {
        GivenAFarmGameWithMaxGold();

        UpgradeEquipment();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenUpgradeEquipmentShowsUpdatedEquipmentLevel()
    {
        GivenAFarmGameWithMaxGold();

        UpgradeEquipment();

        ThenShowsUpdatedEquipmentLevel();
    }

    [Test]
    public void WhenUpgradeEquipmentPlotProductivityIncreaseCorrectAmount()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        int upgradeCount = _rand.Next(1, 100);
        for (int i = 0; i < upgradeCount; i++)
            UpgradeEquipment();

        float correctProductivity = 1.0f +
            upgradeCount * ConfigManager.productivityIncreasePerEquipLv / 100f;
        foreach (FarmPlot plot in _presenter.Farm.Plots)
            if (plot.Productivity != correctProductivity)
                Assert.IsTrue(false);
        Assert.IsTrue(true);
    }
    #endregion

    #region Achievement

    [Test]
    public void WhenHaveHalfTargetGoldShowAchievement()
    {
        GivenAFarmGameZeroGold();

        _presenter.Farm.Gold += ConfigManager.targetGold / 2 + 1;

        ThenShowUpdatedLog(_presenter.Achievement.halfTargetMessage);
    }

    [Test]
    public void WhenHaveEnoughTargetGoldShowFinishAchievement()
    {
        GivenAFarmGameZeroGold();

        _presenter.Farm.Gold += ConfigManager.targetGold;

        ThenShowUpdatedLog(_presenter.Achievement.targetDoneMessage);
    }
    #endregion

    #region Others
    [Test]
    public void WhenLoggerLogShowUpdatedLog()
    {
        GivenAFarmGameWithMaxGold();

        string randomString = _rand.Next(100000, 1000000).ToString();
        Logger.Instance.Log(randomString);

        ThenShowUpdatedLog(randomString);
    }
    #endregion
}
