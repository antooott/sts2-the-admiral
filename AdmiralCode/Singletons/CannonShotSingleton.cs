using Admiral.AdmiralCode.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Singletons;

public class CannonShotSingleton() : CustomSingletonModel(HookType.Combat)
{
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (IsCannonAliveField.IsCannonAlive[player])
        {
            await PlayerCmd.GainEnergy(1, player);
            foreach (Creature pet in player.Creature.Pets)
            {
                await PlayerCmd.GainEnergy(1, player);
                if (pet.HasPower<GunpowderPower>())
                {
                    await PlayerCmd.GainEnergy(1, player);
                    await CreatureCmd.Damage(choiceContext, player.Creature.CombatState!.Enemies[0], 10, ValueProp.Move, null, null);
                }
            }
        }
    }
}

public class IsCannonAliveField {
    public static readonly SpireField<Player, bool> IsCannonAlive = new(() => false);
}