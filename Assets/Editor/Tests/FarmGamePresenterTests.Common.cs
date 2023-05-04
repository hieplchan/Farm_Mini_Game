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

    private void GivenAFarmGameInventoryHaveSeed()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view);
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        Array.Fill(_presenter.Inventory.Seeds, _rand.Next(10, 100));
    }
    #endregion

    #region When
    private void WhenBuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    private void WhenBuyRandomCommoditySeed()
    {
        _presenter.BuyCommoditySeed(_rand.Next(0, _commodityTypeCount - 1));
    }

    private void WhenPlantRandomCommodity()
    {
        _presenter.PlantCommodity(_rand.Next(0, _commodityTypeCount));
    }

    private void WhenPlantRandomCommodityWaitForProduct()
    {
        _presenter.BuyFarmPlot();
        int type = _rand.Next(0, _commodityTypeCount);
        _presenter.PlantCommodity(type);
        // Wait commodity produce product
        while (_presenter.Farm.Plots[0].Commodity.AvailableProduct == 0)
            _presenter.GameUpdate(1);
        _presenter.CollectCommodityProduct(type);
    }

    private void WhenHireWorker()
    {
        _presenter.HireWorker();
    }

    private void ThenGoldDecrease(int initGold)
    {
        Assert.Less(_presenter.Farm.Gold, initGold);
    }
    #endregion

    #region Then
    private void ThenShowsUpdatedGold()
    {
        _view.Received(1).ShowUpdatedGoldAndEquipLevel(_presenter.Farm.Gold, 1);
    }

    private void ThenShowsUpdatedPlotList()
    {
        _view.Received().ShowUpdatedPlots(Arg.Is<List<FarmPlot>>(
            value => _presenter.Farm.Plots.SequenceEqual(value)));
    }

    private void ThenShowUpdatedInventorySeeds()
    {
        _view.Received().ShowUpdatedInventorySeeds(Arg.Is<int[]>(
            value => _presenter.Inventory.Seeds.SequenceEqual(value)));
    }

    private void ThenShowUpdatedInventoryProducts()
    {
        _view.Received().ShowUpdatedInventoryProducts(Arg.Is<int[]>(
            value => _presenter.Inventory.Products.SequenceEqual(value)));
    }
    #endregion
}
