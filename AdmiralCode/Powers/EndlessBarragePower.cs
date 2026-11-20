using Admiral.AdmiralCode.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Admiral.AdmiralCode.Powers;

public class EndlessBarragePower() : AdmiralPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
    {
        if (this.Owner.Player != null && player == this.Owner.Player)
            await AdmiralCmd.LoadCannon(choiceContext, this.Owner.Player, this, this.Amount);
    }
}