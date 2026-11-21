using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Occult;

[Pool(typeof(TokenCardPool))]
public sealed class SneckoFang() : AdmiralCard(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new CardsVar(2)];
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(4);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx(VfxCmd.slashPath)
            .Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => !c.EnergyCost.CostsX)))
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                card.EnergyCost.SetThisTurnOrUntilPlayed(this.NextEnergyCost());
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
    }
    private int _testEnergyCostOverride = -1;
    public int TestEnergyCostOverride
    {
        get => this._testEnergyCostOverride;
        set
        {
            TestMode.AssertOn();
            this.AssertMutable();
            this._testEnergyCostOverride = value;
        }
    }
    private int NextEnergyCost()
    {
        return this.TestEnergyCostOverride >= 0 ? this.TestEnergyCostOverride : this.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }
}