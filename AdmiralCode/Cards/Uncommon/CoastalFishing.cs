using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Uncommon;

public class CoastalFishing() : AdmiralCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Catch)];
    private readonly List<string> _treasurePool = ["Jellyfish", "Seabass", "Lemon Grove"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int count = Math.Max(0, Math.Min(this.DynamicVars.Cards.IntValue, CardPile.MaxCardsInHand - PileType.Hand.GetPile(this.Owner).Cards.Count));
        await CardPileCmd.Add((IEnumerable<CardModel>) PileType.Discard.GetPile(this.Owner).Cards.TakeRandom<CardModel>(count, this.Owner.RunState.Rng.CombatCardSelection).ToList<CardModel>(), PileType.Hand);
        CardModel? catchCard = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => this._treasurePool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
        if (catchCard == null)
            return;
        await CardPileCmd.AddGeneratedCardToCombat(catchCard, PileType.Hand, this.Owner);
    }
}