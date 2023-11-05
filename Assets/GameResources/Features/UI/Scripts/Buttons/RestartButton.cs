using GameJam.Features.UI;

namespace GameResources.Features.UI.Scripts.Buttons
{
    public class RestartButton : AbstractButton
    {
        protected override void ClickAction()
        {
            GameHandler.Instance.RestartGame();
        }
    }
}