using Identity.Infrastructure.AccessObjects;

namespace Identity.Infrastructure.MapDatabase;

internal class MapperUserDatabase
{
    internal static UserDao? Map(Dictionary<int, UserDao?> dict, UserDao userDao, TokenDao? token, int role)
    {

        if (!dict.TryGetValue(userDao.Id, out UserDao? user))
        {
            user = userDao;
            user.RefreshTokens = new List<TokenDao>();
            user.Roles = new List<int>();
            dict.Add(userDao.Id, user);
        }

        if ((user?.Roles != null) && (!user.Roles.Exists(_ => _ == role)))
        {
            user.Roles.Add(role);
        }

        if (token != null && (user?.RefreshTokens != null) && (!user.RefreshTokens.Exists(_ => _.Id == token?.Id)))
        {
            user.RefreshTokens.Add(token);
        }

        return user;
    }
}