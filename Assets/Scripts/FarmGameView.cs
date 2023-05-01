using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using TMPro;

public class FarmGameView : MonoBehaviour
{
    private FarmGamePresenter _presenter;

    [SerializeField]
    private TMP_Text farmPlotTxt;

    private void Start()
    {
        _presenter = new FarmGamePresenter(this);
    }

    private void Update()
    {
        _presenter.GameUpdate(Time.deltaTime);
    }

    public virtual void UpdatedGold(int currentGold)
    {

    }

    public virtual void UpdatedPlots(List<FarmPlot> farmPlots)
    {
        string finalString = "";
        for (int i = 0; i < farmPlots.Count; i++)
        {
            string tmp = string.Format("Plot {0}: ", i);
            if (farmPlots[i].HasCommodity)
            {
                tmp += string.Format("{0} ", farmPlots[i].CommodityType.ToString());
                if (farmPlots[i].AvailableProduct > 0)
                {
                    tmp += string.Format(
                        " - product : {0} - {1} min to die",
                        farmPlots[i].AvailableProduct,
                        farmPlots[i].TimeLeftToHarvest.SecToMin());
                }
            }
            else
            {
                tmp += "Unused";
            }

            finalString += tmp;
            finalString += "\n";
        }

        farmPlotTxt.text = finalString;
    }
}