using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Uncommon.Attack;

public class Stacked() : AdmiralCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure), HoverTipFactory.FromKeyword(AdmiralKeywords.Catch)];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(14M), new ExtraDamageVar(3M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature?, Decimal>) ((card, _) => (Decimal) PileType.Exhaust.GetPile(card.Owner).Cards.Count((c => c.Keywords.Contains(AdmiralKeywords.Treasure) || c.Keywords.Contains(AdmiralKeywords.Catch)))))
    ];
    protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(1M);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(this.DynamicVars.CalculatedDamage).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}