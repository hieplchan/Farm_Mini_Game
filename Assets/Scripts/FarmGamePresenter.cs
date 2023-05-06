using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public Farm Farm { get => _farm; }
    public Inventory Inventory { get => _inventory; }
    public Store Store { get => _store; }
    public Achievement Achievement { get => _achievement; }

    private FarmGameView _view;
    private Farm _farm;
    private Inventory _inventory;
    private Store _store;
    private Achievement _achievement;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;
        _farm = new Farm();
        _inventory = new Inventory();
        _store = new Store();
        _achievement = new Achievement();

        _farm.GoldChanged += OnGoldChanged;
        _farm.GoldChanged += _achievement.OnGoldChanged;
        _farm.EquipLvChanged += OnEquipLvChanged;
        _farm.FarmPlotChanged += OnFarmPlotsChanged;
        _farm.WorkerChanged += OnFarmWorkerChanged;
        _inventory.SeedsChanged += OnInventorySeedsChanged;
        _inventory.ProductsChanged += OnInventoryProductsChanged;
        _achievement.NewAchievement += OnNewAchievement;
        Logger.Instance.NewLog += OnNewLog;

        ShowUpdatedGoldAndEquipLevel();
        ShowUpdatedPlots();
        ShowUpdatedInventorySeeds();
        ShowUpdatedInventoryProducts();
        ShowUpdatedWorkers();

        ApplyNewGameConfig();
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
            _inventory.AddSeed((CommodityType)i, config.initSeeds[i]);
        }

        Logger.Instance.Log("Do you want to play, let's play!");
    }

    public void BuyCommoditySeed(int type)
    {
        CommodityType seedType = (CommodityType)type;
        if (_store.BuyCommoditySeed(seedType, 1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _inventory.AddSeed(seedType);
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
            Commodity seed = _inventory.GetSeed((CommodityType)type);
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
                    _inventory.AddProduct((CommodityProductType)type, 
                        plot.Commodity.Harvest());
        }
    }

    public void SellCommodityProduct(int type)
    {
        CommodityProductType productType = (CommodityProductType)type;
        _farm.Gold += _store.SellCommodityProduct(productType, 
            _inventory.GetAllProduct(productType));
    }

    public void BuyFarmPlot()
    {
        if (_store.BuyFarmPlot(1, _farm.Gold, out int neededGold)) 
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
        if (_store.HireWorker(1, _farm.Gold, out int neededGold))
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
        if (_store.UpgradeEquipment(1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _farm.UpgradeEquipLv();
        }
        else
        {
            Logger.Instance.Log("Not enough money to buy upgrade equipment");
        }
    }

    public void GameUpdate(float deltaTime)
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
                worker.SearchForJob(_farm, _inventory);
        }
    }

    private void OnGoldChanged(int gold)
    {
        ShowUpdatedGoldAndEquipLevel();
    }

    private void OnEquipLvChanged(int obj)
    {
        ShowUpdatedGoldAndEquipLevel();
    }

    private void OnFarmPlotsChanged()
    {
        ShowUpdatedPlots();
    }

    private void OnFarmWorkerChanged()
    {
        ShowUpdatedWorkers();
    }

    private void OnInventorySeedsChanged()
    {
        ShowUpdatedInventorySeeds();
    }

    private void OnInventoryProductsChanged()
    {
        ShowUpdatedInventoryProducts();
    }

    private void OnNewLog(string text)
    {
        _view.ShowUpdatedLog(text);
    }

    private void OnNewAchievement(string achievementMessage)
    {
        Logger.Instance.Log(achievementMessage);
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
        _view.ShowUpdatedInventorySeeds(_inventory.Seeds);
    }

    private void ShowUpdatedInventoryProducts()
    {
        _view.ShowUpdatedInventoryProducts(_inventory.Products);
    }
}