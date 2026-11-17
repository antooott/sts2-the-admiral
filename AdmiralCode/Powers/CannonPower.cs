using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Powers;

public sealed class CannonPower : AdmiralPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
    public override bool ShouldPlayVfx => false;
    public override bool ShouldAllowHitting(Creature creature) => creature.IsAlive;
    public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (this.Owner is not { IsPet: true, Name: "Cannon" })
            await PowerCmd.Remove(this);
    }
    public override Creature ModifyUnblockedDamageTarget(
        Creature target,
        Decimal _,
        ValueProp props,
        Creature? __)
    {
        return target != this.Owner.PetOwner?.Creature || this.Owner.IsDead || !props.IsPoweredAttack() ? target : this.Owner;
    }
    public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature) => creature != this.Owner;
}