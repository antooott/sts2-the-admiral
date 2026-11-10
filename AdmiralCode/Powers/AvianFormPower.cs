using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Powers;

public class AvianFormPower() : AdmiralPower, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageDecrease", 0.5M)];
    public override Decimal ModifyDamageMultiplicative(
        Creature? target,
        Decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (target != this.Owner || !props.IsPoweredAttack())
            return 1M;
        Decimal amount1 = this.DynamicVars["DamageDecrease"].BaseValue;
        return amount1;
    }
    
    public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
    {
        return (int) Math.Max(Math.Floor(currentMaxHandSize*0.5), 1);
    }
}