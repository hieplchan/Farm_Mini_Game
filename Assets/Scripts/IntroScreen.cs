using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Screen;
using UnityScreenNavigator.Runtime.Core.Shared;

using Screen = UnityScreenNavigator.Runtime.Core.Screen.Screen;

public class IntroScreen : Screen
{
    [SerializeField] private Button _button;

    protected override void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    protected override void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        var option = new WindowOption("prefab_page_home_loading", true);
        ScreenContainer.Of(transform).Push(option);
    }
}
