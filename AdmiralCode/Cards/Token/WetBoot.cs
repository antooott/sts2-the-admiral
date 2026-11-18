using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Admiral.AdmiralCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public sealed class WetBoot() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Catch];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}