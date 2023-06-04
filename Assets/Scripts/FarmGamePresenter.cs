using SuperMaxim.Messaging;
using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    const long REFRESH_LOADING_DURATION_SEC = 30;

    public FarmGame Farm { get => _farm; }
    public FarmGameConfig FarmGameConfig { get => _farmGameConfig; }

    private FarmGameView _view;
    private FarmGame _farm;
    private PersistentStorage _persistentStorage;
    private bool _isGamePause = false;
    private FarmGameConfig _farmGameConfig;

    public FarmGamePresenter(FarmGameView view, 
        FarmGameConfig config, string persistentPath = "")
    {
        _view = view;
        _farmGameConfig = config;
        ConfigManager.Reload(_farmGameConfig);

        _farm = new FarmGame();
        _persistentStorage = new PersistentStorage(persistentPath);

        // Messenger Event Subcribe
        Messenger.Default.Subscribe<GoldChangedPayLoad>(OnGoldChanged);
        Messenger.Default.Subscribe<EquipmentLevelChangedPayLoad>(OnEquipLvChanged);
        Messenger.Default.Subscribe<PlotChangedPayLoad>(OnFarmPlotsChanged);
        Messenger.Default.Subscribe<NewAchievementPayLoad>(OnNewAchievement);
        Messenger.Default.Subscribe<WorkerChangedPayLoad>(OnFarmWorkerChanged);
        Messenger.Default.Subscribe<InventorySeedChangedPayLoad>(OnInventorySeedsChanged);
        Messenger.Default.Subscribe<InventoryProductChangedPayLoad>(OnInventoryProductsChanged);

        Logger.Instance.NewLog += OnNewLog;

        ShowUpdatedGoldAndEquipLevel();
        ShowUpdatedPlots();
        ShowUpdatedInventorySeeds();
        ShowUpdatedInventoryProducts();
        ShowUpdatedWorkers();

        ApplyNewGameConfig();

        if (_farm.Achievement.IsHalfTargetDone)
        {
            _view.LoadLateGame();
        }
    }

    public void SetGameConfig(FarmGameConfig config)
    {
        ConfigManager.Reload(config);
    }

    private void ApplyNewGameConfig()
    {
        NewGameConfig config = ConfigManager.GetNewGameConfig();

        _farm.Gold = config.initGold;
        for (int i = 0; i < config.initFarmPlot; i++)
        {
            _farm.AddPlot();
        }
        for (int i = 0; i < config.initSeeds.Length; i++)
        {
            _farm.Inventory.AddSeed((CommodityType)i, config.initSeeds[i]);
        }

        Logger.Instance.Log("Do you want to play, let's play!");
    }

    public void BuyCommoditySeed(int type)
    {
        CommodityType seedType = (CommodityType)type;
        if (_farm.Store.BuyCommoditySeed(seedType, 1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _farm.Inventory.AddSeed(seedType);
        }
        else
        {
            Logger.Instance.Log("Not enough money bro, lol");
        }
    }

    public void PlantCommodity(int type)
    {
        FarmPlot freePlot = _farm.GetFreePlot();
        if (freePlot != null)
        {
            Commodity seed = _farm.Inventory.GetSeed((CommodityType)type);
            if (seed != null)
            {
                freePlot.Plant(seed);
            }
            else
            {
                Logger.Instance.Log(
                    string.Format("Buy some {0} bro", 
                    ((CommodityType)type).ToString()));
            }
        }
        else
        {
            Logger.Instance.Log("Buy some plot bro");
        }
    }

    public void CollectCommodityProduct(int type)
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            if (plot.HasCommodity)
                if (plot.Commodity.Type == (CommodityType)type &&
                    plot.Commodity.AvailableProduct > 0)
                    _farm.Inventory.AddProduct((CommodityProductType)type, 
                        plot.Commodity.Harvest());
        }
    }

    public void CollectAllProduct()
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            if (plot.HasCommodity)
                if (plot.Commodity.AvailableProduct > 0)
                    _farm.Inventory.AddProduct((CommodityProductType)plot.CommodityType,
                        plot.Commodity.Harvest());
        }

        Logger.Instance.Log("I collect all product");
    }

    public void SellCommodityProduct(int type)
    {
        CommodityProductType productType = (CommodityProductType)type;
        _farm.Gold += _farm.Store.SellCommodityProduct(productType,
            _farm.Inventory.GetAllProduct(productType));
    }

    public void SellAllProduct()
    {
        for (int i = 0; i < ConfigManager.commodityTypeCount; i++)
        {
            CommodityProductType productType = (CommodityProductType)i;
            _farm.Gold += _farm.Store.SellCommodityProduct(productType,
                _farm.Inventory.GetAllProduct(productType));
        }
        Logger.Instance.Log("I sell all product");
    }

    public void BuyFarmPlot()
    {
        if (_farm.Store.BuyFarmPlot(1, _farm.Gold, out int neededGold)) 
        {
            _farm.Gold -= neededGold;
            _farm.AddPlot();
        } 
        else
        {
            Logger.Instance.Log("Not enough money to buy farm plot bro");
        }
    }

    public void HireWorker()
    {
        if (_farm.Store.HireWorker(1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _farm.AddWorker();
        }
        else
        {
            Logger.Instance.Log("Not enough money to buy hire worker lol");
        }
    }

    public void UpgradeEquipment()
    {
        if (_farm.Store.UpgradeEquipment(1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _farm.UpgradeEquipLv();
        }
        else
        {
            Logger.Instance.Log("Not enough money to buy upgrade equipment");
        }
    }

    public void SaveGame()
    {
        _isGamePause = true;
        _persistentStorage.Save(_farm);
        Logger.Instance.Log("Game Saved");
        _isGamePause = false;
    }

    // This is very simple load method
    // I can improve if I have more time
    // 1: Move to another thread to not block UI
    // 2: Escape while loop when no job to do...
    public void LoadGame()
    {
        _isGamePause = true;
        _persistentStorage.Load(_farm);

        MLog.Log("FarmGamePresenter", 
            "LoadGame timeDiff: " +
            _farm.differentTimeFromLastSave);

        while (_farm.differentTimeFromLastSave > REFRESH_LOADING_DURATION_SEC)
        {
            _farm.differentTimeFromLastSave -= REFRESH_LOADING_DURATION_SEC;
            FarmGameUpdate(REFRESH_LOADING_DURATION_SEC);
        }

        _isGamePause = false;
        Logger.Instance.Log("Game Loaded");
    }

    public void GameUpdate(float deltaTime)
    {
        //MLog.Log("FarmGamePresenter", "GameUpdate");
        if (!_isGamePause)
        {
            FarmGameUpdate(deltaTime);
        }
    }

    private void FarmGameUpdate(float deltaTime)
    {
        if (!_farm.isGameFinish)
        {
            foreach (FarmPlot plot in _farm.Plots)
            {
                plot.GameUpdate(deltaTime);
            }

            foreach (Worker worker in _farm.Workers)
            {
                worker.GameUpdate(deltaTime);
            }

            // Ask Idle workers to find a job, don't stay unemployed
            foreach (Worker worker in _farm.Workers)
            {
                if (worker.State == WorkerState.Idle)
                    worker.SearchForJob(_farm, _farm.Inventory);
            }
        }
    }

    private void OnGoldChanged(GoldChangedPayLoad obj)
    {
        ShowUpdatedGoldAndEquipLevel();
    }

    private void OnEquipLvChanged(EquipmentLevelChangedPayLoad obj)
    {
        ShowUpdatedGoldAndEquipLevel();
    }

    private void OnFarmPlotsChanged(PlotChangedPayLoad obj)
    {
        ShowUpdatedPlots();
    }

    private void OnFarmWorkerChanged(WorkerChangedPayLoad obj)
    {
        ShowUpdatedWorkers();
    }

    private void OnInventorySeedsChanged(InventorySeedChangedPayLoad obj)
    {
        ShowUpdatedInventorySeeds();
    }

    private void OnInventoryProductsChanged(InventoryProductChangedPayLoad obj)
    {
        ShowUpdatedInventoryProducts();
    }

    private void OnNewLog(string text)
    {
        _view.ShowUpdatedLog(text);
    }

    private void OnNewAchievement(NewAchievementPayLoad obj)
    {
        string achievementMessage = obj.NewAchievement;
        if (_farm.Achievement.IsGoldTargetDone)
        {
            _farm.isGameFinish = true;
        }
        Logger.Instance.Log(achievementMessage);

        if (_farm.Achievement.IsHalfTargetDone)
        {
            _view.LoadLateGame();
        }
    }

    private void ShowUpdatedGoldAndEquipLevel()
    {
        _view.ShowUpdatedGoldAndEquipLevel(_farm.Gold, _farm.EquipLv);
    }

    private void ShowUpdatedPlots()
    {
        _view.ShowUpdatedPlots(_farm.Plots);
    }

    private void ShowUpdatedWorkers()
    {
        _view.ShowUpdatedWorkers(_farm.Workers);
    }

    private void ShowUpdatedInventorySeeds()
    {
        _view.ShowUpdatedInventorySeeds(_farm.Inventory.Seeds);
    }

    private void ShowUpdatedInventoryProducts()
    {
        _view.ShowUpdatedInventoryProducts(_farm.Inventory.Products);
    }
}