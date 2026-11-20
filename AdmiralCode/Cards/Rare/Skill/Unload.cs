using Admiral.AdmiralCode.Commands;
using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Admiral.AdmiralCode.Cards.Rare.Skill;

public sealed class Unload() : AdmiralCard(1, CardType.Skill, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LoadVar(1)];
    protected override void OnUpgrade() => DynamicVars["Load"].UpgradeValueBy(1);
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Creature cannon = await AdmiralCmd.LoadCannon(choiceContext, this.Owner, cardPlay.Card, DynamicVars["Load"].IntValue);
        if (DynamicVars["Load"].IntValue > 0)
            await cannon.Monster!.AfterPlayerTurnStart(choiceContext, cardPlay.Player);
        await PowerCmd.Remove<GunpowderPower>(cannon);
    }
}