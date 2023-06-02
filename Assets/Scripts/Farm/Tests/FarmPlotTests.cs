using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;
using SuperMaxim.Messaging;

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

        WhenPlantRandomCommodity();

        Assert.IsTrue(_plot.HasCommodity);
    }

    [Test]
    public void WhenCommodityDeadRemoveCommodity()
    {
        GivenFarmPlot();

        WhenPlantRandomCommodity();

        float matureTime =
            _config.productCycleTime.MinToSec() * _config.productCycleNum;
        float lifeTime = matureTime +
            _config.dyingTime.MinToSec();

        lifeTime += 1.MinToSec(); // offset 1 min

        while (lifeTime > 0)
        {
            _plot.GameUpdate(1);
            lifeTime -= 1;
        }

        Assert.IsFalse(_plot.HasCommodity);
    }

    [Test]
    public void WhenUpgradeEquipmentPlotProductivityIncrease()
    {
        GivenFarmPlot();

        int equipmentLv = _rand.Next(10, 50);
        UpgradeEquipmentLevel(equipmentLv);

        float correctProductivity = 1.0f +
            equipmentLv * ConfigManager.GetProductivityEquipment() / 100f;
        Assert.IsTrue(_plot.Productivity.Equals(correctProductivity));
    }

    [Test]
    public void WhenUpgradeEquipmentCommodityProductivityIncrease()
    {
        GivenFarmPlot();

        int equipmentLv = _rand.Next(10, 50);
        float actualMatureDuration = 0;
        UpgradeEquipmentLevel(equipmentLv);
        CommodityType type = WhenPlantRandomCommodity();
        while (_plot.Commodity.State == CommodityState.Mature)
        {
            _plot.GameUpdate(1);
            actualMatureDuration += 1.0f;
        }

        float correctProductivity = 1.0f +
            equipmentLv * ConfigManager.GetProductivityEquipment() / 100f;
        CommodityConfig config = ConfigManager.GetCommodityConfig(type);
        float correctMatureDuration =
            config.productCycleTime.MinToSec() * config.productCycleNum / correctProductivity;
        float diff = MathF.Abs(actualMatureDuration - correctMatureDuration);
        float originMatureDuration = config.productCycleTime.MinToSec() * config.productCycleNum;

        MLog.Log("FarmPlotTests", string.Format(
            "WhenUpgradeEquipmentPlantProductivityIncrease : \n" +
            "correctMatureDuration: {0} \n" +
            "actualDuration: {1}\n" +
            "abs: {2}\n" +
            "originMatureDuration: {3}\n" +
            "equipmentLv: {4}",
            correctMatureDuration, actualMatureDuration, diff, originMatureDuration, equipmentLv));
        
        // Offset 5 sec
        Assert.Less(diff, 5);
    }

    private void GivenFarmPlot()
    {
        _plot = new FarmPlot();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        _rand = new Random();
    }

    private CommodityType WhenPlantRandomCommodity()
    {
        _rand = new Random();
        CommodityType type = (CommodityType)_rand.Next(1, _commodityTypeCount - 1);

        _commodity = new Commodity(type);
        _config = ConfigManager.GetCommodityConfig(type);

        _plot.Plant(_commodity);
        return type;
    }

    private void UpgradeEquipmentLevel(int equipmentLv)
    {
        var payload = new EquipmentLevelChangedPayLoad { EquipmentLevel = equipmentLv };
        Messenger.Default.Publish(payload);
    }
}