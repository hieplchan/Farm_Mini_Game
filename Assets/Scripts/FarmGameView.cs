using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FarmGameView : MonoBehaviour
{
    private FarmGamePresenter _presenter;

    [Header("Game Setting")]
    [SerializeField] private GameSetting _gameSetting;

    [Header("Farm Panel")]
    [SerializeField] private TMP_Text _farmPlotText;

    [Header("Resource Panel")]
    [SerializeField] private TMP_Text _farmInfoText;
    [SerializeField] private TMP_Text _plotText;
    [SerializeField] private TMP_Text _inventorySeedText;
    [SerializeField] private TMP_Text _inventoryProductText;
    [SerializeField] private TMP_Text _workerText;

    [Header("Log Panel")]
    [SerializeField] private TMP_Text _logText;

    private void Start()
    {
        FarmGameConfig config = GetConfigFromAsset();

        _presenter = new FarmGamePresenter(this, config,
            Application.persistentDataPath);
    }

    public void BuyCommoditySeed(int type)
    {
        _presenter.BuyCommoditySeed(type);
    }

    public void PlantCommodity(int type)
    {
        _presenter.PlantCommodity(type);
    }

    public void CollectCommodityProduct(int type)
    {
        _presenter.CollectCommodityProduct(type);
    }
    public void SellCommodityProduct(int type)
    {
        _presenter.SellCommodityProduct(type);
    }

    public void BuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    public void HireWorker()
    {
        _presenter.HireWorker();
    }

    public void UpgradeEquipment()
    {
        _presenter.UpgradeEquipment();
    }

    public void SaveGame()
    {
        _presenter.SaveGame();
    }

    public void LoadGame()
    {
        _presenter.LoadGame();
    }

    private void Update()
    {
        _presenter.GameUpdate(Time.deltaTime);
    }

    public virtual void ShowUpdatedGoldAndEquipLevel(int gold, int equipLv)
    {
        string tmp = string.Format(
                "\n\nFarm\n\n" +
                "Gold: {0} \n" +
                "Equip Lv: {1}",
                gold, equipLv);
        _farmInfoText.text = tmp;
    }

    public virtual void ShowUpdatedPlots(List<FarmPlot> farmPlots)
    {
        string farmPanelString = "";
        string resourcePanelString = "";
        int usedPlotCount = 0;

        for (int i = 0; i < farmPlots.Count; i++)
        {
            string tmp = string.Format("Plot {0}: ", i + 1);
            if (farmPlots[i].HasCommodity)
            {
                usedPlotCount++;

                tmp += string.Format("{0} ", farmPlots[i].CommodityType.ToString());
                if (farmPlots[i].AvailableProduct > 0)
                {
                    tmp += string.Format(
                        " - product : {0} - {1} min to die",
                        farmPlots[i].AvailableProduct,
                        farmPlots[i].TimeLeftToHarvest.SecToMin());
                }
                if (farmPlots[i].HasWorker)
                    tmp += " - Has Worker";
            }
            else
            {
                tmp += "Unused";
            }
            farmPanelString += tmp;
            farmPanelString += "\n";
        }

        resourcePanelString = string.Format(
            "\n\nPlot \n\n" +
            "Used: {0} \n" +
            "Free: {1}",
            usedPlotCount, farmPlots.Count - usedPlotCount
            );

        _farmPlotText.text = farmPanelString;
        _plotText.text = resourcePanelString;
    }

    public virtual void ShowUpdatedInventorySeeds(int[] seeds)
    {
        string tmp = "\n\nSeed\n\n";
        for (int i = 0; i < seeds.Length; i++)
        {
            tmp += string.Format("{0}: {1}\n",
                ((CommodityType)i).ToString(), seeds[i]);
        }
        _inventorySeedText.text = tmp;
    }

    public virtual void ShowUpdatedInventoryProducts(int[] products)
    {
        string tmp = "\n\nCollected\n\n";
        for (int i = 0; i < products.Length; i++)
        {
            tmp += string.Format("{0}: {1}\n",
                ((CommodityProductType)i).ToString(), products[i]);
        }
        _inventoryProductText.text = tmp;
    }

    public virtual void ShowUpdatedWorkers(List<Worker> workers)
    {
        int idleWorker = 0;
        foreach (Worker worker in workers)
        {
            if (worker.State == WorkerState.Idle)
                idleWorker += 1;
        }
        string workersString = string.Format(
            "\n\nWorker \n\n" +
            "Idle: {0} \n" +
            "Working: {1}",
            idleWorker, workers.Count - idleWorker);
        _workerText.text = workersString;
    }

    public virtual void ShowUpdatedLog(string text)
    {
        _logText.text = text + "\n" + _logText.text;
    }

    private FarmGameConfig GetConfigFromAsset()
    {
        FarmGameConfig config = ConfigManager.GetDefaultConfig();

        //Game Config
        config.targetGold = _gameSetting.targetGold;
        config.productivityIncreasePerEquipLv = _gameSetting.productivityIncreasePerEquipLv;

        //New Game
        config.newGameConfig.initGold = _gameSetting.newGameResource.gold;
        config.newGameConfig.initFarmPlot = _gameSetting.newGameResource.farmPlot;
        config.newGameConfig.initWorker = _gameSetting.newGameResource.worker;
        config.newGameConfig.initEquipLv = _gameSetting.newGameResource.equipLv;

        config.newGameConfig.initSeeds[(int)CommodityType.Strawberry] =
            _gameSetting.newGameResource.strawBerry;
        config.newGameConfig.initSeeds[(int)CommodityType.Tomato] =
            _gameSetting.newGameResource.tomato;
        config.newGameConfig.initSeeds[(int)CommodityType.Blueberry] =
            _gameSetting.newGameResource.blueBerry;
        config.newGameConfig.initSeeds[(int)CommodityType.Cow] =
            _gameSetting.newGameResource.cow;

        //Commodity
        config.commodityConfigs[(int)CommodityType.Strawberry].productCycleTime = 
            _gameSetting.strawberryConfig.cycleTime;
        config.commodityConfigs[(int)CommodityType.Strawberry].productCycleNum =
            _gameSetting.strawberryConfig.cycleNum;
        config.commodityConfigs[(int)CommodityType.Strawberry].dyingTime =
            _gameSetting.strawberryConfig.dyingTime;

        config.commodityConfigs[(int)CommodityType.Tomato].productCycleTime =
            _gameSetting.tomatoConfig.cycleTime;
        config.commodityConfigs[(int)CommodityType.Tomato].productCycleNum =
            _gameSetting.tomatoConfig.cycleNum;
        config.commodityConfigs[(int)CommodityType.Tomato].dyingTime =
            _gameSetting.tomatoConfig.dyingTime;

        config.commodityConfigs[(int)CommodityType.Blueberry].productCycleTime =
            _gameSetting.blueberryConfig.cycleTime;
        config.commodityConfigs[(int)CommodityType.Blueberry].productCycleNum =
            _gameSetting.blueberryConfig.cycleNum;
        config.commodityConfigs[(int)CommodityType.Blueberry].dyingTime =
            _gameSetting.blueberryConfig.dyingTime;

        config.commodityConfigs[(int)CommodityType.Cow].productCycleTime =
            _gameSetting.cowConfig.cycleTime;
        config.commodityConfigs[(int)CommodityType.Cow].productCycleNum =
            _gameSetting.cowConfig.cycleNum;
        config.commodityConfigs[(int)CommodityType.Cow].dyingTime =
            _gameSetting.cowConfig.dyingTime;

        //Store
        config.storeConfig.farmPlotPrice = _gameSetting.storeConfig.farmPlotPrice;
        config.storeConfig.hireWorkerPrice = _gameSetting.storeConfig.hireWorkerPrice;
        config.storeConfig.equipUpgradePrice = _gameSetting.storeConfig.equipUpgradePrice;

        config.storeConfig.seedPrices[(int)CommodityType.Strawberry] =
            _gameSetting.storeConfig.strawBerryBuyPrice;
        config.storeConfig.seedPrices[(int)CommodityType.Tomato] =
            _gameSetting.storeConfig.tomatoBuyPrice;
        config.storeConfig.seedPrices[(int)CommodityType.Blueberry] =
            _gameSetting.storeConfig.blueBerryBuyPrice;
        config.storeConfig.seedPrices[(int)CommodityType.Cow] =
            _gameSetting.storeConfig.cowBuyPrice;

        config.storeConfig.productPrices[(int)CommodityProductType.Strawberry] =
            _gameSetting.storeConfig.strawBerrySellPrice;
        config.storeConfig.productPrices[(int)CommodityProductType.Tomato] =
            _gameSetting.storeConfig.tomatoSellPrice;
        config.storeConfig.productPrices[(int)CommodityProductType.Blueberry] =
            _gameSetting.storeConfig.blueBerrySellPrice;
        config.storeConfig.productPrices[(int)CommodityProductType.Milk] =
            _gameSetting.storeConfig.milkSellPrice;

        //Worker
        config.workerConfig.timeNeededPerTask = _gameSetting.workerConfig.timeNeededPerTask;

        return config;
    }
}