using System;

public class FarmGamePresenter
{
    public int Gold { get; private set; }
    private FarmGameView _view;
    private Farm _farm;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;

        ConfigManager.Reload();

        Gold = 0;
        _farm = new Farm();

        FarmSetup();
    }

    public void BuyFarmPlot()
    {
        Gold -= 500;

        _view.UpdatedGold(Gold);
    }

    public void HireWorker()
    {
        Gold -= 500;

        _view.UpdatedGold(Gold);
    }

    private void FarmSetup()
    {
        // Just for test
        for (int i = 0; i < Enum.GetNames(typeof(CommodityType)).Length; i++)
        {
            _farm.plotList.Add(new FarmPlot());

            Commodity commodity = new Commodity(
                ConfigManager.GetCommodityConfig((CommodityType)i), 
                (CommodityType)i);

            _farm.plotList[i].Plant(commodity);
        }
    }

    public void GameUpdate(float deltaTime)
    {
        foreach (FarmPlot plot in _farm.plotList)
        {
            plot.GameUpdate(deltaTime);
        }
    }
}
