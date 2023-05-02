using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class FarmPlotTests
{
    private FarmPlot _plot;
    private Commodity _commodity;
    private CommodityConfig _config;
    private Random _rand;

    [Test]
    public void WhenNewPlotNotHaveCommodity()
    {
        GivenFarmPlot();

        Assert.IsFalse(_plot.HasCommodity);
    }

    [Test]
    public void WhenPlantHaveCommodity()
    {
        GivenFarmPlot();

        PlantRandomCommodity();

        Assert.IsTrue(_plot.HasCommodity);
    }

    [Test]
    public void WhenCommodityDeadRemoveCommodity()
    {
        GivenFarmPlot();

        PlantRandomCommodity();

        float matureTime =
            _config.productCycleTime.MinToSec() * _config.productCycleNum /
            (_config.productivity / 100f);
        float lifeTime = matureTime + 
            _config.dyingTime.MinToSec() / (_config.productivity / 100f);

        lifeTime += 1.MinToSec(); // offset 1 min

        while (lifeTime > 0)
        {
            _plot.GameUpdate(1);
            lifeTime -= 1;
        }

        Assert.IsFalse(_plot.HasCommodity);
    }

    private void GivenFarmPlot()
    {
        _plot = new FarmPlot();
    }


    private void PlantRandomCommodity()
    {
        _rand = new Random();

        _config = new CommodityConfig(
            productCycleTime: _rand.Next(1, 100),
            productCycleNum: _rand.Next(1, 100),
            dyingTime: _rand.Next(1, 100),
            productivity: 100 + _rand.Next(1, 100) * 10);

        MLog.Log("CommodityTests _config",
            "\n productCycleTime: " + _config.productCycleTime +
            "\n productCycleNum: " + _config.productCycleNum +
            "\n dyingTime: " + _config.dyingTime +
            "\n productivity: " + _config.productivity);

        _commodity = new Commodity(_config,
            (CommodityType)_rand.Next(0, Enum.GetNames(typeof(CommodityType)).Length));

        _plot.Plant(_commodity);
    }
}