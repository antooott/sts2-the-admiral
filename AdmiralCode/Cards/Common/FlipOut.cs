using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public class FlipOut() : AdmiralCard(0, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];
    protected override bool HasEnergyCostX => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not null)
        {
            int count = this.ResolveEnergyXValue();
            if (this.IsUpgraded)
                ++count;
            await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(count).FromCard((CardModel) this, cardPlay).TargetingRandomOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        }
    }
}