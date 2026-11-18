using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Cards.Status;

[Pool(typeof(StatusCardPool))]
public class Scurvy() : AdmiralCard(-1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override int MaxUpgradeLevel => 0;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FrailPower>(), HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Power", 1M)];
    public override bool HasTurnEndInHandEffect => true;
    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        int randint = this.Owner.RunState.Rng.Niche.NextInt(3);
        switch (randint)
        {
            case 0:
                bool alreadyHasFrail = this.Owner.Creature.HasPower<FrailPower>();
                PowerModel? powerModelFrail = (PowerModel?) await PowerCmd.Apply<FrailPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Power"].BaseValue, null, (CardModel) this);
                if (powerModelFrail == null || alreadyHasFrail)
                    return;
                powerModelFrail.SkipNextDurationTick = true;
                break;
            case 1:
                bool alreadyHasWeak = this.Owner.Creature.HasPower<WeakPower>();
                PowerModel? powerModelWeak = (PowerModel?) await PowerCmd.Apply<WeakPower>(choiceContext, this.Owner.Creature, this.DynamicVars["Power"].BaseValue, null, (CardModel) this);
                if (powerModelWeak == null || alreadyHasWeak)
                    return;
                powerModelWeak.SkipNextDurationTick = true;
                break;
            case 2:
                await PowerCmd.Apply<VulnerablePower>(choiceContext, this.Owner.Creature, this.DynamicVars["Power"].BaseValue, null, (CardModel) this);
                break;
        }
    }
}