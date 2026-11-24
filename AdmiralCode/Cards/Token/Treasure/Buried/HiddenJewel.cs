using Admiral.AdmiralCode.Keywords;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Admiral.AdmiralCode.Cards.Token.Treasure.Buried;

[Pool(typeof(TokenCardPool))]
public sealed class HiddenJewel() : AdmiralCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
  protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Replay", 1M)];
  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, AdmiralKeywords.Treasure];
  protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
  protected override void OnUpgrade() => this.DynamicVars["Replay"].UpgradeValueBy(1M);
  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    List<CardModel> list1 = PileType.Discard.GetPile(this.Owner).Cards.ToList<CardModel>();
    if (list1.Count == 0)
      return;
    List<CardModel> list2 = list1.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      bool flag1 = !c.Keywords.Contains(CardKeyword.Unplayable);
      if (flag1)
      {
        bool flag2;
        switch (c.Type)
        {
          case CardType.Curse:
          case CardType.Quest:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = !flag2;
      }
      return flag1 && c.GetEnchantedReplayCount() < 1;
    })).ToList<CardModel>();
    List<CardModel> list3 = list2.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Type)
      {
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    })).ToList<CardModel>();
    CardModel? card = this.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(list3.Count == 0 ? (IEnumerable<CardModel>) list2 : (IEnumerable<CardModel>) list3);
    if (card == null)
      return;
    card.BaseReplayCount += this.DynamicVars["Replay"].IntValue;
    CardCmd.Preview(card);
  }
}
