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
            GoldChanged?.Invoke(_gold);
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
            EquipLvChanged?.Invoke(_equipLv);
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
        FarmPlotChanged?.Invoke();
    }

    private void OnWorkerChanged()
    {
        WorkerChanged?.Invoke();
    }

    public void Save(GameDataWriter writer)
    {
        writer.Write(saveFormatVersion);
        _inventory.Save(writer);
        _achievement.Save(writer);
    }

    public void Load(GameDataReader reader)
    {
        int formatVersion = reader.ReadInt();
        _inventory.Load(reader);
        _achievement.Load(reader);

        MLog.Log("FarmGame", 
            "Load saved game, format version: " +
            formatVersion);
    }
}