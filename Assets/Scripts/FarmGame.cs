using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class FarmGame : IPersistableObject
{
    const int saveFormatVersion = 1;

    public event Action<int> GoldChanged;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            NotifyGoldChanged();
        }
    }
    int _gold;

    public event Action<int> EquipLvChanged;
    public int EquipLv
    {
        get => _equipLv;
        set
        {
            _equipLv = value;
            NotifyEquipLvChanged();
        }
    }
    int _equipLv;

    public event Action FarmPlotChanged;
    public List<FarmPlot> Plots { get => _plots; }
    private List<FarmPlot> _plots;

    public event Action WorkerChanged;
    public List<Worker> Workers { get => _workers; }
    private List<Worker> _workers;

    public Inventory Inventory => _inventory;
    private Inventory _inventory;

    public Store Store => _store;
    private Store _store;

    public Achievement Achievement => _achievement;
    private Achievement _achievement;

    public FarmGame()
    {
        _plots = new List<FarmPlot>();
        _workers = new List<Worker>();
        _inventory = new Inventory();
        _store = new Store();
        _achievement = new Achievement();
    }

    public void UpgradeEquipLv()
    {
        EquipLv += 1;
    }

    public FarmPlot AddPlot()
    {
        FarmPlot plot = new FarmPlot();
        _plots.Add(plot);
        plot.PlotChanged += OnPlotChanged;
        EquipLvChanged += plot.OnFarmEquipLvChanged;
        OnPlotChanged();
        return plot;
    }

    public Worker AddWorker()
    {
        Worker worker = new Worker();
        _workers.Add(worker);
        worker.WorkerStateChanged += OnWorkerChanged;
        OnWorkerChanged();
        return worker;
    }

    public int CountFreePlot()
    {
        int count = 0;
        foreach (FarmPlot plot in _plots)
        {
            if (!plot.HasCommodity)
                count++;
        }
        return count;
    }

    public int CountFreeWorker()
    {
        int count = 0;
        foreach (Worker worker in _workers)
        {
            if (worker.State == WorkerState.Idle)
                count++;
        }
        return count;
    }

    public FarmPlot GetFreePlot()
    {
        foreach (FarmPlot plot in _plots)
        {
            if (!plot.HasCommodity)
                return plot;
        }
        return null;
    }

    private void OnPlotChanged()
    {
        NotifyPlotChanged();
    }

    private void OnWorkerChanged()
    {
        NotifyWorkerChanged();
    }

    private void NotifyPlotChanged()
    {
        FarmPlotChanged?.Invoke();
    }

    private void NotifyWorkerChanged()
    {
        WorkerChanged?.Invoke();
    }

    private void NotifyGoldChanged()
    {
        GoldChanged?.Invoke(_gold);
    }

    private void NotifyEquipLvChanged()
    {
        EquipLvChanged?.Invoke(_equipLv);
    }

    public void Save(GameDataWriter writer)
    {
        long currentTimeStamp = TimeUtils.CurrentTimeStamp();

        writer.Write(saveFormatVersion);
        writer.Write(currentTimeStamp);

        _inventory.Save(writer);
        _achievement.Save(writer);

        // farm info
        writer.Write(_gold);
        writer.Write(_equipLv);

        // worker - just simple save worker quantity for now
        // can implement worker's current work (harvest/plant)
        // if I have more time
        writer.Write(_workers.Count);

        // farm plot
        // just ignore worker for now
        // can implement worker's current work (harvest/plant)
        // if I have more time
        writer.Write(_plots.Count);
        for (int i = 0; i < _plots.Count; i++)
        {
            _plots[i].Save(writer);
        }


        MLog.Log("FarmGame",
            "Save game at " + currentTimeStamp);
    }

    public void Load(GameDataReader reader)
    {
        long currentTimeStamp = TimeUtils.CurrentTimeStamp();

        int formatVersion = reader.ReadInt();
        long savedTimeStamp = reader.ReadLong();

        _inventory.Load(reader);
        _achievement.Load(reader);

        // farm info
        _gold = reader.ReadInt();
        _equipLv = reader.ReadInt();

        // worker - just simple save worker quantity for now
        // can implement worker's current work (harvest/plant)
        // if I have more time
        int workersCount = reader.ReadInt();
        // just clear all current workers and add new
        _workers.Clear();
        for (int i = 0; i < workersCount; i++)
        {
            AddWorker();
        }

        // farm plot
        // just ignore worker for now
        // can implement worker's current work (harvest/plant)
        // if I have more time
        int plotsCount = reader.ReadInt();
        // just clear all current plots and add new
        _plots.Clear();
        for(int i = 0; i < plotsCount; i++)
        {
            FarmPlot plot = AddPlot();
            plot.Load(reader);
        }

        // Notify after loading all data
        NotifyGoldChanged();
        NotifyEquipLvChanged();
        NotifyWorkerChanged();
        NotifyPlotChanged();

        long timeDiffSec = currentTimeStamp - savedTimeStamp;
        MLog.Log("FarmGame", string.Format( 
            "Load saved game at {0} \n" +
            "Format version: {1}\n" +
            "savedTimeStamp: {2}\n" +
            "timeDiffSec: {3}\n",
            currentTimeStamp, formatVersion, savedTimeStamp, timeDiffSec));
    }
}