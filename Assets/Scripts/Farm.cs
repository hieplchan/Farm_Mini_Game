using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Farm
{
    public ObservableCollection<FarmPlot> plotList;

    public Farm()
    {
        plotList = new ObservableCollection<FarmPlot>();
    }

    public FarmPlot AddPlot()
    {
        FarmPlot plot = new FarmPlot();
        plotList.Add(plot);
        return plot;
    }
}