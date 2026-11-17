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
            foreach (Creature pet in player.Creature.Pets)
            {
                if (pet.HasPower<GunpowderPower>())
                    continue;
                //await CreatureCmd.Damage(choiceContext, player.Creature.CombatState!.Enemies[0], 10, ValueProp.Move, null, null);
            }
        }
    }
}

public class IsCannonAliveField {
    public static readonly SpireField<Player, bool> IsCannonAlive = new(() => false);
}