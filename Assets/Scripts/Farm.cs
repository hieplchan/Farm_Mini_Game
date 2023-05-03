using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class Farm
{
    public event Action GoldChanged;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            GoldChanged?.Invoke();
        }
    }
    public ObservableCollection<FarmPlot> plotList;

    int _gold;

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