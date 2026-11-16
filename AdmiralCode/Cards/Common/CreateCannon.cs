using Admiral.AdmiralCode.Commands;
using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Admiral.AdmiralCode.Cards.Common;

public class CreateCannon() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GunpowderPower>(2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AdmiralCmd.SummonCannon(choiceContext, this.Owner, cardPlay.Card, DynamicVars["GunpowderPower"].IntValue);
    }
}