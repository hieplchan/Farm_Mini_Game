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

        WhenMature();

        CommodityStateEqual(CommodityState.Mature);
    }

    [Test]
    public void AfterMatureProductEqualCycleNum()
    {
        GivenRandomCommodity();

        AfterMature();

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
        Assert.IsTrue(_commodity.State.Equals(state));
    }

    private void AvailableProductEqual(int count)
    {
        Assert.IsTrue(_commodity.AvailableProduct.Equals(count));
    }

    private void GivenRandomCommodity()
    {
        var rand = new Random();

        _config = new CommodityConfig(
            productCycleTime: rand.Next(1, 100),
            productCycleNum: rand.Next(1, 100),
            dyingTime: rand.Next(1, 100),
            productivity: rand.Next(100, 1000));

        _commodity = new Commodity(_config,
            (CommodityType)rand.Next(0, Enum.GetNames(typeof(CommodityType)).Length));
    }

    private void Plant()
    {
        FarmPlot plot = new FarmPlot();
        _commodity.Plant(plot);
    }

    private void WhenMature()
    {
        Plant();

        float matureTime =
            _config.productCycleTime.MinToSec() * _config.productCycleNum /
            (_config.productivity / 100f);

        int loopSecCount = (int)(matureTime - 59);

        for (int i = 0; i < loopSecCount; i++)
        {
            _commodity.GameUpdate(1);
        }
    }

    private void AfterMature()
    {
        Plant();

        float matureTime = 
            _config.productCycleTime.MinToSec() * _config.productCycleNum /
            (_config.productivity / 100f);

        int loopSecCount = (int)(matureTime + 59);

        for (int i = 0; i < loopSecCount; i++)
        {
            _commodity.GameUpdate(1);
        }
    }

    private void AfterDying()
    {
        Plant();

        float lifeTime =
            (_config.productCycleTime.MinToSec() * _config.productCycleNum +
            _config.dyingTime.MinToSec()) / (_config.productivity / 100f);

        int loopSecCount = (int)(lifeTime + 59);

        for (int i = 0; i < loopSecCount; i++)
        {
            _commodity.GameUpdate(1);
        }
    }
}