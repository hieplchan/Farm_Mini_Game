using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public int Gold { get; private set; }
    public Farm Farm { get => _farm; }

    private FarmGameView _view;
    private Farm _farm;
    private Inventory _inventory;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;
        _farm = new Farm();
        _inventory = new Inventory();

        _farm.plotList.CollectionChanged += OnPlotListChanged;

        ConfigManager.Reload();
        Gold = 0;
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
        _view.UpdatedPlots(_farm.plotList.ToList());
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
        _view.UpdatedPlots(_farm.plotList.ToList());
    }

    private void OnPlotListChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _view.UpdatedPlots(_farm.plotList.ToList());
        MLog.Log("FarmGamePresenter", 
            "UpdatedPlots plot count: " + _farm.plotList.Count);
    }
}