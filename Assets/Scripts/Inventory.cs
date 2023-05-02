public class Inventory
{
    public Commodity GetSeed(CommodityType type)
    {
        Commodity seed = new Commodity(
            ConfigManager.GetCommodityConfig(type), type);
        return seed;
    }
}
