using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Powers;

public class CurrentPower : AdmiralPower
{

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer == null || dealer != this.Owner && dealer.PetOwner?.Creature != this.Owner || !props.IsPoweredAttack_())
            return;
        await PowerCmd.Apply<RiptidePower>(choiceContext, target, (Decimal) (this.Amount), this.Owner, null);
        await PowerCmd.Remove((PowerModel)this);
    }
}