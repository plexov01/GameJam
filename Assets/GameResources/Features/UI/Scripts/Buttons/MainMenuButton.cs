using GameJam.Features.UI;

namespace GameResources.Features.UI.Scripts.Buttons
{
    public class MainMenuButton : AbstractButton
    {
        protected override void ClickAction()
        {
            SceneLoader.LoadScene(SceneLoader.Scene.Menu);
            GameHandler.Instance.ChangeState(GameHandler.State.WaitingForStart);
        }
    }
}