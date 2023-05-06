using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class CommodityTests
{
    private Commodity _commodity;
    private CommodityConfig _config;
    private Random _rand;
    private int _commodityTypeCount;

    [Test]
    public void WhenNewCommodityStateEqualSeed()
    {
        GivenRandomCommodity();

        CommodityStateEqual(CommodityState.Seed);
    }

    [Test]
    public void WhenPlantCommodityStateEqualMature()
    {
        GivenRandomCommodity();

        Plant();

        CommodityStateEqual(CommodityState.Mature);
    }

    [Test]
    public void WhenMatureStateEqualMature()
    {
        GivenRandomCommodity();

        int cycleCount = _rand.Next(1, _config.productCycleNum);
        WhenMature(cycleCount);

        CommodityStateEqual(CommodityState.Mature);
    }

    [Test]
    public void WhenHarvestAvailableProductEqualZero()
    {
        GivenRandomCommodity();

        int cycleCount = _rand.Next(1, _config.productCycleNum - 1);
        WhenMature(cycleCount);
        int harvestedProduct = _commodity.Harvest();

        Assert.IsTrue(_commodity.AvailableProduct.Equals(0));
    }

    [Test]
    public void WhenHarvestProductEqualCycleCount()
    {
        GivenRandomCommodity();

        int cycleCount = _rand.Next(1, _config.productCycleNum - 1);
        WhenMature(cycleCount);
        _commodity.GameUpdate(59); //offset 59 sec
        int harvestedProduct = _commodity.Harvest();

        MLog.Log("CommodityTests",
            "\n cycleCount: " + cycleCount +
            "\n harvestedProduct: " + harvestedProduct);

        Assert.IsTrue(harvestedProduct.Equals(cycleCount));
    }

    [Test]
    public void WhenLiveProductEqualCycleNum()
    {
        GivenRandomCommodity();

        WhenAlive();

        AvailableProductEqual(_config.productCycleNum);
    }

    [Test]
    public void AfterMatureStateEqualDying()
    {
        GivenRandomCommodity();

        AfterMature();

        CommodityStateEqual(CommodityState.Dying);
    }

    [Test]
    public void AfterDyingStateEqualDead()
    {
        GivenRandomCommodity();

        AfterDying();

        CommodityStateEqual(CommodityState.Dead);
    }


    private void CommodityStateEqual(CommodityState state)
    {
        MLog.Log("CommodityTests", "CommodityStateEqual: " + _commodity.State.ToString());
        Assert.IsTrue(_commodity.State.Equals(state));
    }

    private void AvailableProductEqual(int count)
    {
        Assert.IsTrue(_commodity.AvailableProduct.Equals(count));
    }

    private void GivenRandomCommodity()
    {
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
        _config = ConfigManager.GetCommodityConfig(type);
        _commodity = new Commodity(type);
    }

    private void Plant()
    {
        FarmPlot plot = new FarmPlot();
        _commodity.Plant(plot);
    }

    private void WhenMature(int cycleCount)
    {
        Plant();

        float matureTime =
            _config.productCycleTime.MinToSec() * cycleCount;

        int loopSecCount = (int)(matureTime);

        MLog.Log("CommodityTests",
            "\n matureTime: " + matureTime +
            "\n loopSecCount: " + loopSecCount);

        for (int i = 0; i < loopSecCount; i++)
        {
            _commodity.GameUpdate(1);
        }
    }

    private void AfterMature()
    {
        Plant();
        while (_commodity.State == CommodityState.Mature)
            _commodity.GameUpdate(1);
    }

    private void WhenAlive()
    {
        Plant();
        while (_commodity.State != CommodityState.Dead)
            _commodity.GameUpdate(1);
    }

    private void AfterDying()
    {
        Plant();
        while (_commodity.State == CommodityState.Mature ||
                _commodity.State == CommodityState.Dying)
            _commodity.GameUpdate(1);
    }
}