namespace Pegasus.Cryptography
{
    public static class BCryptProvider
    {
        private const int WorkFactor = 12;

        public static string HashPassword(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, WorkFactor);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
