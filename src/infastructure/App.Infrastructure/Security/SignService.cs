using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.Infrastructure.Security;

public static class SignService
{
    // TOKEN İMZALAMA İŞLEMLERİNDE KULLANMAK İÇİN SYMETRIC KEY OLUŞTURUYORUZ.
    public static SecurityKey GetSymetricKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}