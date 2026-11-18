using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class Slide() : AdmiralCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move)];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
    }
    
    private void ReduceCost() => this.EnergyCost.SetThisTurn(0);
    private void RestoreCost() => this.EnergyCost.SetThisTurn(this.CanonicalEnergyCost);
    protected override bool ShouldGlowGoldInternal => this.Owner.Creature.HasPower<VigorPower>();
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || !this.Owner.Creature.HasPower<VigorPower>())
            return Task.CompletedTask;
        this.ReduceCost();
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayedLate(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!this.Owner.Creature.HasPower<VigorPower>())
        {
            this.RestoreCost();
            return base.AfterCardPlayedLate(context, cardPlay);
        }
        this.ReduceCost();
        return base.AfterCardPlayedLate(context, cardPlay);
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!this.Owner.Creature.HasPower<VigorPower>())
            return base.AfterPlayerTurnStart(choiceContext, player);
        this.ReduceCost();
        return base.AfterPlayerTurnStart(choiceContext, player);
    }
}