using Admiral.AdmiralCode.Cards.Status;
using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Explored;

[Pool(typeof(TokenCardPool))]
public sealed class FruitFruition() : AdmiralCard(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain, AdmiralKeywords.Treasure,];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Scurvy>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.IsUpgraded)
        {
            List<CardModel> list1 = PileType.Hand.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Title == "Scurvy" && c.Rarity == CardRarity.Token)).ToList<CardModel>();
            foreach (CardModel card in list1)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
        else
        {
            CardModel? card = this.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) PileType.Hand.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Title == "Scurvy")));
            if (card == null)
                return;
            await CardCmd.Exhaust(choiceContext, card);
        }
    }
}