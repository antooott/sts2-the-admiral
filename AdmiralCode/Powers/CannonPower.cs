using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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
        {
            await PowerCmd.Remove(this);
        }
    }
    public override Creature ModifyUnblockedDamageTarget(
        Creature target,
        Decimal _,
        ValueProp props,
        Creature? __)
    {
        return target != this.Owner.PetOwner?.Creature || this.Owner.IsDead || !props.IsPoweredAttack() ? target : this.Owner;
    }
    public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
    {
        return creature != this.Owner;
    }
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains<Creature>(this.Owner) && this.Owner.IsAlive)
        {
            await CreatureCmd.SetMaxHp(this.Owner, 3);
            await CreatureCmd.Heal(this.Owner, 3);
        }
    }
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive) != null && this.Owner.HasPower<GunpowderPower>())
            await CreatureCmd.Damage(choiceContext, player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive)!, 10, ValueProp.Move, null, null);
    }
}