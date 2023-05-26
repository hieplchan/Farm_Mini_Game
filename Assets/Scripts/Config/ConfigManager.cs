using System;

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

public class NewGameConfig
{
    public int initGold;
    public int initFarmPlot;
    public int initWorker;
    public int initEquipLv;
    public int[] initSeeds;

    public NewGameConfig(int commodityTypeCount)
    {
        initSeeds = new int[commodityTypeCount];
    }
}

public class CommodityConfig
{
    public int productCycleTime;
    public int productCycleNum;
    public int dyingTime;

    public CommodityConfig(int productCycleTime, int productCycleNum, int dyingTime)
    {
        this.productCycleTime = productCycleTime;
        this.productCycleNum = productCycleNum;
        this.dyingTime = dyingTime;
    }
}

public class StoreConfig
{
    public int farmPlotPrice;
    public int hireWorkerPrice;
    public int equipUpgradePrice;

    public int[] seedPrices;
    public int[] productPrices;

    public StoreConfig(int commodityTypeCount)
    {
        seedPrices = new int[commodityTypeCount];
        productPrices = new int[commodityTypeCount];
    }
}

public class WorkerConfig
{
    public int timeNeededPerTask;
}

public class FarmGameConfig
{
    public int targetGold;
    public int productivityIncreasePerEquipLv;
    public NewGameConfig newGameConfig;
    public CommodityConfig[] commodityConfigs;
    public StoreConfig storeConfig;
    public WorkerConfig workerConfig;

    public FarmGameConfig(int commodityTypeCount)
    {
        commodityConfigs = new CommodityConfig[commodityTypeCount];
        for (int i = 0; i < commodityTypeCount; i++)
        {
            commodityConfigs[i] = new CommodityConfig(1, 1, 1);
        }

        newGameConfig = new NewGameConfig(commodityTypeCount);
        storeConfig = new StoreConfig(commodityTypeCount);
        workerConfig = new WorkerConfig();
    }
}

public static class ConfigManager
{
    public static int commodityTypeCount;
    public static FarmGameConfig _config;  

    static ConfigManager()
    {
        commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;
        _config = new FarmGameConfig(commodityTypeCount);
        Reload(GetDefaultConfig());
    }

    public static void Reload(FarmGameConfig newConfig)
    {
        _config = newConfig;
    }

    public static NewGameConfig GetNewGameConfig()
    {
        return _config.newGameConfig;
    }

    public static CommodityConfig GetCommodityConfig(CommodityType type)
    {
        if ((int)type < commodityTypeCount)
        {
            return _config.commodityConfigs[(int)type];
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
        return _config.storeConfig.farmPlotPrice;
    }

    public static int GetStoreHireWorkerPrice()
    {
        return _config.storeConfig.hireWorkerPrice;
    }

    public static int GetStoreEquipUpgradePrice()
    {
        return _config.storeConfig.equipUpgradePrice;
    }

    public static int GetStoreSeedPrice(CommodityType type)
    {
        if ((int)type < commodityTypeCount)
        {
            return _config.storeConfig.seedPrices[(int)type];
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
            return _config.storeConfig.productPrices[(int)type];
        }
        else
        {
            MLog.LogError("ConfigManager",
                "GetStoreProductPrice wrong product type: " + (int)type);
            return 0;
        }
    }
    public static WorkerConfig GetWorkerConfig()
    {
        return _config.workerConfig;
    }

    public static int GetTargetGold()
    {
        return _config.targetGold;
    }

    public static int GetProductivityEquipment()
    {
        return _config.productivityIncreasePerEquipLv;
    }


    public static FarmGameConfig GetDefaultConfig()
    {
        FarmGameConfig config = new FarmGameConfig(commodityTypeCount);

        //Game Config
        config.targetGold = 1000000;
        config.productivityIncreasePerEquipLv = 10;

        //New Game
        config.newGameConfig.initGold = 9999;
        config.newGameConfig.initFarmPlot = 6;
        config.newGameConfig.initWorker = 1;
        config.newGameConfig.initEquipLv = 1;
        config.newGameConfig.initSeeds[(int)CommodityType.Strawberry] = 3;
        config.newGameConfig.initSeeds[(int)CommodityType.Tomato] = 4;
        config.newGameConfig.initSeeds[(int)CommodityType.Blueberry] = 5;
        config.newGameConfig.initSeeds[(int)CommodityType.Cow] = 6;

        //Commodity
        config.commodityConfigs[(int)CommodityType.Strawberry].productCycleTime = 1;
        config.commodityConfigs[(int)CommodityType.Strawberry].productCycleNum = 2;
        config.commodityConfigs[(int)CommodityType.Strawberry].dyingTime = 3;

        config.commodityConfigs[(int)CommodityType.Tomato].productCycleTime = 1;
        config.commodityConfigs[(int)CommodityType.Tomato].dyingTime = 3;

        config.commodityConfigs[(int)CommodityType.Blueberry].productCycleTime = 1;
        config.commodityConfigs[(int)CommodityType.Blueberry].productCycleNum = 4;
        config.commodityConfigs[(int)CommodityType.Blueberry].dyingTime = 3;

        config.commodityConfigs[(int)CommodityType.Cow].productCycleTime = 1;
        config.commodityConfigs[(int)CommodityType.Cow].productCycleNum = 5;
        config.commodityConfigs[(int)CommodityType.Cow].dyingTime = 3;

        //Store
        config.storeConfig.farmPlotPrice = 500;
        config.storeConfig.hireWorkerPrice = 200;
        config.storeConfig.equipUpgradePrice = 400;

        config.storeConfig.seedPrices[(int)CommodityType.Strawberry] = 200;
        config.storeConfig.seedPrices[(int)CommodityType.Tomato] = 300;
        config.storeConfig.seedPrices[(int)CommodityType.Blueberry] = 400;
        config.storeConfig.seedPrices[(int)CommodityType.Cow] = 500;

        config.storeConfig.productPrices[(int)CommodityProductType.Strawberry] = 600;
        config.storeConfig.productPrices[(int)CommodityProductType.Tomato] = 700;
        config.storeConfig.productPrices[(int)CommodityProductType.Blueberry] = 800;
        config.storeConfig.productPrices[(int)CommodityProductType.Milk] = 900;

        //Worker
        config.workerConfig.timeNeededPerTask = 2;

        return config;
    }
}