using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameJam.Features.CardSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildTowerCard", menuName = "Cards/UpgradeTowerCard")]
public class UpgradeTowerCard : AbstractCard
{
    public class BuildTowerCard : AbstractCard
    {
        public override void ActivateCard()
        {
            base.ActivateCard();
            TDManager.instance.ChangeTurretTier(true);
        }
    }
}
