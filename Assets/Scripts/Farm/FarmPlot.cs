using SuperMaxim.Messaging;
using System;

public class FarmPlot : IPersistableObject
{
    public string ID { get => _id; }
    private string _id;

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

    public FarmPlot()
    {
        _id = Guid.NewGuid().ToString();
        Messenger.Default.Subscribe<EquipmentLevelChangedPayLoad>(OnFarmEquipLvChanged);
        NotifyPlotChange();
    }

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

    private void AddCommodity(Commodity commodity)
    {
        _commodity = commodity;
        _commodity.SetPlot(this);
    }

    private void NotifyPlotChange()
    {
        var payload = new PlotChangedPayLoad { ID = _id };
        Messenger.Default.Publish(payload);
    }

    public void OnFarmEquipLvChanged(EquipmentLevelChangedPayLoad obj)
    {
        _productivity = 1.0f +
            obj.EquipmentLevel * ConfigManager.GetProductivityEquipment() / 100f;
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

    public void Save(GameDataWriter writer)
    {
        writer.Write(_productivity);
        writer.Write(_availableProduct);
        writer.Write(HasCommodity);
        int type = -1;
        if (HasCommodity)
        {
            type = (int)_commodity.Type;
            writer.Write(type);
            _commodity.Save(writer);
        }

        MLog.Log("FarmPlot", string.Format(
            "Save: \n" +
            "_productivity: {0}\n" +
            "_availableProduct: {1}\n" +
            "HasCommodity: {2}\n" +
            "type: {3}",
            _productivity, _availableProduct, HasCommodity,
            type >=0 ? ((CommodityType)type).ToString() : type));
    }

    public void Load(GameDataReader reader)
    {
        _productivity = reader.ReadFloat();
        _availableProduct = reader.ReadInt();
        bool hasCommodity = reader.ReadBool();
        int type = -1;
        if (hasCommodity)
        {
            type = reader.ReadInt();
            Commodity commodity = new Commodity((CommodityType)type);
            commodity.Load(reader);
            AddCommodity(commodity);
        }

        NotifyPlotChange();

        MLog.Log("FarmPlot", string.Format(
            "Load: \n" +
            "_productivity: {0}\n" +
            "_availableProduct: {1}\n" +
            "HasCommodity: {2}\n" +
            "type: {3}",
            _productivity, _availableProduct, hasCommodity,
            type >= 0 ? ((CommodityType)type).ToString() : type));
    }
}