using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ProtectedData
{
	class DPAPI
	{
		public byte[] Protect(byte[] data, byte[] s_additionalEntropy)
		{
			try
			{
				// Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted 
				//  only by the same current user. 
				return System.Security.Cryptography.ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("Data was not encrypted. An error occurred.");
				Console.WriteLine(e.ToString());
				return null;
			}
		}

		public byte[] Unprotect(byte[] data, byte[] s_additionalEntropy)
		{
			try
			{
				//Decrypt the data using DataProtectionScope.CurrentUser. 
				return System.Security.Cryptography.ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("Data was not decrypted. An error occurred.");
				Console.WriteLine(e.ToString());
				return null;
			}
		}

		public string EncryptString(byte[] secret, byte[] s_additionalEntropy)
		{
			var encrypted = Protect(secret, s_additionalEntropy);
			return System.Convert.ToBase64String(encrypted);
		}

		internal byte[] DecryptString(string encryptedBase64, byte[] s_additionalEntropy)
		{
			var encrypted = System.Convert.FromBase64String(encryptedBase64);
			return Unprotect(encrypted, s_additionalEntropy);
		}
	}
}
