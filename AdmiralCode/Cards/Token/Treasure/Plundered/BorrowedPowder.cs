using Admiral.AdmiralCode.Commands;
using Admiral.AdmiralCode.Keywords;
using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Plundered;

[Pool(typeof(TokenCardPool))]
public class BorrowedPowder() : AdmiralCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new LoadVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override void OnUpgrade() => this.DynamicVars["Load"].UpgradeValueBy(1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await AdmiralCmd.LoadCannon(choiceContext, this.Owner, cardPlay.Card, DynamicVars["Load"].IntValue);
    }
}