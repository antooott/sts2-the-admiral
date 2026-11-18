using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class PlipPlap() : AdmiralCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2);
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // decomp game code has ThrowIfNull((object) cardPlay.Target, "cardPlay.Target")
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .WithHitCount(2)
            // ~
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            // taken from Strike template, decomp game code has string
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }
}