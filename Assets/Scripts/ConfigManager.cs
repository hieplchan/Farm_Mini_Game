public class CommodityConfig
{
    public int productCycleTime;
    public int productCycleNum;
    public int dyingTime;
    public int productivity;

    public CommodityConfig(int productCycleTime, int productCycleNum, int dyingTime, int productivity)
    {
        this.productCycleTime = productCycleTime;
        this.productCycleNum = productCycleNum;
        this.dyingTime = dyingTime;
        this.productivity = productivity;
    }
}

public static class ConfigManager
{
    public static CommodityConfig strawberry = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig tomato = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig blueberry = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig cow = new CommodityConfig(1, 1, 1, 100);

    public static void Reload()
    {
        strawberry.productCycleTime = 1;
        strawberry.productCycleNum = 2;
        strawberry.dyingTime = 3;
        strawberry.productivity = 100;

        tomato.productCycleTime = 1;
        tomato.productCycleNum = 3;
        tomato.dyingTime = 3;
        tomato.productivity = 100;

        blueberry.productCycleTime = 1;
        blueberry.productCycleNum = 4;
        blueberry.dyingTime = 3;
        blueberry.productivity = 100;

        cow.productCycleTime = 1;
        cow.productCycleNum = 5;
        cow.dyingTime = 3;
        cow.productivity = 100;
    }

    public static CommodityConfig GetCommodityConfig(CommodityType type)
    {
        switch (type)
        {
            case CommodityType.Strawberry:
                return strawberry;
            case CommodityType.Tomato:
                return tomato;
            case CommodityType.Blueberry:
                return blueberry;
            case CommodityType.Cow:
                return cow;
        }
        MLog.LogError("Commodity", "Not support commodity type: " + type);
        return null;
    }
}