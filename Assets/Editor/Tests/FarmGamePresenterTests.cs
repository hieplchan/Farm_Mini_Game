using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class FarmGamePresenterTests
{
    private FarmGameView _view;
    private FarmGamePresenter _presenter;
    private Random _rand;

    #region Buy Farm Plot
    [Test]
    public void WhenBuyFarmPlotGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Gold;

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
        int plotCount = _presenter.Farm.plotList.Count;

        WhenBuyFarmPlot();

        Assert.Less(plotCount, _presenter.Farm.plotList.Count);
    }
    #endregion

    #region Plant Commodity
    [Test]
    public void WhenPlantSuccessFreePlotDecrease()
    {
        GivenAFarmGame();

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

    #region Hire Worker
    [Test]
    public void WhenHireWorkerGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Gold;

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

    #region Common
    private void GivenAFarmGame()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view);
        _rand = new Random();
    }

    private void WhenBuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    private void WhenPlantRandomCommodity()
    {
        CommodityType type = (CommodityType)_rand.Next(0,
            Enum.GetNames(typeof(CommodityType)).Length);
        _presenter.PlantCommodity(type);
    }

    private void WhenHireWorker()
    {
        _presenter.HireWorker();
    }

    private void ThenGoldDecrease(int initGold)
    {
        Assert.Less(_presenter.Gold, initGold);
    }

    private void ThenShowsUpdatedGold()
    {
        _view.Received(1).UpdatedGold(_presenter.Gold);
    }

    private void ThenShowsUpdatedPlotList()
    {
        _view.Received().UpdatedPlots(Arg.Is<List<FarmPlot>>(
            value => _presenter.Farm.plotList.ToList().SequenceEqual(value)));
    }
    #endregion
}
