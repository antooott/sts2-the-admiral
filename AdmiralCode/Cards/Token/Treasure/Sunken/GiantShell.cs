using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Sunken;

[Pool(typeof(TokenCardPool))]
public sealed class GiantShell() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(13M, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(5);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
}