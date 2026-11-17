using Admiral.AdmiralCode.Monsters;
using Admiral.AdmiralCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Admiral.AdmiralCode.Commands;

public static class AdmiralCmd
{
  public static async Task<Creature> SummonCannon(
    PlayerChoiceContext ctx,
    Player summoner,
    AbstractModel? source,
    int gunpowderAmount)
  {
    var combatState = summoner.Creature.CombatState;
    ArgumentNullException.ThrowIfNull(combatState);
    ArgumentNullException.ThrowIfNull(summoner.PlayerCombatState);
    var existing = combatState.Allies.FirstOrDefault(c => c.Monster is Cannon && c.PetOwner == summoner);

    var isReviving = existing is { IsAlive: false };

    if (existing is { IsAlive: true })
    {
      // await CreatureCmd.SetMaxAndCurrentHp(existing, 3);
      await PowerCmd.Apply<GunpowderPower>(ctx, existing, gunpowderAmount, null, null);
      return existing;
    }

    if (isReviving)
    {
      // has died this combat
      summoner.PlayerCombatState.AddPetInternal(existing!);
      ArgumentNullException.ThrowIfNull(existing);
      await CreatureCmd.SetMaxHp(existing, 3);
      await CreatureCmd.Heal(existing, 3, isReviving);
      await PowerCmd.Apply<GunpowderPower>(ctx, existing!, gunpowderAmount, null, null);
      return existing;
    }
    else
    {
      // non-existent; create
      existing = await PlayerCmd.AddPet<Cannon>(summoner);
      var node = NCombatRoom.Instance?.GetCreatureNode(existing);
      var playerNode = NCombatRoom.Instance?.GetCreatureNode(summoner.Creature);

      if (node != null && source is CardModel && playerNode != null)
      {
        node.Position = playerNode.Position + new Vector2(250f, -75f);
        node.Modulate = Colors.Transparent;
        node.CreateTween()
          .TweenProperty(node, "modulate", Colors.White, 0.35)
          .SetDelay(0.1);
      }
      await PowerCmd.Apply<CannonPower>(ctx, existing, 1M, null, null);
      await PowerCmd.Apply<GunpowderPower>(ctx, existing, gunpowderAmount, null, null);
      node?.TrackBlockStatus(summoner.Creature);
      node?.SetIsInteractable(true, true);
    }

    ArgumentNullException.ThrowIfNull(existing);
    await CreatureCmd.SetMaxHp(existing, 3);
    await CreatureCmd.Heal(existing, 3, isReviving);

    return existing;
  }
}