using Admiral.AdmiralCode.Powers;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Features;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace Admiral.AdmiralCode.Monsters;

public sealed class Cannon : MonsterModel, ICustomModel
{
  private readonly int _blastDamage = 15;
  private bool HasGunPowder
  {
    get => this.IsMutable && this.Creature.HasPower<GunpowderPower>();
  }
  public static Vector2 MinOffset => new Vector2(150f, -75f);
  public static Vector2 MaxOffset => new Vector2(250f, -75f);
  public static Vector2 ScaleRange => new Vector2(1f, 2f);
  public override int MinInitialHp => 3;
  public override int MaxInitialHp => 3;
  
  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }
  
  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (!this.Creature.IsAlive || this.Creature.PetOwner == null)
      return;
    await CreatureCmd.SetMaxHp(this.Creature, 3);
    await CreatureCmd.Heal(this.Creature, 3);
    if (player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive) != null &&
        this.Creature.HasPower<GunpowderPower>())
    {
      for (int i = this.Creature.GetPowerAmount<GunpowderPower>(); i > 0; --i)
      {
        IReadOnlyList<Creature> targets = this.Creature.PetOwner!.HasPower<FragbombPower>() ? player.Creature.CombatState!.Enemies : [player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive)!];
        await FireMove(targets);
      }
    }
  }
  
  private async Task FireMove(IReadOnlyList<Creature> targets)
  {
    await DamageCmd.Attack((Decimal) _blastDamage)
      .FromMonster((MonsterModel) this)
      .TargetingFiltered(targets)
      .WithAttackerAnim("Attack", 0.2f) // no
      .WithAttackerFx(sfx: this.AttackSfx) // no
      .WithHitFx("vfx/vfx_attack_slash") // no
      .Unpowered()
      .Execute(null);
  }
}
