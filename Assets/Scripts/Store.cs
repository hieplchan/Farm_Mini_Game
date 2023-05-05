public class Store
{
    public bool BuyFarmPlot(int quantity, int currentGold, out int neededGold)
    {
        neededGold = ConfigManager.GetStoreFarmPlotPrice() * quantity;
        if (currentGold > neededGold)
        {
            return true;
        }
        else
        {
            neededGold = 0;
            return false;
        }
    }

    public bool UpgradeEquipment(int quantity, int currentGold, out int neededGold)
    {
        neededGold = ConfigManager.GetStoreFarmPlotPrice() * quantity;
        if (currentGold > neededGold)
        {
            return true;
        }
        else
        {
            neededGold = 0;
            return false;
        }
    }

    public bool BuyCommoditySeed(CommodityType type, int quantity, int currentGold, out int neededGold)
    {
        neededGold = ConfigManager.GetStoreSeedPrice(type) * quantity;
        if (currentGold >= neededGold)
        {
            return true;
        }
        else
        {
            neededGold = 0;
            return false;
        }
    }

    public int SellCommodityProduct(CommodityProductType type, int quantity)
    {
        return ConfigManager.GetStoreProductPrice(type) * quantity;
    }
}