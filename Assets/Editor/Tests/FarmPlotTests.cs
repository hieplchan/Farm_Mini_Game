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
    private int _commodityTypeCount;

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
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
    }


    private void PlantRandomCommodity()
    {
        _rand = new Random();
        CommodityType type = (CommodityType)_rand.Next(1, _commodityTypeCount - 1);

        _commodity = new Commodity(type);
        _config = ConfigManager.GetCommodityConfig(type);

        _plot.Plant(_commodity);
    }
}