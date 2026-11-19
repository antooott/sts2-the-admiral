using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Plundered;

[Pool(typeof(TokenCardPool))]
public sealed class RestolenGoods() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? card;
        
        List<CardPoolModel> list1 = this.Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
        if (list1.Count > 1)
            list1.Remove(this.Owner.Character.CardPool);
        List<CardModel> list2 = CardFactory.GetDistinctForCombat(this.Owner, list1.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (c => c.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint))), 3, this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) list2, this.Owner, true);
        if (card == null)
            return;
        card.SetToFreeThisTurn();
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
    }
}