using Scripts.Pieces.Interfaces;

namespace Scripts.Pieces
{
    public static class PieceDamageModificator
    {
        public static double GetModificatorForPiece(IPiece attacker, IPiece defender)
        {

            var applyDmg =  (attacker is Mage    && defender is Paladin) ||
                            (attacker is Paladin && defender is Warrior) ||
                            (attacker is Warrior && defender is Archer)  ||
                            (attacker is Archer  && defender is Mage); 
            
            return applyDmg ? 1.5f : 1f;
        }
    }
}