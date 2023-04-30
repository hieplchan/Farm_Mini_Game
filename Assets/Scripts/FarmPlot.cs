using System;

public class FarmPlot
{
    public event Action FarmPlotChanged;
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
            NotifyChange();

            _commodity = null;
            _availableProduct = 0;
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
        FarmPlotChanged?.Invoke();
    }
}