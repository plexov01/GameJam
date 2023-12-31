namespace GameJam.Features.CardSystem
{
    using UnityEngine;
    /// <summary>
    /// Карточка постройки стены
    /// </summary>
    [CreateAssetMenu(fileName = "BuildWallCard", menuName = "Cards/BuildWallCard")]
    public class BuildWallCard : AbstractCard
    {
        public override void ActivateCard()
        {
            base.ActivateCard();
            TDManager.instance.BuildWall();
        }
    }

}
