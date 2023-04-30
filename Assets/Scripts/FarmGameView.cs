using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class FarmGameView : MonoBehaviour
{
    private FarmGamePresenter _presenter;

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

    public void UpdatedPlots(List<FarmPlot> farmPlots)
    {
        
    }
}