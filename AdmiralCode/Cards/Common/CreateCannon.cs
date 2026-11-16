using Admiral.AdmiralCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Admiral.AdmiralCode.Cards.Common;

public class CreateCannon() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AdmiralCmd.SummonCannon(choiceContext, this.Owner, cardPlay.Card);
    }
}