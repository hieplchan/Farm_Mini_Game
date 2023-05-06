using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System;

[TestFixture]
public class WorkerTest
{
    private Commodity _commodity;
    private Random _rand;
    private int _commodityTypeCount;
    private Worker _worker;
    private FarmGame _farm;
    private Inventory _inventory;
    CommodityType _commodityType;

    [Test]
    public void WhenHaveAvailableProductIdleWorkerSeekForJobTrue()
    {
        GivenIdleWorker();

        WhenHaveAvailableProduct();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsTrue(seekJobResult);
    }

    [Test]
    public void WhenNotAvailableProductIdleWorkerSeekForJobFalse()
    {
        GivenIdleWorker();

        WhenNotAvailableProduct();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsFalse(seekJobResult);
    }

    [Test]
    public void WhenPlotFreeAndHaveSeedIdleWorkerSeekForJobTrue()
    {
        GivenIdleWorker();

        WhenPlotFreeAndHaveSeed();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsTrue(seekJobResult);
    }

    [Test]
    public void WhenNotHavePlotFreeAndHaveSeedIdleWorkerSeekForJobFalse()
    {
        GivenIdleWorker();

        WhenNotHavePlotFreeAndHaveSeed();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsFalse(seekJobResult);
    }

    [Test]
    public void WhenHavePlotFreeAndNotHaveSeedIdleWorkerSeekForJobFalse()
    {
        GivenIdleWorker();

        WhenHavePlotFreeAndNotHaveSeed();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsFalse(seekJobResult);
    }

    [Test]
    public void WhenHaveAvailableProductIdleWorkerSeekForJobStateChangeToWorking()
    {
        GivenIdleWorker();

        WhenHaveAvailableProduct();
        _worker.SearchForJob(_farm, _inventory);

        Assert.IsTrue(_worker.State.Equals(WorkerState.Working));
    }

    [Test]
    public void WhenPlotFreeAndHaveSeedIdleWorkerSeekForJobStateChangeToWorking()
    {
        GivenIdleWorker();

        WhenPlotFreeAndHaveSeed();
        _worker.SearchForJob(_farm, _inventory);

        Assert.IsTrue(_worker.State.Equals(WorkerState.Working));
    }

    [Test]
    public void WhenHaveAvailableProductWorkingWorkerSeekForJobFalse()
    {
        GivenWorkingWorker();

        WhenHaveAvailableProduct();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsFalse(seekJobResult);
    }

    [Test]
    public void WhenPlotFreeAndHaveSeedWorkingWorkerSeekForJobFalse()
    {
        GivenWorkingWorker();

        WhenPlotFreeAndHaveSeed();
        bool seekJobResult = _worker.SearchForJob(_farm, _inventory);

        Assert.IsFalse(seekJobResult);
    }

    public void WhenHarvestWorkerStateChangeToIdleAfterWorkingDuration()
    {
        GivenIdleWorker();
    }

    public void WhenPlantWorkerStateChangeToIdleAfterWorkingDuration()
    {
        GivenIdleWorker();
    }

    private void GivenIdleWorker()
    {
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);

        _worker = new Worker();
        _farm = new FarmGame();
        _inventory = new Inventory();
    }

    private void GivenWorkingWorker()
    {
        _rand = new Random();
        _commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        CommodityType type = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);

        _worker = new Worker();
        _farm = new FarmGame();
        _inventory = new Inventory();

        _farm.AddPlot();
        _inventory.AddSeed(_commodityType);
        _worker.SearchForJob(_farm, _inventory);
    }

    private void WhenHaveAvailableProduct()
    {
        FarmPlot plot = _farm.AddPlot();
        plot.Plant(new Commodity(_commodityType));
        while (plot.Commodity.State == CommodityState.Mature)
            plot.Commodity.GameUpdate(1);
    }

    private void WhenNotAvailableProduct()
    {
        _farm.AddPlot();
        _farm.GetFreePlot().Plant(new Commodity(_commodityType));
        _farm.Plots[0].Commodity.GameUpdate(1);
    }

    private void WhenPlotFreeAndHaveSeed()
    {
        _farm.AddPlot();
        _inventory.AddSeed(_commodityType);
    }
    
    private void WhenNotHavePlotFreeAndHaveSeed()
    {
        _inventory.AddSeed(_commodityType);
    }

    private void WhenHavePlotFreeAndNotHaveSeed()
    {
        _farm.AddPlot();
    }

    private void ShuffleCommodityType()
    {
        _commodityType = (CommodityType)_rand.Next(0, _commodityTypeCount - 1);
    }
}