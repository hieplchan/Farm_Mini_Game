using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public Farm Farm { get => _farm; }
    public Inventory Inventory { get => _inventory; }

    private FarmGameView _view;
    private Farm _farm;
    private Inventory _inventory;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;
        _farm = new Farm();
        _inventory = new Inventory();

        _farm.GoldChanged += OnGoldChanged;
        _farm.FarmPlotChanged += OnPlotListChanged;
        _inventory.SeedsChanged += OnInventorySeedChanged;
        _inventory.ProductsChanged += OnProductsChanged;

        ShowUpdatedGoldAndEquipLevel();
        ShowUpdatePlots();
        ShowUpdatedInventorySeeds();

        ConfigManager.Reload();
        _farm.Gold = 0;
    }

    public void BuyCommoditySeed(CommodityType type)
    {
        _farm.Gold -= 500;
        _inventory.AddSeed(type);
    }

    public void PlantCommodity(CommodityType type)
    {
        FarmPlot freePlot = _farm.GetFreePlot();
        if (freePlot != null)
        {
            Commodity seed = _inventory.GetSeed(type);
            if (seed != null)
            {
                freePlot.Plant(seed);
            }
            else
            {
                MLog.Log("FarmGamePresenter",
                    string.Format("PlantCommodity: No {0}, please buy", type.ToString()));
            }
        }
        else
        {
            MLog.Log("FarmGamePresenter",
                "PlantCommodity: No free plot, please buy");
        }
    }

    public void CollectCommodityProduct(CommodityType type)
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            if (plot.HasCommodity)
                if (plot.Commodity.Type == type &&
                    plot.Commodity.AvailableProduct > 0)
                    _inventory.AddProduct(type, plot.Commodity.Harvest());
        }
    }

    public void BuyFarmPlot()
    {
        _farm.Gold -= 500;

        _farm.AddPlot();
    }

    public void HireWorker()
    {
        _farm.Gold -= 500;
    }

    public void GameUpdate(float deltaTime)
    {
        foreach (FarmPlot plot in _farm.Plots)
        {
            plot.GameUpdate(deltaTime);
        }
    }

    private void OnGoldChanged()
    {
        ShowUpdatedGoldAndEquipLevel();
    }

    private void OnPlotChanged()
    {
        ShowUpdatePlots();
    }

    private void OnPlotListChanged()
    {
        ShowUpdatePlots();
    }

    private void OnInventorySeedChanged()
    {
        ShowUpdatedInventorySeeds();
    }

    private void OnProductsChanged()
    {
        ShowUpdatedInventoryProducts();
    }

    private void ShowUpdatedGoldAndEquipLevel()
    {
        _view.ShowUpdatedGoldAndEquipLevel(_farm.Gold, 1);
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