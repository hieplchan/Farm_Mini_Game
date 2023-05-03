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
    int _gold;

    public event Action FarmPlotChanged;
    public List<FarmPlot> Plots { get => _plots; }
    private List<FarmPlot> _plots;

    public Farm()
    {
        _plots = new List<FarmPlot>();
    }

    public FarmPlot AddPlot()
    {
        FarmPlot plot = new FarmPlot();
        _plots.Add(plot);
        plot.PlotChanged += OnPlotChanged;
        OnPlotChanged();
        return plot;
    }

    public int CountFreePlot()
    {
        int count = 0;
        foreach (FarmPlot plot in _plots)
        {
            if (!plot.HasCommodity)
                count++;
        }
        return count;
    }

    public FarmPlot GetFreePlot()
    {
        foreach (FarmPlot plot in _plots)
        {
            if (!plot.HasCommodity)
                return plot;
        }
        return null;
    }

    private void OnPlotChanged()
    {
        FarmPlotChanged?.Invoke();
    }
}