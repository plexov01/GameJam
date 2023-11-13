using GameJam.Features.UI;
using UnityEngine;

namespace GameResources.Features.UI.Scripts.Buttons
{
    public class QuitButton : AbstractButton
    {
        protected override void ClickAction()
        {
            Application.Quit();
        }
    }
}