using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Common.Skill;

public class Recovery() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("TreasureVar",1)];
    protected override void OnUpgrade() => this.DynamicVars["TreasureVar"].UpgradeValueBy(1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    private List<string> _treasurePool = ["Ancient Ruins", "Shiny Badge", "Broken Compass", "Rhystic Scimitar", "Giant Shell"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        Decimal count = this.DynamicVars["TreasureVar"].BaseValue;
        for (; count > 0; --count)
        {
            CardModel? card = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => _treasurePool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (card == null)
                return;
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
        }
    }
}