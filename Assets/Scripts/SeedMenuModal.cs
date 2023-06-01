using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Shared;

public class SeedMenuModal : Modal
{
    [SerializeField]
    public Button strawberryButton, tomatoButton, blueberryButton, cowButton;

    public override async UniTask Cleanup()
    {
        strawberryButton.onClick.RemoveAllListeners();
        tomatoButton.onClick.RemoveAllListeners();
        blueberryButton.onClick.RemoveAllListeners();
        cowButton.onClick.RemoveAllListeners();
        await UniTask.CompletedTask;
    }
}
