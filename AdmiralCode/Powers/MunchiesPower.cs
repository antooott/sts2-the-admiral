using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Admiral.AdmiralCode.Powers;

public class MunchiesPower : AdmiralPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => (object) new MunchiesPower.Data();
    
    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.Owner.Player != null && cardPlay.Card.Owner.Creature == this.Owner && cardPlay.Card.Keywords.Contains(AdmiralKeywords.Catch) &&
            this.GetInternalData<MunchiesPower.Data>().CatchesPlayedThisTurn < this.Amount)
        {
            PlayerCmd.GainEnergy(1, this.Owner.Player);
            ++this.GetInternalData<MunchiesPower.Data>().CatchesPlayedThisTurn;
        }
        return Task.CompletedTask;
    }
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return Task.CompletedTask;
        this.GetInternalData<MunchiesPower.Data>().CatchesPlayedThisTurn = 0;
        return Task.CompletedTask;
    }
    
    private class Data
    {
        public int CatchesPlayedThisTurn;
    }
}