namespace Autobot.Infrastructure.Auth
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}