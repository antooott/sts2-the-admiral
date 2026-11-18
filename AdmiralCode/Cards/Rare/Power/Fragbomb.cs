using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Rare.Power;

public sealed class Fragbomb() : AdmiralCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FragbombPower>(choiceContext, this.Owner.Creature,
            1M, this.Owner.Creature, (CardModel)this);
    }
}