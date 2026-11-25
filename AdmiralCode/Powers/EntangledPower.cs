using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Powers;

public sealed class EntangledPower : AdmiralPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
    private bool _hasAttacked = false;

    public override async Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != this.Owner)
            return;
        this.Flash();
        await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, (Decimal) this.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        _hasAttacked = true;
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        if (_hasAttacked)
        {
            _hasAttacked = false;
            return;
        }
        await PowerCmd.Decrement(this);
    }
}