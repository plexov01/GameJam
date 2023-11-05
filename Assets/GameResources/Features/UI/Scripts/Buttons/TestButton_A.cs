using GameJam.Features.UI;

namespace GameResources.Features.UI.Scripts.Buttons
{
    public class TestButton_A : AbstractButton
    {
        protected override void ClickAction()
        {
            CoolnessScaleController.Instance.AddCoolness(-50);
        }
    }
}