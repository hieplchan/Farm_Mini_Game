using System;

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

public class StoreConfig
{
    public int farmPlotPrice;
    public int[] seedPrices;
    public int[] productPrices;

    public StoreConfig(int commodityTypeCount)
    {
        seedPrices = new int[commodityTypeCount];
        productPrices = new int[commodityTypeCount];
    }
}

public static class ConfigManager
{
    public static int commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

    public static CommodityConfig strawberry = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig tomato = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig blueberry = new CommodityConfig(1, 1, 1, 100);
    public static CommodityConfig cow = new CommodityConfig(1, 1, 1, 100);

    public static StoreConfig storeConfig = new StoreConfig(commodityTypeCount);

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

        // store config
        storeConfig.farmPlotPrice = 500;

        storeConfig.seedPrices[(int)CommodityType.Strawberry] = 200;
        storeConfig.seedPrices[(int)CommodityType.Tomato] = 300;
        storeConfig.seedPrices[(int)CommodityType.Blueberry] = 400;
        storeConfig.seedPrices[(int)CommodityType.Cow] = 500;

        storeConfig.productPrices[(int)CommodityProductType.Strawberry] = 600;
        storeConfig.productPrices[(int)CommodityProductType.Tomato] = 700;
        storeConfig.productPrices[(int)CommodityProductType.Blueberry] = 800;
        storeConfig.productPrices[(int)CommodityProductType.Milk] = 900;
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

    public static int GetStoreFarmPlotPrice()
    {
        return storeConfig.farmPlotPrice;
    }

    public static int GetStoreSeedPrice(CommodityType type)
    {
        if ((int)type >= commodityTypeCount)
        {
            MLog.LogError("ConfigManager", 
                "GetStoreSeedPrice wrong seed type: " + (int)type);
            return 0;
        }
        else
        {
            return storeConfig.seedPrices[(int)type];
        }
    }

    public static int GetStoreProductPrice(CommodityProductType type)
    {
        if ((int)type >= commodityTypeCount)
        {
            MLog.LogError("ConfigManager", 
                "GetStoreProductPrice wrong product type: " + (int)type);
            return 0;
        }
        else
        {
            return storeConfig.productPrices[(int)type];
        }
    }
}