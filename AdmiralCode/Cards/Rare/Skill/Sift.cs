using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Rare.Skill;

public class Sift() : AdmiralCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new EnergyVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
    private bool HasPlayedTreasureThisCombat => CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Any(c => c.CardPlay.Player == this.Owner && c.CardPlay.Card.Keywords.Contains(AdmiralKeywords.Treasure));
    protected override bool ShouldGlowGoldInternal => HasPlayedTreasureThisCombat;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
        CardModel? exhaustTarget = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, (AbstractModel) this)).FirstOrDefault<CardModel>();
        if (exhaustTarget != null)
            await CardCmd.Exhaust(choiceContext, exhaustTarget);
        if (HasPlayedTreasureThisCombat)
            await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }
}