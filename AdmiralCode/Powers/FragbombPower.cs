using MegaCrit.Sts2.Core.Entities.Powers;

namespace Admiral.AdmiralCode.Powers;

public sealed class FragbombPower : AdmiralPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}
