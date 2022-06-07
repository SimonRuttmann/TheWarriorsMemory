namespace Scripts.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string source, string target)
        {
            return source.ToUpper().Equals(target.ToUpper());
        }
    }
}