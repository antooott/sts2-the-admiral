using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common.Attack;

public sealed class Frontload() : AdmiralCard(2, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move), new EnergyVar(1)];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive) != null)
            await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(this.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive)!)
                .WithHitFx(VfxCmd.slashPath)
                .Execute(choiceContext);
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }
}