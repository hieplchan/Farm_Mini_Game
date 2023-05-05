using System;
using System.Linq;

public class Inventory
{
    public event Action SeedsChanged;
    public int[] Seeds { get => _seeds; }
    private int[] _seeds;
    public bool HasSeed { get => _seeds.Sum() > 0; }

    public event Action ProductsChanged;
    public int[] Products { get => _products; }
    private int[] _products;

    public Inventory()
    {
        int commodityTypeCount = Enum.GetNames(typeof(CommodityType)).Length;

        _seeds = new int[commodityTypeCount];
        Array.Fill(_seeds, 0);

        _products = new int[commodityTypeCount];
        Array.Fill(_products, 0);
    }

    public void AddSeed(CommodityType type, int quantity = 1)
    {
        _seeds[(int)type] += quantity;
        NotifySeedsChange();
    }

    public Commodity GetSeed(CommodityType type)
    {
        if (_seeds[(int)type] > 0)
        {
            Commodity seed = new Commodity(type);
            _seeds[(int)type] -= 1;
            NotifySeedsChange();
            return seed;
        }
        else
        {
            MLog.Log("Inventory",
                string.Format("GetSeed: no {0} seed, please buy",
                type.ToString()));
            return null;
        }
    }

    // Get available seeds in order instead of random
    // Can improve if have more time
    public Commodity GetAvailableSeed()
    {
        if (!HasSeed)
        {
            return null;
        }
        else
        {
            for (int i = 0; i < _seeds.Length; i++)
                if (_seeds[i] > 0)
                    return GetSeed((CommodityType)i);
            return null;
        }
    }

    public void AddProduct(CommodityProductType type, int quantity = 1)
    {
        _products[(int)type] += quantity;
        NotifyProductsChanged();
    }

    public int GetAllProduct(CommodityProductType type)
    {
        int productCount = _products[(int)type];
        _products[(int)type] = 0;
        NotifyProductsChanged();
        return productCount;
    }

    private void NotifySeedsChange()
    {
        SeedsChanged?.Invoke();
    }

    private void NotifyProductsChanged()
    {
        ProductsChanged?.Invoke();
    }
}
