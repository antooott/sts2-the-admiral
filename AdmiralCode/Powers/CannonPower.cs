using Admiral.AdmiralCode.Monsters;
using Admiral.AdmiralCode.Singletons;
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
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (this.Owner is not { IsPet: true, Name: "Cannon" })
        {
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        await CreatureCmd.SetMaxAndCurrentHp(this.Owner, 3);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive) != null && this.Owner.HasPower<GunpowderPower>())
            await CreatureCmd.Damage(choiceContext, player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive)!, 10, ValueProp.Move, null, null);
    }
}