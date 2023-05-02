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

    public int CountFreePlot()
    {
        int count = 0;
        foreach (FarmPlot plot in plotList)
        {
            if (!plot.HasCommodity)
                count++;
        }
        return count;
    }

    public FarmPlot GetFreePlot()
    {
        foreach (FarmPlot plot in plotList)
        {
            if (!plot.HasCommodity)
                return plot;
        }
        return null;
    }
}