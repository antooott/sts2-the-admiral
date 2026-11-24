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

namespace Admiral.AdmiralCode.Cards.Uncommon.Skill;

public class TreasureMap() : AdmiralCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("TreasureVar",1), new CardsVar(3)];
    protected override void OnUpgrade() => this.DynamicVars["TreasureVar"].UpgradeValueBy(1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    private readonly List<string> _treasurePool = ["Hidden Jewel", "Doubloons", "Old Rum"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        IEnumerable<CardModel> cardOptions = PileType.Draw.GetPile(this.Owner).Cards.ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).Take<CardModel>(this.DynamicVars.Cards.IntValue);
        CardModel? card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(this.Owner), this.Owner, new CardSelectorPrefs(this.SelectionScreenPrompt, 1), (Func<CardModel, bool>) (c => cardOptions.Contains<CardModel>(c)))).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Hand);
        Decimal count = this.DynamicVars["TreasureVar"].BaseValue;
        for (; count > 0; --count)
        {
            CardModel? treasure = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => _treasurePool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
            if (treasure == null)
                return;
            await CardPileCmd.AddGeneratedCardToCombat(treasure, PileType.Hand, this.Owner);
        }
    }
}