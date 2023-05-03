using System.Collections.ObjectModel;
using System;

public class Inventory
{
    public ObservableCollection<int> seedList;

    public Inventory()
    {
        seedList = new ObservableCollection<int>();

        int commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        for (int i = 0; i < commodityTypeCount; i++)
        {
            seedList.Add(0);
        }
    }

    public void AddSeed(CommodityType type, int quantity = 1)
    {
        seedList[(int)type] += quantity;
    }

    public Commodity GetSeed(CommodityType type)
    {
        Commodity seed = new Commodity(
            ConfigManager.GetCommodityConfig(type), type);
        return seed;
    }
}
