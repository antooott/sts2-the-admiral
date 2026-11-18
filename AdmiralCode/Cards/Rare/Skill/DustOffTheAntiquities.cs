using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Admiral.AdmiralCode.Cards.Rare.Skill;

public class DustOffTheAntiquities() : AdmiralCard(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        foreach (CardModel card in this.Owner.PlayerCombatState!.ExhaustPile.Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Keywords.Contains(AdmiralKeywords.Treasure) && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList<CardModel>())
            await CardCmd.AutoPlay(choiceContext, card, null);
    }
}