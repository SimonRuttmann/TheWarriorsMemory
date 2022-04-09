namespace Scripts.Extensions
{
    public static class IntegerExtensions
    {
        public static bool IsOdd(this int a){
            return (a % 2) != 1;
        }

    }
}