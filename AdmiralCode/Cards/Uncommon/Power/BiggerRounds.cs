using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Admiral.AdmiralCode.Cards.Uncommon.Power;

public sealed class BiggerRounds() : AdmiralCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BiggerRoundsPower>(5M)];
    protected override void OnUpgrade() => this.DynamicVars["BiggerRoundsPower"].UpgradeValueBy(2M);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BiggerRoundsPower>(choiceContext, Owner.Creature, DynamicVars["BiggerRoundsPower"].IntValue, Owner.Creature, this);
    }
}