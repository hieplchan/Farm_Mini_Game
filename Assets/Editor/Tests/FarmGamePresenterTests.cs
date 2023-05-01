using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

[TestFixture]
public class FarmGamePresenterTests
{
    private FarmGameView _view;
    private FarmGamePresenter _presenter;

    [Test]
    public void WhenBuyFarmPlotGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Gold;

        WhenBuyFarmPlot();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedGold()
    {
        GivenAFarmGame();

        WhenBuyFarmPlot();

        ThenShowsUpdatedGold();
    }

    [Test]
    public void WhenBuyFarmPlotShowsUpdatedPlotList()
    {
        GivenAFarmGame();

        WhenBuyFarmPlot();

        ThenShowsUpdatedPlotList();
    }


    [Test]
    public void WhenBuyFarmPlotPlotListCountIncrease()
    {
        GivenAFarmGame();
        int plotCount = _presenter.Farm.plotList.Count;

        WhenBuyFarmPlot();

        Assert.Less(plotCount, _presenter.Farm.plotList.Count);
    }

    [Test]
    public void WhenHireWorkerGoldDecrease()
    {
        GivenAFarmGame();
        int initGold = _presenter.Gold;

        WhenHireWorker();

        ThenGoldDecrease(initGold);
    }

    [Test]
    public void WhenHireWorkerShowsUpdatedGold()
    {
        GivenAFarmGame();

        WhenHireWorker();

        ThenShowsUpdatedGold();
    }

    private void GivenAFarmGame()
    {
        _view = Substitute.For<FarmGameView>();
        _presenter = new FarmGamePresenter(_view);
    }

    private void WhenBuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    private void WhenHireWorker()
    {
        _presenter.HireWorker();
    }

    private void ThenGoldDecrease(int initGold)
    {
        Assert.Less(_presenter.Gold, initGold);
    }

    private void ThenShowsUpdatedGold()
    {
        _view.Received(1).UpdatedGold(_presenter.Gold);
    }

    private void ThenShowsUpdatedPlotList()
    {
        _view.Received(1).UpdatedPlots(Arg.Is<List<FarmPlot>>(
            value => _presenter.Farm.plotList.ToList().SequenceEqual(value)));
    }
}
