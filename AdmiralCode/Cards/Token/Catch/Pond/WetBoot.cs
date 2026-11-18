using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Catch.Pond;

[Pool(typeof(TokenCardPool))]
public sealed class WetBoot() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Catch];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}