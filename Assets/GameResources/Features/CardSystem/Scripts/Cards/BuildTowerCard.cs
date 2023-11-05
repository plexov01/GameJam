namespace GameJob.Features.CardSystem
{
    using UnityEngine;
    /// <summary>
    /// Карточка постройки башни
    /// </summary>
    [CreateAssetMenu(fileName = "BuildTowerCard", menuName = "Cards/BuildTowerCard")]
    public class BuildTowerCard : AbstractCard
    {
        public override void ActivateCard()
        {
            
            // BuildManager.instance.BuildTurret();
        }
    }
}

