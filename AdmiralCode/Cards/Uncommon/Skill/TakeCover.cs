using Admiral.AdmiralCode.Commands;
using Admiral.AdmiralCode.Powers;
using Admiral.AdmiralCode.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Uncommon.Skill;

public class TakeCover() : AdmiralCard(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(10, ValueProp.Move), new LoadVar(2)];
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(4);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<CannonPower>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.CombatState is null)
            return;
        int count = this.CombatState.Enemies.Count<Creature>((Func<Creature, bool>)(e => e.Monster != null && e.Monster.IntendsToAttack));
        for (; count > 0; --count)
            await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        await AdmiralCmd.LoadCannon(choiceContext, this.Owner, cardPlay.Card, DynamicVars["Load"].IntValue);
    }
}
