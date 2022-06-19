using Scripts.Pieces.Interfaces;

namespace Scripts.Pieces
{
    public static class PieceDamageModificator
    {
        public static double GetModificatorForPiece(IPiece attacker, IPiece defender)
        {

            var attackerType = attacker.GetType();
            var defenderType = defender.GetType();
            
            var applyDmg =  (attackerType == typeof(Mage)    && defenderType == typeof(Paladin)) ||
                            (attackerType == typeof(Paladin) && defenderType == typeof(Warrior)) ||
                            (attackerType == typeof(Warrior) && defenderType == typeof(Archer))  ||
                            (attackerType == typeof(Archer)  && defenderType == typeof(Mage)); 
            
            return applyDmg ? 0f : 1.5f;
        }
    }
}