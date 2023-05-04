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
    public void WhenHireWorkerGoldDecrease()
    {
        GivenAFarmGameWithMaxGold();
        int initGold = _presenter.Farm.Gold;

        WhenHireWorker();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenHireWorkerShowsUpdatedGold()
    {
        GivenAFarmGameWithMaxGold();

        WhenHireWorker();

        ThenShowsUpdatedGold();
    }
    #endregion
}
