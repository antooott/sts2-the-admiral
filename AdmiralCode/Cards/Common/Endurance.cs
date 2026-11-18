using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class Endurance() : AdmiralCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [(DynamicVar) new CalculationBaseVar(0M), (DynamicVar) new CalculationExtraVar(1M), new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature?, Decimal>) ((card, _) => (Decimal) Math.Max(0, card.Owner.Creature.GetPowerAmount<VigorPower>())))];
    protected override void OnUpgrade() => this.DynamicVars.CalculationBase.UpgradeValueBy(3);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<VigorPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), this.DynamicVars.CalculatedBlock.Props, cardPlay);
    }
}