namespace Scripts.Pieces.Interfaces
{
    public interface IAnimatable
    {
        public void DyingAnimation();
    
        public void AttackAnimation();

        public void MoveAnimation(float timeToMove);
        
        public void PainAnimation();
    

    }
}
