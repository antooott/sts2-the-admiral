using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Common;

public class PondFishing() : AdmiralCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4, ValueProp.Move)];
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        List<string> treasurePool = ["Wet Boot", "Guppy", "Piranha"];
        CardModel? card = CardFactory.GetDistinctForCombat(this.Owner, ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => treasurePool.Contains(c.Title))), 1, this.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
    }
}