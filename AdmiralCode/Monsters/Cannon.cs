#nullable enable
using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Singletons;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

namespace Admiral.AdmiralCode.Monsters;

public sealed class Cannon : MonsterModel, ICustomModel
{
  private int _shotDamage = 10;
  private bool HasGunPowder
  {
    get => this.IsMutable && this.Creature.HasPower<GunpowderPower>();
  }
  public const float attackerAnimDelay = 0.3f;
  //public const string pokeAnim = "attack_poke";
  //public const string ostyAttackSfx = "event:/sfx/characters/osty/osty_attack";
  public static Vector2 MinOffset => new Vector2(150f, -75f);
  public static Vector2 MaxOffset => new Vector2(250f, -75f);
  public static Vector2 ScaleRange => new Vector2(1f, 2f);
  public override int MinInitialHp => 3;
  public override int MaxInitialHp => 3;
  //public override string DeathSfx => "event:/sfx/characters/osty/osty_die";
  //public override bool HasDeathSfx => true;

  /*
  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    MoveState initialState = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    initialState.FollowUpState = (MonsterState) initialState;
    List<MonsterState> states = new List<MonsterState>();
    states.Add(initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }
  */
  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState move0 = new MoveState("NOTHING_MOVE", (Func<IReadOnlyList<Creature>, Task>) (_ => Task.CompletedTask), Array.Empty<AbstractIntent>());
    MoveState move1 = new MoveState("CANNON_SHOT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.CannonShotMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this._shotDamage)
    });
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    if (this.HasGunPowder)
    {
      initialState.AddState((MonsterState) move1, (Func<bool>) (() => ((Cannon) this.Creature.Monster!).HasGunPowder));
    }
    else
    {
      initialState.AddState((MonsterState) move0, (Func<bool>) (() => !((Cannon) this.Creature.Monster!).HasGunPowder));
    }
    move0.FollowUpState = this.HasGunPowder ?  (MonsterState) move1 : (MonsterState) move0;
    move1.FollowUpState = this.HasGunPowder ?  (MonsterState) move1 : (MonsterState) move0;
    states.Add((MonsterState) move0);
    states.Add((MonsterState) move1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }
  
  private async Task CannonShotMove(IReadOnlyList<Creature> targets)
  {
    await DamageCmd.Attack((Decimal)this._shotDamage).TargetingAllOpponents(CombatState)
      .WithHitFx(VfxCmd.slashPath)
      .Execute(null);
    //await DamageCmd.Attack((Decimal) this._shotDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute(null);
  }
  
  public override Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    CardModel? source,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (this.Creature.PetOwner != null)
      IsCannonAliveField.IsCannonAlive[this.Creature.PetOwner] = false;
    return Task.CompletedTask;
  }
}
