using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class Strategize() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(5), new PowerVar<WeakPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>(), HoverTipFactory.FromPower<WeakPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object?) cardPlay.Target, (string) "cardPlay.Target");
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, this.DynamicVars.Weak.BaseValue, this.Owner.Creature, (CardModel) this);
    }
    protected override void OnUpgrade() => DynamicVars["WeakPower"].UpgradeValueBy(1M);
}