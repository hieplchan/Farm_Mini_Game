using System;

public class FarmPlot
{
    public event Action PlotChanged;
    public bool HasCommodity { get => _commodity != null; }
    public Commodity Commodity { get => _commodity; }
    public CommodityType CommodityType { get => _commodity.Type; }
    public int AvailableProduct { get => _availableProduct; }
    public float Productivity { get => _productivity; }
    public float TimeLeftToHarvest { get => _commodity.TimeLeftToHarvest; }

    public bool HasWorker { get => _currentWorker != null; }
    public Worker Worker { get => _currentWorker; }
    private Worker _currentWorker;

    Commodity _commodity;
    int _availableProduct;
    float _productivity = 1.0f;

    public void GameUpdate(float deltaTime)
    {
        _commodity?.GameUpdate(deltaTime);

        if (_commodity != null) 
            if (_commodity.AvailableProduct != _availableProduct)
            {
                _availableProduct = _commodity.AvailableProduct;

                MLog.Log("Plot", "Notify AvailableProduct changed: " + _commodity.AvailableProduct);
                NotifyPlotChange();
            }

        if (_commodity?.State == CommodityState.Dead)
        {
            MLog.Log("Plot", "Notify Commodity dead: " + _commodity.Type);
            _commodity = null;
            _availableProduct = 0;
            NotifyPlotChange();
        }
    }

    public void Plant(Commodity commodity)
    {
        _commodity = commodity;
        _commodity.Plant(this);
        _availableProduct = _commodity.AvailableProduct;

        MLog.Log("Plot", "Notify Commodity planted: " + _commodity.Type);
        NotifyPlotChange();
    }

    private void NotifyPlotChange()
    {
        PlotChanged?.Invoke();
    }

    public void OnFarmEquipLvChanged(int equipLv)
    {
        _productivity = 1.0f + 
            equipLv * ConfigManager.productivityIncreasePerEquipLv / 100f;
        MLog.Log("Plot", "OnFarmEquipLvChanged _productivity: " +
            _productivity);
    }

    public bool AddWorker(Worker worker)
    {
        if (!HasWorker)
        {
            _currentWorker = worker;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveWorker()
    {
        _currentWorker = null;
    }
}