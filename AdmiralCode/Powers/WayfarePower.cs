using Admiral.AdmiralCode.Cards.Status;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Powers;

public class WayfarePower() : AdmiralPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != this.Owner.Player)
            return;
        int count = this.Amount;
        for (; count > 0; count--)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat((CardModel) this.CombatState!.CreateCard<Scurvy>(this.Owner.Player!), PileType.Discard, this.Owner.Player));
        await PowerCmd.Remove((PowerModel) this);
    }
}
