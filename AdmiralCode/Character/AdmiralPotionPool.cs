using BaseLib.Abstracts;
using Admiral.AdmiralCode.Extensions;
using Godot;

namespace Admiral.AdmiralCode.Character;

public class AdmiralPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Admiral.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}