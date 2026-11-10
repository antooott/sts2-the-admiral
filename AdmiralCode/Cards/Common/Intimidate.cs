using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Common;

public class Intimidate() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1), new PowerVar<VulnerablePower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_flying_slash");
        
        foreach (Creature enemy in (IEnumerable<Creature>)this.CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, (Decimal)DynamicVars["WeakPower"].IntValue,
                this.Owner.Creature, (CardModel)this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy,
                (Decimal)DynamicVars["VulnerablePower"].IntValue, this.Owner.Creature, (CardModel)this);
        }
    }
}