using System.Collections;
using System.Collections.Generic;
using GameJam.Features.UI;
using UnityEngine;

public class TestButton_C : AbstractButton
{
    protected override void ClickAction()
    {
        GameHandler.Instance.FinishGame();
    }
}
