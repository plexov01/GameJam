using GameJam.Features.UI;

namespace GameResources.Features.UI.Scripts.Buttons
{
    public class SkipButton : AbstractButton
    {
        protected override void ClickAction()
        {
            gameObject.SetActive(false);
            SceneLoader.LoadScene(SceneLoader.Scene.TowerDefense_NEW);
            GameHandler.Instance.ChangeState(GameHandler.State.FirstStage);
        }
    }
}