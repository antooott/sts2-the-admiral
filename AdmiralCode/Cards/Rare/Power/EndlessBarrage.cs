using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Rare.Power;

public class EndlessBarrage() : AdmiralCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LoadVar(1)];
    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EndlessBarragePower>(choiceContext, this.Owner.Creature,
            DynamicVars["Load"].IntValue, this.Owner.Creature, (CardModel)this);
    }
}