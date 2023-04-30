public class FarmGamePresenter
{
    public int Gold { get; private set; }
    private FarmGameView _view;

    public FarmGamePresenter(FarmGameView view)
    {
        _view = view;

        Gold = 0;
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
}
