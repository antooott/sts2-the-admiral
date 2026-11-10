using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Rare;

public class DragUnder() : AdmiralCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(6), new PowerVar<DragUnderPower>(2)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
        await Cmd.Wait(0.1f);
        await PowerCmd.Apply<DragUnderPower>(choiceContext, Owner.Creature, DynamicVars["DragUnderPower"].IntValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2M);
        DynamicVars["DragUnderPower"].UpgradeValueBy(1M);
    }
}