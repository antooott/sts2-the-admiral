using Admiral.AdmiralCode.Cards.Status;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Rare.Attack;

public sealed class DesperateStrike() : AdmiralCard(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
   protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8, ValueProp.Move),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedHitCountVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature?, Decimal>) ((card, _) => (Decimal) CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count<CardGeneratedEntry>((Func<CardGeneratedEntry, bool>) (c => c.Creator == card.Owner && c.Card.Title == "Scurvy" && c.Card.Rarity == CardRarity.Status)))) 
    ];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Scurvy>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object?) cardPlay.Target);
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount((int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
}