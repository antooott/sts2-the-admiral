using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Uncommon;

public class CitrusSnack() : AdmiralCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> list = CitrusSnack.GetStatuses(this.Owner).ToList<CardModel>();
        foreach (CardModel card in list)
        {
            await CardCmd.Exhaust(choiceContext, card);
        }
        if (this.IsUpgraded)
            await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    }
    private static IEnumerable<CardModel> GetStatuses(Player owner)
    {
        return owner.PlayerCombatState!.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Status && c.Pile!.Type != PileType.Exhaust));
    }
}