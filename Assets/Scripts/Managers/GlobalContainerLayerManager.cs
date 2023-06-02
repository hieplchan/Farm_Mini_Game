using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Shared.Layers;

[DisallowMultipleComponent]
public class GlobalContainerLayerManager : ContainerLayerManager
{
    public static GlobalContainerLayerManager Root;

    private void Start()
    {
        Root = this;
    }

    private void OnDestroy()
    {
        Root = null;
    }
}
