using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace Admiral.AdmiralCode.Commands;

public static class AdmiralCmd
{
  public static async Task<Creature> Summon<T>(
    PlayerChoiceContext ctx,
    Player summoner,
    int hp,
    AbstractModel? source) where T : MonsterModel
  {
    var combatState = summoner.Creature.CombatState;
    ArgumentNullException.ThrowIfNull(combatState);
    ArgumentNullException.ThrowIfNull(summoner.PlayerCombatState);
    var existing = combatState.Allies.FirstOrDefault(c => c.Monster is T && c.PetOwner == summoner);

    var isReviving = existing is { IsAlive: false };

    if (existing is { IsAlive: true })
    {
      await CreatureCmd.GainMaxHp(existing, hp);
      return existing;
    }

    if (isReviving)
      summoner.PlayerCombatState.AddPetInternal(existing!);
    else
    {
      existing = await PlayerCmd.AddPet<T>(summoner);
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

      await PowerCmd.Apply<DieForYouPower>(ctx, existing, 1M, null, null);
      node?.TrackBlockStatus(summoner.Creature);
      node?.SetIsInteractable(true, true);
    }

    ArgumentNullException.ThrowIfNull(existing);
    await CreatureCmd.SetMaxHp(existing, hp);
    await CreatureCmd.Heal(existing, hp, isReviving);

    return existing;
  }
}