using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public Farm Farm { get => _farm; }
    public Inventory Inventory { get => _inventory; }
    public Store Store { get => _store; }

    private FarmGameView _view;
    private Farm _farm;
    private Inventory _inventory;
    private Store _store;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;
        _farm = new Farm();
        _inventory = new Inventory();
        _store = new Store();

        _farm.GoldChanged += OnGoldChanged;
        _farm.EquipLvChanged += OnEquipLvChanged;
        _farm.FarmPlotChanged += OnFarmPlotsChanged;
        _inventory.SeedsChanged += OnInventorySeedsChanged;
        _inventory.ProductsChanged += OnInventoryProductsChanged;

        ShowUpdatedGoldAndEquipLevel();
        ShowUpdatePlots();
        ShowUpdatedInventorySeeds();
        ShowUpdatedInventoryProducts();

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
            MLog.Log("FarmGamePresenter",
                "BuyCommoditySeed: not have enough money");
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
                MLog.Log("FarmGamePresenter",
                    string.Format("PlantCommodity: No {0}, please buy", ((CommodityType)type).ToString()));
            }
        }
        else
        {
            MLog.Log("FarmGamePresenter",
                "PlantCommodity: No free plot, please buy");
        }
    }

    public void CollectCommodityProduct(int type)
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            if (plot.HasCommodity)
                if (plot.Commodity.Type == (CommodityType)type &&
                    plot.Commodity.AvailableProduct > 0)
                    _inventory.AddProduct((CommodityType)type, 
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
            MLog.Log("FarmGamePresenter",
                    "BuyFarmPlot: not have enough money");
        }
    }

    public void HireWorker()
    {
        _farm.Gold -= 500;
    }

    public void UpgradeEquipment()
    {
        if (_store.UpgradeEquipment(1, _farm.Gold, out int neededGold))
        {
            _farm.Gold -= neededGold;
            _farm.UpgradEquipLv();
        }
        else
        {
            MLog.Log("FarmGamePresenter",
                    "UpgradeEquipment: not have enough money");
        }
    }

    public void GameUpdate(float deltaTime)
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            plot.GameUpdate(deltaTime);
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
        ShowUpdatePlots();
    }

    private void OnInventorySeedsChanged()
    {
        ShowUpdatedInventorySeeds();
    }

    private void OnInventoryProductsChanged()
    {
        ShowUpdatedInventoryProducts();
    }

    private void ShowUpdatedGoldAndEquipLevel()
    {
        _view.ShowUpdatedGoldAndEquipLevel(_farm.Gold, _farm.EquipLv);
    }

    private void ShowUpdatePlots()
    {
        _view.ShowUpdatedPlots(_farm.Plots);
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