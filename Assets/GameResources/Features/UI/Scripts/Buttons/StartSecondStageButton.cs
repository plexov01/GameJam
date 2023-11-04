using System.Collections;
using System.Collections.Generic;
using GameJam.Features.UI;
using UnityEngine;

public class StartSecondStageButton : AbstractButton
{
    protected override void ClickAction()
    {
        GameHandler.Instance.StartSecondStage();
    }
}
