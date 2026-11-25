using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Commercial;

[Pool(typeof(TokenCardPool))]
public sealed class Arms() : AdmiralCard(-1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override bool HasEnergyCostX => true;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int count = this.ResolveEnergyXValue();
        if (this.IsUpgraded)
            ++count;
        for (int i = 0; i < count; ++i)
        {
            CardModel? card = PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.Keywords.Contains(CardKeyword.Unplayable) && c.Type == CardType.Attack)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).FirstOrDefault<CardModel>() ?? PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).FirstOrDefault<CardModel>();
            if (card != null)
                await CardCmd.AutoPlay(choiceContext, card, null);
        }
    }
}