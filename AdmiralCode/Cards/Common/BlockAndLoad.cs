using Admiral.AdmiralCode.Commands;
using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class BlockAndLoad() : AdmiralCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8, ValueProp.Move), new LoadVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        await AdmiralCmd.LoadCannon(choiceContext, this.Owner, cardPlay.Card, DynamicVars["Load"].IntValue);
    }
}