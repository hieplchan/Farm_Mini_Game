using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public partial class FarmGamePresenterTests
{
    #region Given
    private void GivenAFarmGame()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view);
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
    }
    #endregion

    #region When
    private void WhenBuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    private void WhenBuyRandomCommoditySeed()
    {
        _presenter.BuyCommoditySeed(
            (CommodityType)_rand.Next(1, _commodityTypeCount));
    }

    private void WhenPlantRandomCommodity()
    {
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount);
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
    #endregion

    #region Then
    private void ThenShowsUpdatedGold()
    {
        _view.Received(1).UpdatedGold(_presenter.Gold);
    }

    private void ThenShowsUpdatedPlotList()
    {
        _view.Received().ShowUpdatedPlots(Arg.Is<List<FarmPlot>>(
            value => _presenter.Farm.plotList.ToList().SequenceEqual(value)));
    }

    private void ThenShowUpdatedInventorySeed()
    {
        _view.Received(1).ShowUpdatedInventorySeed(Arg.Is <List<int>>(
            value => _presenter.Inventory.seedList.ToList().SequenceEqual(value)));
    }

    #endregion
}
