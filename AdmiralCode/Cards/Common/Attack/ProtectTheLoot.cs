using Admiral.AdmiralCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common.Attack;

public sealed class ProtectTheLoot() : AdmiralCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move), new BlockVar(5, ValueProp.Move)];
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(AdmiralKeywords.Prosper), HoverTipFactory.FromKeyword(AdmiralKeywords.Treasure)];
    private bool HasPlayedTreasureThisCombat => CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Any(c => c.CardPlay.Player == this.Owner && c.CardPlay.Card.Keywords.Contains(AdmiralKeywords.Treasure));
    protected override bool ShouldGlowGoldInternal => HasPlayedTreasureThisCombat;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
        if (HasPlayedTreasureThisCombat)
            await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
}