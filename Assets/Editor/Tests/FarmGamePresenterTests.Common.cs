using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public partial class FarmGamePresenterTests
{
    #region Given
    private void GivenAFarmGameWithMaxGold()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view, ConfigManager.GetDefaultConfig());
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        _presenter.Farm.Gold = int.MaxValue;
    }
    private void GivenAFarmGameZeroGold()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view, ConfigManager.GetDefaultConfig());
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
    }

    private void GivenAFarmGameInventoryHaveSeed()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view, ConfigManager.GetDefaultConfig());
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        Array.Fill(_presenter.Farm.Inventory.Seeds, _rand.Next(10, 100));
    }

    private void GivenAFarmGameInventoryHaveProduct()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view, ConfigManager.GetDefaultConfig());
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        Array.Fill(_presenter.Farm.Inventory.Products, _rand.Next(10, 100));
    }
    #endregion

    #region When
    private CommodityType WhenBuyRandomCommoditySeed(int quantity = 1)
    {
        int type = _rand.Next(0, _commodityTypeCount - 1);
        for (int i = 0; i < quantity; i++)
            _presenter.BuyCommoditySeed(type);
        return (CommodityType)type;
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

    private void WhenSellRandomProduct()
    {
        int quantity = _rand.Next(10, 100);
        CommodityProductType type =
            (CommodityProductType)_rand.Next(1, _commodityTypeCount - 1);
        int gold = _presenter.Farm.Store.SellCommodityProduct(type, quantity);
    }

    private void WhenBuyFarmPlot(int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
            _presenter.BuyFarmPlot();
    }

    private void WhenHireWorker()
    {
        _presenter.HireWorker();
    }

    private void UpgradeEquipment(int quantity = 1)
    {
        _presenter.UpgradeEquipment();
    }
    #endregion

    #region Then
    private void ThenGoldDecrease(int initGold)
    {
        Assert.Less(_presenter.Farm.Gold, initGold);
    }

    private void ThenShowsUpdatedGold()
    {
        _view.Received().ShowUpdatedGoldAndEquipLevel(
            _presenter.Farm.Gold, _presenter.Farm.EquipLv);
    }

    private void ThenShowsUpdatedEquipmentLevel()
    {
        _view.Received(1).ShowUpdatedGoldAndEquipLevel(
            _presenter.Farm.Gold, _presenter.Farm.EquipLv);
    }

    private void ThenShowsUpdatedPlotList()
    {
        _view.Received().ShowUpdatedPlots(Arg.Is<List<FarmPlot>>(
            value => _presenter.Farm.Plots.SequenceEqual(value)));
    }

    private void ThenShowUpdatedInventorySeeds()
    {
        _view.Received().ShowUpdatedInventorySeeds(Arg.Is<int[]>(
            value => _presenter.Farm.Inventory.Seeds.SequenceEqual(value)));
    }

    private void ThenShowUpdatedInventoryProducts()
    {
        _view.Received().ShowUpdatedInventoryProducts(Arg.Is<int[]>(
            value => _presenter.Farm.Inventory.Products.SequenceEqual(value)));
    }

    private void ThenShowUpdatedLog(string randomString)
    {
        _view.Received(1).ShowUpdatedLog(randomString);
    }
    #endregion
}
