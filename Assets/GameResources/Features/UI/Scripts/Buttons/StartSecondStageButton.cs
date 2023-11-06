using System.Collections;
using System.Collections.Generic;
using GameJam.Features.UI;
using UnityEngine;

public class StartSecondStageButton : AbstractButton
{
    protected override void ClickAction()
    {
        GameHandler.Instance.ChangeState(GameHandler.State.SecondStage);
        
        SoundManager soundManager = SoundManager.Instance;
        soundManager.PlaySound(soundManager.audioClipRefsSo.stealCheese,Camera.main.transform.position);
        
        Destroy(gameObject);
    }
}
