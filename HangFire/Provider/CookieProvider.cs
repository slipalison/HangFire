using Microsoft.Owin.Security.Cookies;

namespace HangFire.Provider
{
    public class CookieProvider : CookieAuthenticationProvider
    {
        public CookieProvider()
        {
            OnResponseSignIn = SignIn;
        }

        private void SignIn(CookieResponseSignInContext obj)
        {
            if (obj.Properties.IssuedUtc.HasValue)
            {
                obj.Properties.ExpiresUtc = obj.Properties.IssuedUtc.Value.Add(obj.Options.ExpireTimeSpan);
            }
        }
    }
}