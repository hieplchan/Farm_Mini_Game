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
    [SerializeField] private TMP_Text _farmInfoText;
    [SerializeField] private TMP_Text _plotText;
    [SerializeField] private TMP_Text _inventorySeedText;
    [SerializeField] private TMP_Text _inventoryProductText;

    [Header("Log Panel")]
    [SerializeField] private TMP_Text _logText;

    private void Start()
    {
        _presenter = new FarmGamePresenter(this);
    }

    public void BuyCommoditySeed(int type)
    {
        _presenter.BuyCommoditySeed(type);
    }

    public void PlantCommodity(int type)
    {
        _presenter.PlantCommodity(type);
    }

    public void CollectCommodityProduct(int type)
    {
        _presenter.CollectCommodityProduct(type);
    }
    public void SellCommodityProduct(int type)
    {
        _presenter.SellCommodityProduct(type);
    }

    public void BuyFarmPlot()
    {
        _presenter.BuyFarmPlot();
    }

    public void UpgradeEquipment()
    {
        _presenter.UpgradeEquipment();
    }

    private void Update()
    {
        _presenter.GameUpdate(Time.deltaTime);
    }

    public virtual void ShowUpdatedGoldAndEquipLevel(int gold, int equipLv)
    {
        string tmp = string.Format(
                "\n\nFarm\n\n" +
                "Gold: {0} \n" +
                "Equip Lv: {1}",
                gold, equipLv);
        _farmInfoText.text = tmp;
    }

    public virtual void ShowUpdatedPlots(List<FarmPlot> farmPlots)
    {
        string farmPanelString = "";
        string resourcePanelString = "";
        int usedPlotCount = 0;

        for (int i = 0; i < farmPlots.Count; i++)
        {
            string tmp = string.Format("Plot {0}: ", i + 1);
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

    public virtual void ShowUpdatedInventorySeeds(int[] seeds)
    {
        string tmp = "\n\nSeed\n\n";
        for (int i = 0; i < seeds.Length; i++)
        {
            tmp += string.Format("{0}: {1}\n",
                ((CommodityType)i).ToString(), seeds[i]);
        }
        _inventorySeedText.text = tmp;
    }

    public virtual void ShowUpdatedInventoryProducts(int[] products)
    {
        string tmp = "\n\nCollected\n\n";
        for (int i = 0; i < products.Length; i++)
        {
            tmp += string.Format("{0}: {1}\n",
                ((CommodityProductType)i).ToString(), products[i]);
        }
        _inventoryProductText.text = tmp;
    }

    public virtual void ShowUpdatedLog(string text)
    {
        _logText.text = text + "\n" + _logText.text;
    }
}