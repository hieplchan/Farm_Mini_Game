using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Screen;
using UnityScreenNavigator.Runtime.Core.Shared;

using Screen = UnityScreenNavigator.Runtime.Core.Screen.Screen;

public class HomeLoadingScreen : Screen
{
    public override void DidPushEnter()
    {
        var pushOption = new WindowOption("prefab_page_home", true);
        ScreenContainer.Of(transform).Push(pushOption);
    }
}