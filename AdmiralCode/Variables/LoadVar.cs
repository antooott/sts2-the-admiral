using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Admiral.AdmiralCode.Variables;

public class LoadVar : DynamicVar
{
    public const string Key = "Load";

    public LoadVar(decimal loadCount) : base(Key, loadCount)
    {
        this.WithTooltip();
    }
}