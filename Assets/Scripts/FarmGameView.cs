using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FarmGameView : MonoBehaviour
{
    private FarmGamePresenter _presenter;

    [Header("Farm Panel")]
    [SerializeField] private TMP_Text _farmPlotText;

    [Header("Resource Panel")]
    [SerializeField] private TMP_Text _plotText;
    [SerializeField] private TMP_Text _inventorySeedText;

    private void Start()
    {
        _presenter = new FarmGamePresenter(this);
    }

    public void BuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    public void PlantCommodity(int type)
    {
        _presenter.PlantCommodity((CommodityType)type);
    }

    public void BuyCommoditySeed(int type)
    {
        _presenter.BuyCommoditySeed((CommodityType)type);
    }

    private void Update()
    {
        _presenter.GameUpdate(Time.deltaTime);
    }

    public virtual void UpdatedGold(int currentGold)
    {

    }

    public virtual void ShowUpdatedPlots(List<FarmPlot> farmPlots)
    {
        string farmPanelString = "";
        string resourcePanelString = "";
        int usedPlotCount = 0;

        for (int i = 0; i < farmPlots.Count; i++)
        {
            string tmp = string.Format("Plot {0}: ", i);
            if (farmPlots[i].HasCommodity)
            {
                usedPlotCount++;

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
            farmPanelString += tmp;
            farmPanelString += "\n";
        }

        resourcePanelString = string.Format(
            "\n\nPlot \n\n" +
            "Used: {0} \n" +
            "Free: {1}",
            usedPlotCount, farmPlots.Count - usedPlotCount
            );

        _farmPlotText.text = farmPanelString;
        _plotText.text = resourcePanelString;
    }

    public virtual void ShowUpdatedInventorySeed(List<int> seedList)
    {
        string tmp = "\n\nSeed\n";
        for (int i = 0; i < seedList.Count; i++)
        {
            tmp += string.Format("{0}: {1}\n",
                ((CommodityType)i).ToString(), seedList[i]);
        }
        _inventorySeedText.text = tmp;
    }
}