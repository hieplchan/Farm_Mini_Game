using System;
using System.Collections.Specialized;
using System.Linq;

public class FarmGamePresenter
{
    public int Gold { get; private set; }
    public Farm Farm { get => _farm; }

    private FarmGameView _view;
    private Farm _farm;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;
        _farm = new Farm();
        _farm.plotList.CollectionChanged += OnPlotListChanged;

        ConfigManager.Reload();
        Gold = 0;

        FarmTestSetup();
    }

    public void BuyFarmPlot()
    {
        Gold -= 500;

        _farm.AddPlot();

        _view.UpdatedGold(Gold);
    }

    public void HireWorker()
    {
        Gold -= 500;

        _view.UpdatedGold(Gold);
    }

    private void FarmTestSetup()
    {
        // Just for test
        for (int i = 0; i < 4; i++)
        {
            Commodity commodity = new Commodity(
                ConfigManager.GetCommodityConfig((CommodityType)i),
                (CommodityType)i);

            FarmPlot plot = _farm.AddPlot();
            plot.FarmPlotChanged += OnPlotChanged;
            plot.Plant(commodity);
        }
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

    private  void OnPlotListChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _view.UpdatedPlots(_farm.plotList.ToList());
        MLog.Log("FarmGamePresenter", 
            "UpdatedPlots plot count: " + _farm.plotList.Count);
    }
}
