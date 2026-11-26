using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Common.Skill;

public sealed class TradePort() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    private List<string> _treasurePool = ["Arms", "Provisions", "Powder Keg"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? exhaustTarget = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, (AbstractModel) this)).FirstOrDefault<CardModel>();
        if (exhaustTarget == null)
            return;
        int count = exhaustTarget.EnergyCost.GetWithModifiers(CostModifiers.All);
        await CardCmd.Exhaust(choiceContext, exhaustTarget);
        for (; count > 0; --count)
        {
            CardModel? treasure = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => _treasurePool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (treasure == null)
                return;
            await CardPileCmd.AddGeneratedCardToCombat(treasure, PileType.Hand, this.Owner);
        }
    }
}