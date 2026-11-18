using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Token.Catch.HighSea;

[Pool(typeof(TokenCardPool))]
public sealed class Whale() : AdmiralCard(3, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Catch];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(60, ValueProp.Move), new DynamicVar(nameof (Blur), 1M)];
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(40);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<BlurPower>(choiceContext, this.Owner.Creature, this.DynamicVars[nameof (Blur)].BaseValue, this.Owner.Creature, (CardModel) this);
    }
}