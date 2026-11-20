using Admiral.AdmiralCode.Cards.Token.Treasure.Sunken;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Admiral.AdmiralCode.Powers;

public class ShinyBadgePower : TemporaryDexterityPower, ICustomModel
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<ShinyBadge>();
}