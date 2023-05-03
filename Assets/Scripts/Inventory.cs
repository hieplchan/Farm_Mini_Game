using System.Collections.ObjectModel;
using System;

public class Inventory
{
    public event Action SeedsChanged;
    public int[] Seeds { get => _seeds; }
    private int[] _seeds;

    public Inventory()
    {
        int commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        _seeds = new int[commodityTypeCount];

        for (int i = 0; i < commodityTypeCount; i++)
        {
            _seeds[i] = 0;
        }
    }

    public void AddSeed(CommodityType type, int quantity = 1)
    {
        _seeds[(int)type] += quantity;
        NotifySeedsChange();
    }

    public Commodity GetSeed(CommodityType type)
    {
        if (_seeds[(int)type] > 0)
        {
            Commodity seed = new Commodity(
                ConfigManager.GetCommodityConfig(type), type);
            _seeds[(int)type] -= 1;
            NotifySeedsChange();
            return seed;
        }
        else
        {
            MLog.Log("Inventory",
                string.Format("GetSeed: no {0} seed, please buy",
                type.ToString()));
            return null;
        }
    }

    private void NotifySeedsChange()
    {
        SeedsChanged?.Invoke();
    }
}
