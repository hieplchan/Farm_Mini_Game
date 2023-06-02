using SuperMaxim.Messaging;
using System;

public enum WorkerState
{
    Idle,
    Working,
}

public class Worker
{
    public string ID { get => _id; }
    private string _id;

    public WorkerState State
    {
        get => _state;
        private set
        {
            _state = value;
            NotifyWorkerChanged();
        }
    }

    private WorkerState _state;

    private FarmPlot _currentWorkingPlot;
    private float _currentWorkingTime;
    private int _timeNeededPerTask;

    public Worker()
    {
        _id = Guid.NewGuid().ToString();

        Logger.Instance.Log("Thanks for having me <3");
        State = WorkerState.Idle;
        _timeNeededPerTask = 
            ConfigManager.GetWorkerConfig().timeNeededPerTask.MinToSec();

        NotifyWorkerChanged();
    }

    public void GameUpdate(float deltaTime)
    {
        if (State == WorkerState.Working)
        {
            _currentWorkingTime += deltaTime;
            if (_currentWorkingTime >= _timeNeededPerTask)
            {
                FinishWork();
            }
        }
    }

    public bool SearchForJob(FarmGame farm, Inventory inventory)
    {
        if (State != WorkerState.Idle)
            return false;

        if (SearchForHarvestJob(farm, inventory))
            return true;

        if (SearchForPlantJob(farm, inventory))
            return true;

        return false; // no job founded
    }

    private bool SearchForHarvestJob(FarmGame farm, Inventory inventory)
    {
        foreach (FarmPlot plot in farm.Plots)
        {
            if (plot.HasCommodity)
                if (plot.Commodity.AvailableProduct > 0 && !plot.HasWorker)
                {
                    Harvest(plot, inventory);
                    StartWorking(plot);
                    return true;
                }
        }

        return false;
    }

    private bool SearchForPlantJob(FarmGame farm, Inventory inventory)
    {
        foreach (FarmPlot plot in farm.Plots)
        {
            if (!plot.HasCommodity && inventory.HasSeed)
            {
                Plant(plot, inventory);
                StartWorking(plot);
                return true;
            }
        }

        return false;
    }

    public void Harvest(FarmPlot plot, Inventory inventory)
    {
        int productCount = plot.Commodity.Harvest();
        CommodityProductType productType =
            (CommodityProductType)plot.Commodity.Type;
        inventory.AddProduct(productType, productCount);
        Logger.Instance.Log(string.Format(
            "I collect {0} {1}", productCount, productType.ToString()));
    }

    public void Plant(FarmPlot plot, Inventory inventory)
    {
        Commodity seed = inventory.GetAvailableSeed();
        plot.Plant(seed);
        Logger.Instance.Log(string.Format(
            "I plant {0}", seed.Type.ToString()));
    }

    private void StartWorking(FarmPlot plot)
    {
        State = WorkerState.Working;
        _currentWorkingPlot = plot;
        _currentWorkingPlot.AddWorker(this);
    }

    private void FinishWork()
    {
        _currentWorkingTime = 0f;
        State = WorkerState.Idle;

        _currentWorkingPlot.RemoveWorker();
        _currentWorkingPlot = null;
    }

    private void NotifyWorkerChanged()
    {
        var payload = new WorkerChangedPayLoad { ID = _id };
        Messenger.Default.Publish(payload);
    }
}