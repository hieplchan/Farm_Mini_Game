using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class StoreTests
{
    private Store _store;
    private Random _rand;
    private int _commodityTypeCount;

    [Test]
    public void WhenNoGoldCanNotBuyFarmPlot()
    {
        GivenStore();

        bool isSuccess = _store.BuyFarmPlot(
            _rand.Next(10, 100), 0, out int neededGold);

        Assert.IsFalse(isSuccess);
    }

    [Test]
    public void WhenNoGoldBuyFarmPlotNeededGoldIsZero()
    {
        GivenStore();

        _store.BuyFarmPlot(_rand.Next(10, 100), 0, out int neededGold);

        Assert.IsTrue(neededGold.Equals(0));
    }

    [Test]
    public void WhenFullGoldCanBuyFarmPlot()
    {
        GivenStore();

        bool isSuccess = _store.BuyFarmPlot(
            _rand.Next(10, 100), int.MaxValue, out int neededGold);

        Assert.IsTrue(isSuccess);
    }

    [Test]
    public void WhenFullGoldBuyFarmPlotCorrectNeededGold()
    {
        GivenStore();

        int quantity = _rand.Next(10, 100);
        _store.BuyFarmPlot(quantity, int.MaxValue, out int neededGold);

        Assert.IsTrue(neededGold.Equals(
            ConfigManager.GetStoreFarmPlotPrice() * quantity));
    }

    [Test]
    public void WhenNoGoldCanNotUpgradeEquipment()
    {
        GivenStore();

        bool isSuccess = _store.UpgradeEquipment(
            _rand.Next(10, 100), 0, out int neededGold);

        Assert.IsFalse(isSuccess);
    }

    [Test]
    public void WhenNoGoldUpgradeEquipmentGoldIsZero()
    {
        GivenStore();

        _store.UpgradeEquipment(_rand.Next(10, 100), 0, out int neededGold);

        Assert.IsTrue(neededGold.Equals(0));
    }

    [Test]
    public void WhenFullGoldCanUpgradeEquipment()
    {
        GivenStore();

        bool isSuccess = _store.UpgradeEquipment(
            _rand.Next(10, 100), int.MaxValue, out int neededGold);

        Assert.IsTrue(isSuccess);
    }

    [Test]
    public void WhenFullGoldUpgradeEquipmentCorrectNeededGold()
    {
        GivenStore();

        int quantity = _rand.Next(10, 100);
        _store.UpgradeEquipment(quantity, int.MaxValue, out int neededGold);

        Assert.IsTrue(neededGold.Equals(
            ConfigManager.GetStoreEquipUpgradePrice() * quantity));
    }

    [Test]
    public void WhenNoGoldCanNotBuyCommoditySeed()
    {
        GivenStore();

        bool isSuccess = _store.BuyCommoditySeed(
            (CommodityType)_rand.Next(1, _commodityTypeCount - 1),
            _rand.Next(10, 100), 0, out int neededGold);

        Assert.IsFalse(isSuccess);
    }

    [Test]
    public void WhenNoGoldBuyCommoditySeedNeededGoldIsZero()
    {
        GivenStore();

        _store.BuyCommoditySeed(
            (CommodityType)_rand.Next(1, _commodityTypeCount - 1),
            _rand.Next(10, 100), 0, out int neededGold);

        Assert.IsTrue(neededGold.Equals(0));
    }

    [Test]
    public void WhenFullGoldCanBuyCommoditySeed()
    {
        GivenStore();

        bool isSuccess = _store.BuyCommoditySeed(
            (CommodityType)_rand.Next(1, _commodityTypeCount - 1),
            _rand.Next(10, 100), int.MaxValue, out int neededGold);

        Assert.IsTrue(isSuccess);
    }

    [Test]
    public void WhenFullGoldBuyCommoditySeedCorrectNeededGold()
    {
        GivenStore();

        int quantity = _rand.Next(10, 100);
        CommodityType type = 
            (CommodityType)_rand.Next(1, _commodityTypeCount - 1);
        bool isSuccess = _store.BuyCommoditySeed(
            type, quantity, int.MaxValue, out int neededGold);

        Assert.IsTrue(neededGold.Equals(
            ConfigManager.GetStoreSeedPrice(type) * quantity));
    }

    [Test]
    public void WhenSellCommodityProductReceiveCorrectGold()
    {
        GivenStore();

        int quantity = _rand.Next(10, 100);
        CommodityProductType type =
            (CommodityProductType)_rand.Next(1, _commodityTypeCount - 1);
        int gold = _store.SellCommodityProduct(type, quantity);

        Assert.IsTrue(gold.Equals(
            ConfigManager.GetStoreProductPrice(type) * quantity));
    }

    private void GivenStore()
    {
        ConfigManager.Reload();
        _store = new Store();
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
    }
}