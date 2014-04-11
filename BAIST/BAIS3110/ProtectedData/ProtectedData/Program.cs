using System;
using System.Security.Cryptography;

namespace ProtectedData
{
	public class DataProtectionSample
	{
		// Create byte array for additional entropy when using Protect method. 
		static byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };

		public static void Main()
		{
			// Create a simple byte array containing data to be encrypted. 

			byte[] secret = { 0, 1, 2, 3, 4, 1, 2, 3, 4 };

			DPAPI dpapi = new DPAPI();

			Step4(secret, dpapi);

			Step5(secret, dpapi);

			Step6();

            Step7();

			Console.ReadLine();
		}

		private static void Step4(byte[] secret, DPAPI dpapi)
		{
			Console.WriteLine();
			Console.WriteLine(" ====================== Step 4 ====================== ");
			Console.WriteLine();

			//Encrypt the data. 
			byte[] encryptedSecret = dpapi.Protect(secret, s_additionalEntropy);
			Console.WriteLine("The encrypted byte array is:");
			PrintValues(encryptedSecret);

			// Decrypt the data and store in a byte array. 
			byte[] originalData = dpapi.Unprotect(encryptedSecret, s_additionalEntropy);
			Console.WriteLine("{0}The original data is:", Environment.NewLine);
			PrintValues(originalData);
		}

		private static void Step5(byte[] secret, DPAPI dpapi)
		{
			Console.WriteLine();
			Console.WriteLine(" ====================== Step 5 ====================== ");
			Console.WriteLine();

			string encryptedBase64 = dpapi.EncryptString(secret, s_additionalEntropy);
			Console.WriteLine("Encrypted string: " + encryptedBase64);
			Console.WriteLine();

			byte[] originalBase64 = dpapi.DecryptString(encryptedBase64, s_additionalEntropy);
			Console.WriteLine("Original data: ");
			PrintValues(originalBase64);
			Console.WriteLine();
		}

		private static void Step6()
		{
			byte[] numbers = new byte[5];
			Console.WriteLine();
			Console.WriteLine(" ====================== Step 6 ====================== ");
			Console.WriteLine();

			Console.Write("Random class: ");
			var random = new Random();
			random.NextBytes(numbers);
			PrintValues(numbers);
			Console.WriteLine();

			Console.Write("RNGCryptoServiceProvider: ");
			var crypto = new RNGCryptoServiceProvider();
			crypto.GetBytes(numbers);
			PrintValues(numbers);
			Console.WriteLine();
		}

        private static void Step7()
        {
			Console.WriteLine();
			Console.WriteLine(" ====================== Step 7 ====================== ");
			Console.WriteLine();

			var helloWorld = GetBytes("Hello world");

            var md5 = HashAlgorithm.Create("MD5");
            var md5hash = md5.ComputeHash(helloWorld);
            Console.WriteLine("MD5: " + Convert.ToBase64String(md5hash));

            var sha1 = HashAlgorithm.Create("SHA1");
            var sha1hash = sha1.ComputeHash(helloWorld);
            Console.WriteLine("SHA1: " + Convert.ToBase64String(sha1hash));

            var sha512 = HashAlgorithm.Create("SHA512");
            var sha512hash = sha512.ComputeHash(helloWorld);
            Console.WriteLine("SHA512: " + Convert.ToBase64String(sha512hash));

            Console.WriteLine();
        }

        public static void PrintValues(Byte[] myArr)
		{
			foreach (Byte i in myArr)
			{
				Console.Write("\t{0}", i);
			}
			Console.WriteLine();
		}

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
	}
}
