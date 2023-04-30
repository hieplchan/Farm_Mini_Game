public class FarmPlot
{
    Commodity _commodity;

    public void GameUpdate(float deltaTime)
    {
        _commodity?.GameUpdate(deltaTime);
        if (_commodity?.State == CommodityState.Dead)
        {
            MLog.Log("Plot", "Remove: " + _commodity.Type);
            _commodity = null;
        }
    }

    public void Plant(Commodity commodity)
    {
        _commodity = commodity;
        _commodity.Plant(this);
    }
}
