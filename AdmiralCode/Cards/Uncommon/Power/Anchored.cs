using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Uncommon.Power;

public class Anchored() : AdmiralCard(-1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not null)
        {
            int count = this.ResolveEnergyXValue();
            await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature,
                2M * count, this.Owner.Creature, (CardModel)this);
        }
    }
}