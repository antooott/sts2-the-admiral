using Admiral.AdmiralCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace Admiral.AdmiralCode.Cards.Common;

public class SummonNibbit() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar(3)];
    protected override void OnUpgrade() => this.DynamicVars.Summon.UpgradeValueBy(2M);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AdmiralCmd.Summon<Nibbit>(choiceContext, this.Owner, DynamicVars.Summon.IntValue, cardPlay.Card);
    }
}