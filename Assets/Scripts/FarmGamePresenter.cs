using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public int Gold { get; private set; }
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

        _farm.plotList.CollectionChanged += OnPlotListChanged;
        _inventory.seedList.CollectionChanged += OnInventorySeedChanged;

        ShowUpdatePlots();
        ShowUpdatedInventorySeed();

        ConfigManager.Reload();
        Gold = 0;
    }

    public void BuyCommoditySeed(CommodityType type)
    {
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

    public void BuyFarmPlot()
    {
        Gold -= 500;

        FarmPlot newPlot = _farm.AddPlot();

        newPlot.FarmPlotChanged += OnPlotChanged;

        _view.UpdatedGold(Gold);
    }

    public void HireWorker()
    {
        Gold -= 500;

        _view.UpdatedGold(Gold);
    }

    public void GameUpdate(float deltaTime)
    {
        foreach (FarmPlot plot in _farm.plotList)
        {
            plot.GameUpdate(deltaTime);
        }
    }

    private void OnPlotChanged()
    {
        ShowUpdatePlots();
    }

    private void OnPlotListChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ShowUpdatePlots();
        MLog.Log("FarmGamePresenter", 
            "UpdatedPlots plot count: " + _farm.plotList.Count);
    }

    private void OnInventorySeedChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ShowUpdatedInventorySeed();
    }

    private void ShowUpdatedInventorySeed()
    {
        _view.ShowUpdatedInventorySeed(_inventory.seedList.ToList());
    }

    private void ShowUpdatePlots()
    {
        _view.ShowUpdatedPlots(_farm.plotList.ToList());
    }
}