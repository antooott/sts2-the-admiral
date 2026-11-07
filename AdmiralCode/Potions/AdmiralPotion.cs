using BaseLib.Abstracts;
using BaseLib.Utils;
using Admiral.AdmiralCode.Character;

namespace Admiral.AdmiralCode.Potions;

[Pool(typeof(AdmiralPotionPool))]
public abstract class AdmiralPotion : CustomPotionModel;