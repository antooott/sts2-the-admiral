using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Rare;

public sealed class AvianForm() : AdmiralCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<AvianFormPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
        await PowerCmd.Apply<AvianFormPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
    }
    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Ethereal);
}