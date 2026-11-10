using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Powers;

public sealed class RiptidePower : AdmiralPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
    private Boolean _tookAttackDamage = false;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        if (_tookAttackDamage)
        {
            await this.Trigger();
        }
        _tookAttackDamage = false;
    }
    
    public async Task Trigger()
    {
        await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, (Decimal) this.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        if (this.Owner.IsAlive)
            await Cmd.CustomScaledWait(0.1f, 0.25f);
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (result.TotalDamage > 0 && CombatState.CurrentSide == CombatSide.Player)
        {
            _tookAttackDamage = true;
        }
        return Task.CompletedTask;
    }
}