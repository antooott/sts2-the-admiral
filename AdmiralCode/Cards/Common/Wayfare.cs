using Admiral.AdmiralCode.Cards.Status;
using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class Wayfare() : AdmiralCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Scurvy>()];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, this.Owner.Creature, 1, this.Owner.Creature, (CardModel) this);
        await PowerCmd.Apply<WayfarePower>(choiceContext, this.Owner.Creature, 1 , this.Owner.Creature, (CardModel) this);
    }
}