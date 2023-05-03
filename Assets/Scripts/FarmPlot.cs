using System;

public class FarmPlot
{
    public event Action PlotChanged;
    public bool HasCommodity { get => _commodity != null; }
    public CommodityType CommodityType { get => _commodity.Type; }
    public int AvailableProduct { get => _availableProduct; }
    public float TimeLeftToHarvest { get => _commodity.TimeLeftToHarvest; }

    Commodity _commodity;
    int _availableProduct;

    public void GameUpdate(float deltaTime)
    {
        _commodity?.GameUpdate(deltaTime);

        if (_commodity != null) 
            if (_commodity.AvailableProduct != _availableProduct)
            {
                _availableProduct = _commodity.AvailableProduct;

                MLog.Log("Plot", "Notify AvailableProduct changed: " + _commodity.AvailableProduct);
                NotifyChange();
            }

        if (_commodity?.State == CommodityState.Dead)
        {
            MLog.Log("Plot", "Notify Commodity dead: " + _commodity.Type);
            _commodity = null;
            _availableProduct = 0;
            NotifyChange();
        }
    }

    public void Plant(Commodity commodity)
    {
        _commodity = commodity;
        _commodity.Plant(this);
        _availableProduct = _commodity.AvailableProduct;

        MLog.Log("Plot", "Notify Commodity planted: " + _commodity.Type);
        NotifyChange();
    }

    private void NotifyChange()
    {
        PlotChanged?.Invoke();
    }
}