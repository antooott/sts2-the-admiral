using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public sealed class TestOfWill() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7, ValueProp.Move), new CardsVar(1)];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
    protected override bool ShouldGlowGoldInternal => this.Owner.Creature.HasPower<VigorPower>();
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        bool hasVigor = this.Owner.Creature.HasPower<VigorPower>();
        if (hasVigor)
            await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    }
}