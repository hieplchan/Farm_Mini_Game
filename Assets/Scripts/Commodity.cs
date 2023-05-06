public enum CommodityType
{
    Strawberry,
    Tomato,
    Blueberry,
    Cow
}

public enum CommodityProductType
{
    Strawberry,
    Tomato,
    Blueberry,
    Milk
}

public enum CommodityState
{
    Seed,
    Mature,
    Dying,
    Dead
}

public class Commodity
{
    public CommodityState State { get; private set; }
    public CommodityType Type { get; private set; }
    public float Age { get; private set; }
    public float TimeLeftToHarvest { get => _totalLifeTime - Age; }
    public int AvailableProduct => _availableProduct;

    FarmPlot _plot;
    int _productCycleNum;
    int _availableProduct, _harvestedProduct, _totalProduct;
    int _productCycleTime, _matureTime, _totalLifeTime;

    public Commodity(CommodityType type)
    {
        Type = type;

        CommodityConfig config = ConfigManager.GetCommodityConfig(Type);
        _productCycleNum = config.productCycleNum;
        _productCycleTime = config.productCycleTime.MinToSec();
        _matureTime = _productCycleTime * _productCycleNum;
        _totalLifeTime = _matureTime + config.dyingTime.MinToSec();

        _availableProduct = _harvestedProduct = _totalProduct = 0;

        Age = 0f;
        State = CommodityState.Seed;
    }

    public void GameUpdate(float deltaTime)
    {
        if (State == CommodityState.Seed || State == CommodityState.Dead)
            return;

        Age += deltaTime * _plot.Productivity;

        if (Age <= _matureTime)
        {
            Produce();
            State = CommodityState.Mature;
        }
        else if (Age > _matureTime && Age <= _totalLifeTime)
        {
            Dying();
            State = CommodityState.Dying;
        }
        else
        {
            Dead();
            State = CommodityState.Dead;
        }
    }

    private void Produce()
    {
        CheckNewProduct();
    }
    private void Dying()
    {
        CheckNewProduct();           
    }
    private void Dead()
    {
        MLog.Log(Type.ToString(), "Dead - Age: " + Age);
    }
    private void CheckNewProduct()
    {
        if (_totalProduct >= _productCycleNum)
            return;

        if (Age > (_totalProduct + 1) * _productCycleTime)
        {
            _totalProduct += 1;
            _availableProduct = _totalProduct - _harvestedProduct;
            MLog.Log(Type.ToString(),
                "Available Product: " + _availableProduct +
                " - Age: " + Age);
        }
    }

    public void Plant(FarmPlot plot)
    {
        _plot = plot;
        State = CommodityState.Mature;
        MLog.Log(Type.ToString(), "Plant");
    }

    public int Harvest()
    {
        if (_availableProduct > 0)
        {
            _harvestedProduct += _availableProduct;
            int temp = _availableProduct;
            _availableProduct = 0;
            return temp;
        }
        else
        {
            return 0;
        }
    }
}
