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
    public void WhenBuyFarmPlotGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Farm.Gold;

        WhenBuyFarmPlot();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedGold()
    {
        GivenAFarmGame();

        WhenBuyFarmPlot();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedPlotList()
    {
        GivenAFarmGame();

        WhenBuyFarmPlot();

        ThenShowsUpdatedPlotList();
    }

    [Test]
    public void WhenBuyFarmPlotPlotListCountIncrease()
    {
        GivenAFarmGame();
        int plotCount = _presenter.Farm.Plots.Count;

        WhenBuyFarmPlot();

        Assert.Less(plotCount, _presenter.Farm.Plots.Count);
    }
    #endregion

    #region Buy Commodity Seed
    [Test]
    public void WhenBuyCommoditySeedSuccessShowUpdatedInventorySeeds()
    {
        GivenAFarmGame();

        WhenBuyRandomCommoditySeed();

        ThenShowUpdatedInventorySeeds();
    }

    [Test]
    public void WhenBuyCommoditySeedGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Farm.Gold;

        WhenBuyRandomCommoditySeed();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenBuyCommoditySeedSuccessShowsUpdatedGold()
    {
        GivenAFarmGame();

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
        GivenAFarmGame();

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

    #region Hire Worker
    [Test]
    public void WhenHireWorkerGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Farm.Gold;

        WhenHireWorker();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenHireWorkerShowsUpdatedGold()
    {
        GivenAFarmGame();

        WhenHireWorker();

        ThenShowsUpdatedGold();
    }
    #endregion
}
