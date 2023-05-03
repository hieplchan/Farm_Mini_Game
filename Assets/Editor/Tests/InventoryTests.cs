using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class InventoryTests
{
    private Inventory _inventory;
    private Random _rand;
    private int _commodityTypeCount;

    [Test]
    public void WhenAddSeedQuantityIncrease()
    {
        GivenHaveSeedInventory();
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
        int initQuantity = _inventory.Seeds[(int)type];

        int addedQuantity = _rand.Next(1, 1000);
        _inventory.AddSeed(type, addedQuantity);

        Assert.IsTrue(_inventory.Seeds[(int)type].Equals(initQuantity + addedQuantity));
    }

    [Test]
    public void WhenGetSeedQuantityDecrease()
    {
        GivenHaveSeedInventory();
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
        int initQuantity = _inventory.Seeds[(int)type];

        _inventory.GetSeed(type);

        Assert.IsTrue(_inventory.Seeds[(int)type].Equals(initQuantity - 1));
    }

    [Test]
    public void WhenNoSeedGetSeedReturnNull()
    {
        GivenZeroSeedInventory();

        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
        Commodity seed = _inventory.GetSeed(type);

        Assert.IsTrue(seed == null);
    }

    private void GivenHaveSeedInventory()
    {
        _rand = new Random();
        _inventory = new Inventory();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        for (int i = 0; i < _commodityTypeCount; i++)
        {
            _inventory.Seeds[i] = _rand.Next(10, 100);
        }
    }

    private void GivenZeroSeedInventory()
    {
        _rand = new Random();
        _inventory = new Inventory();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        for (int i = 0; i < _commodityTypeCount; i++)
        {
            _inventory.Seeds[i] = 0;
        }
    }
}