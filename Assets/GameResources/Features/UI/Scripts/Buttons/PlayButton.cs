using GameJam.Features.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.Features.UI.Scripts.Buttons
{
    /// <summary>
    /// Реализация кнопки, которая загружает уровень 1
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class PlayButton : AbstractButton
    {

        protected override void ClickAction()
        {
            SceneLoader.LoadScene(SceneLoader.Scene.TowerDefense);
            GameHandler.Instance.ChangeState(GameHandler.State.FirstStage);
        }
    }
}
