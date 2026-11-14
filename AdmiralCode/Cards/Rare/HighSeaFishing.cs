using Admiral.AdmiralCode.Cards.Status;
using Admiral.AdmiralCode.Keywords;
using Admiral.AdmiralCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Rare;

public class HighSeaFishing() : AdmiralCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    private bool HasBetterBait => this.IsMutable  && this.Owner.Creature.HasPower<BetterBaitPower>();
    protected override bool ShouldGlowGoldInternal => this.HasBetterBait;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Scurvy", 2)];
    protected override void OnUpgrade() => this.DynamicVars["Scurvy"].UpgradeValueBy(-1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Catch)];
    private List<string> _catchPool = ["Swordfish", "Whale", "Giant Squid"];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int scurvyCount = this.DynamicVars["Scurvy"].IntValue;
        for(; scurvyCount >= 0; scurvyCount--)
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat((CardModel) this.CombatState!.CreateCard<Scurvy>(this.Owner), PileType.Discard, this.Owner));
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
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