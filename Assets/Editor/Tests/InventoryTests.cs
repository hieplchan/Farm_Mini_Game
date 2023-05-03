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
        GivenRandomInventory();
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
        int initQuantity = _inventory.seedList[(int)type];

        int addedQuantity = _rand.Next(1, 1000);
        _inventory.AddSeed(type, addedQuantity);

        Assert.IsTrue(_inventory.seedList[(int)type].Equals(initQuantity + addedQuantity));
    }

    private void GivenRandomInventory()
    {
        _rand = new Random();
        _inventory = new Inventory();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        for (int i = 0; i < _commodityTypeCount; i++)
        {
            _inventory.seedList[i] = _rand.Next(1, 100);
        }
    }
}