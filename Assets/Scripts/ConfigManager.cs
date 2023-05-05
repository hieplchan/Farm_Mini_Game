using System;

public class NewGameConfig
{
    public int initGold;
    public int initFarmPlot;
    public int[] initSeeds;
}

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
    public int equipUpgradePrice;

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
    public static int commodityTypeCount;

    public static int productivityIncreasePerEquipLv;

    public static NewGameConfig newGameConfig;
    public static CommodityConfig[] commodityConfigs;
    public static StoreConfig storeConfig;

    static ConfigManager()
    {
        commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        newGameConfig = new NewGameConfig();
        newGameConfig.initSeeds = new int[commodityTypeCount];

        commodityConfigs = new CommodityConfig[commodityTypeCount];
        for (int i = 0; i < commodityTypeCount; i++)
        {
            commodityConfigs[i] = new CommodityConfig(1, 1, 1, 100);
        }

        storeConfig = new StoreConfig(commodityTypeCount);

        Reload();
    }

    public static void Reload()
    {
        // Game Config
        productivityIncreasePerEquipLv = 10;

        // New Game
        newGameConfig.initGold = 9999;
        newGameConfig.initFarmPlot = 6;
        newGameConfig.initSeeds[(int)CommodityType.Strawberry] = 2;
        newGameConfig.initSeeds[(int)CommodityType.Tomato] = 3;
        newGameConfig.initSeeds[(int)CommodityType.Blueberry] = 4;
        newGameConfig.initSeeds[(int)CommodityType.Cow] = 5;

        // Commodity
        commodityConfigs[(int)CommodityType.Strawberry].productCycleTime = 1;
        commodityConfigs[(int)CommodityType.Strawberry].productCycleNum = 2;
        commodityConfigs[(int)CommodityType.Strawberry].dyingTime = 3;
        commodityConfigs[(int)CommodityType.Strawberry].productivity = 100;

        commodityConfigs[(int)CommodityType.Tomato].productCycleTime = 1;
        commodityConfigs[(int)CommodityType.Tomato].productCycleNum = 3;
        commodityConfigs[(int)CommodityType.Tomato].dyingTime = 3;
        commodityConfigs[(int)CommodityType.Tomato].productivity = 100;

        commodityConfigs[(int)CommodityType.Blueberry].productCycleTime = 1;
        commodityConfigs[(int)CommodityType.Blueberry].productCycleNum = 4;
        commodityConfigs[(int)CommodityType.Blueberry].dyingTime = 3;
        commodityConfigs[(int)CommodityType.Blueberry].productivity = 100;

        commodityConfigs[(int)CommodityType.Cow].productCycleTime = 1;
        commodityConfigs[(int)CommodityType.Cow].productCycleNum = 5;
        commodityConfigs[(int)CommodityType.Cow].dyingTime = 3;
        commodityConfigs[(int)CommodityType.Cow].productivity = 100;

        // Store
        storeConfig.farmPlotPrice = 500;
        storeConfig.equipUpgradePrice = 500;

        storeConfig.seedPrices[(int)CommodityType.Strawberry] = 200;
        storeConfig.seedPrices[(int)CommodityType.Tomato] = 300;
        storeConfig.seedPrices[(int)CommodityType.Blueberry] = 400;
        storeConfig.seedPrices[(int)CommodityType.Cow] = 500;

        storeConfig.productPrices[(int)CommodityProductType.Strawberry] = 600;
        storeConfig.productPrices[(int)CommodityProductType.Tomato] = 700;
        storeConfig.productPrices[(int)CommodityProductType.Blueberry] = 800;
        storeConfig.productPrices[(int)CommodityProductType.Milk] = 900;
    }
    
    public static NewGameConfig GetNewGameConfig()
    {
        return newGameConfig;
    }

    public static CommodityConfig GetCommodityConfig(CommodityType type)
    {
        if ((int)type < commodityTypeCount)
        {
            return commodityConfigs[(int)type];
        }
        else
        {
            MLog.LogError("ConfigManager",
                "GetStoreSeedPrice wrong seed type: " + (int)type);
            return null;
        }
    }

    public static int GetStoreFarmPlotPrice()
    {
        return storeConfig.farmPlotPrice;
    }

    public static int GetStoreEquipUpgradePrice()
    {
        return storeConfig.equipUpgradePrice;
    }

    public static int GetStoreSeedPrice(CommodityType type)
    {
        if ((int)type < commodityTypeCount)
        {
            return storeConfig.seedPrices[(int)type];
        }
        else
        {
            MLog.LogError("ConfigManager",
                "GetStoreSeedPrice wrong seed type: " + (int)type);
            return 0;
        }
    }

    public static int GetStoreProductPrice(CommodityProductType type)
    {
        if ((int)type < commodityTypeCount)
        {
            return storeConfig.productPrices[(int)type];
        }
        else
        {
            MLog.LogError("ConfigManager",
                "GetStoreProductPrice wrong product type: " + (int)type);
            return 0;
        }
    }
}