using Admiral.AdmiralCode.Keywords;
using Admiral.AdmiralCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Occult;

[Pool(typeof(TokenCardPool))]
public sealed class Hexatorch() : AdmiralCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BurnPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BurnPower>()];
    protected override void OnUpgrade() => DynamicVars["BurnPower"].UpgradeValueBy(2M);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        for (int i = 6; i > 0; --i)
            await PowerCmd.Apply<BurnPower>(choiceContext, cardPlay.Target, this.DynamicVars["BurnPower"].BaseValue, this.Owner.Creature, (CardModel) this);
    }
}