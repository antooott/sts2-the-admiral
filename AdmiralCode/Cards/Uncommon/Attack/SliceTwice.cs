using Admiral.AdmiralCode.Cards.Status;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Uncommon.Attack;

public sealed class SliceTwice() : AdmiralCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Scurvy>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // decomp game code has ThrowIfNull((object) cardPlay.Target, "cardPlay.Target")
        ArgumentNullException.ThrowIfNull(cardPlay.Target); 
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .WithHitCount(2)
            // ~
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            // taken from Strike template, decomp game code has string
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat((CardModel) this.CombatState!.CreateCard<Scurvy>(this.Owner), PileType.Discard, this.Owner));
        
    }
}