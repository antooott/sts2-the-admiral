using Admiral.AdmiralCode.Powers;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Monsters;

public sealed class Cannon : MonsterModel, ICustomModel
{
  //public const float attackerAnimDelay = 0.3f;
  //public const string pokeAnim = "attack_poke";
  //public const string ostyAttackSfx = "event:/sfx/characters/osty/osty_attack";
  public static Vector2 MinOffset => new Vector2(150f, -75f);
  public static Vector2 MaxOffset => new Vector2(250f, -75f);
  public static Vector2 ScaleRange => new Vector2(1f, 2f);
  public override int MinInitialHp => 3;
  public override int MaxInitialHp => 3;
  //public override string DeathSfx => "event:/sfx/characters/osty/osty_die";
  //public override bool HasDeathSfx => true;
  
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
    if (!this.Creature.IsAlive)
      return;
    await CreatureCmd.SetMaxHp(this.Creature, 3);
    await CreatureCmd.Heal(this.Creature, 3);
    if (player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive) != null && this.Creature.HasPower<GunpowderPower>())
      await CreatureCmd.Damage(choiceContext, player.Creature.CombatState!.Enemies.FirstOrDefault(c => c.IsAlive)!, 10, ValueProp.Move, null, null);
  }
}
