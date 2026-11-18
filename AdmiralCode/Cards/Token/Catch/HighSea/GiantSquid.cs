using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Token.Catch.HighSea;

[Pool(typeof(TokenCardPool))]
public sealed class GiantSquid() : AdmiralCard(2, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Catch];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20, ValueProp.Move), new PowerVar<WeakPower>(3), new PowerVar<VulnerablePower>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState!).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        foreach (Creature enemy in (IEnumerable<Creature>)this.CombatState!.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, (Decimal)DynamicVars["WeakPower"].IntValue,
                this.Owner.Creature, (CardModel)this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy,
                (Decimal)DynamicVars["VulnerablePower"].IntValue, this.Owner.Creature, (CardModel)this);
        }
        if (this.IsUpgraded && CombatState?.HittableEnemies is not null)
            foreach (Creature target in this.CombatState.HittableEnemies)
                await CreatureCmd.Stun(target);
    }
}