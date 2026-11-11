using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Admiral.AdmiralCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class WetBoot() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    public static async Task<CardModel?> CreateInHand(
        Player owner,
        ICombatState combatState,
        Player? creator = null)
    {
        return (await Shiv.CreateInHand(owner, 1, combatState, creator)).FirstOrDefault<CardModel>();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState,
        Player? creator = null)
    {
        if (count == 0)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return (IEnumerable<CardModel>) Array.Empty<CardModel>();
        List<CardModel> tokens = new List<CardModel>();
        for (int index = 0; index < count; ++index)
            tokens.Add((CardModel) combatState.CreateCard<Shiv>(owner));
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) tokens, PileType.Hand, creator ?? owner);
        return (IEnumerable<CardModel>) tokens;
    }
}