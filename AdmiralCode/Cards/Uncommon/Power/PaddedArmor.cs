using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Admiral.AdmiralCode.Cards.Uncommon.Power;

public class PaddedArmor() : AdmiralCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PaddedArmorPower>(6M)];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BiggerRoundsPower>(choiceContext, Owner.Creature,
            DynamicVars["PaddedArmorPower"].IntValue, Owner.Creature, this);
    }
}