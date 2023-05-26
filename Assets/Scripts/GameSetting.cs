using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameSetting : ScriptableObject
{
    [SerializeField]
    public int targetGold;

    [SerializeField]
    public int productivityIncreasePerEquipLv;

    [SerializeField]
    public NewGameSetting newGameResource;

    [SerializeField]
    public CommoditySetting strawberryConfig;
    [SerializeField]
    public CommoditySetting tomatoConfig;
    [SerializeField]
    public CommoditySetting blueberryConfig;
    [SerializeField]
    public CommoditySetting cowConfig;

    [SerializeField]
    public StoreSetting storeConfig;

    [SerializeField]
    public WorkerSetting workerConfig;
}

[System.Serializable]
public class NewGameSetting
{
    public int gold;
    public int farmPlot;
    public int worker;
    public int equipLv;

    public int strawBerry;
    public int tomato;
    public int blueBerry;
    public int cow;
}

[System.Serializable]
public class CommoditySetting
{
    public int cycleTime;
    public int cycleNum;
    public int dyingTime;
}

[System.Serializable]
public class StoreSetting
{
    public int farmPlotPrice;
    public int hireWorkerPrice;
    public int equipUpgradePrice;

    public int strawBerryBuyPrice;
    public int tomatoBuyPrice;
    public int blueBerryBuyPrice;
    public int cowBuyPrice;

    public int strawBerrySellPrice;
    public int tomatoSellPrice;
    public int blueBerrySellPrice;
    public int milkSellPrice;
}

[System.Serializable]
public class WorkerSetting
{
    public int timeNeededPerTask;
}