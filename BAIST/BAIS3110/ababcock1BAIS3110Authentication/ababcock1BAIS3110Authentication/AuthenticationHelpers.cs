using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using ababcock1BAIS3110Authentication.DataAccessLayer;

namespace ababcock1BAIS3110Authentication
{
	public class AuthenticationHelpers
	{
		public static string CreateSalt(int size)
		{
			var rng = new RNGCryptoServiceProvider();
			var buff = new byte[size];
			rng.GetBytes(buff);
			return Convert.ToBase64String(buff);
		}

		public static string CreatePasswordHash(string pwd, string salt)
		{
			var saltAndPwd = String.Concat(pwd, salt);
			var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "SHA1");
			return hashedPwd;
		}

		public static HttpCookie GetAuthCookie(UserInfo userInfo, bool persist)
		{
			var authTicket = new FormsAuthenticationTicket(
				2, userInfo.email, DateTime.Now, DateTime.Now.AddMinutes(60), persist, userInfo.roles);
			var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
			return new HttpCookie(
				"Roles", encryptedTicket);
		}

		public static CustomPrincipal GetPrincipalFromRequest(HttpContext context)
		{
			string cookieName = "Roles";
			HttpCookie authCookie = context.Request.Cookies[cookieName];

			if (authCookie == null)
			{
				return null;
			}

			FormsAuthenticationTicket authTicket = null;
			try
			{
				authTicket = FormsAuthentication.Decrypt(authCookie.Value);
			}
			catch (Exception ex)
			{
				return null;
			}

			if (authTicket == null)
			{
				return null;
			}

			string[] roles = authTicket.UserData.Split('|');

			FormsIdentity id = new FormsIdentity(authTicket);
			return new CustomPrincipal(id, roles);
		}
	}
}