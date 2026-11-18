using Admiral.AdmiralCode.Keywords;
using Admiral.AdmiralCode.Powers;
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

public sealed class CoastalFishing() : AdmiralCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    private bool HasBetterBait => this.IsMutable  && this.Owner.Creature.HasPower<BetterBaitPower>();
    protected override bool ShouldGlowGoldInternal => this.HasBetterBait;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Catch)];
    private readonly List<string> _catchPool = ["Jellyfish", "Seabass", "Lemon Grove"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int count = Math.Max(0, Math.Min(this.DynamicVars.Cards.IntValue, CardPile.MaxCardsInHand - PileType.Hand.GetPile(this.Owner).Cards.Count));
        await CardPileCmd.Add((IEnumerable<CardModel>) PileType.Discard.GetPile(this.Owner).Cards.TakeRandom<CardModel>(count, this.Owner.RunState.Rng.CombatCardSelection).ToList<CardModel>(), PileType.Hand);
        if (this.HasBetterBait)
        {
            foreach (var catchCard in this._catchPool)
            {
                CardModel? card = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Title == catchCard)), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
                if (card != null)
                    await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
            }
        }
        else
        {
            CardModel? card = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => _catchPool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (card == null)
                return;
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
        }
    }
}