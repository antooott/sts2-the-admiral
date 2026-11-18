using Admiral.AdmiralCode.Cards.Status;
using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Uncommon;

public sealed class SetSail() : AdmiralCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Scurvy>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<SetSailPower>(choiceContext, this.Owner.Creature,
            1M, this.Owner.Creature, (CardModel)this);
    }
}