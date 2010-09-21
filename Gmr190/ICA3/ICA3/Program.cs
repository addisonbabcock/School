﻿using System;

namespace ICA3
{
	class Program
	{
		static void Main(string[] args)
		{
			double HoursWorked = 0.0;
			double PayRate = 0.0;
			double GrossPay = 0.0;
			double Tax = 0.0;
			double NetPay = 0.0;

			try
			{
				Console.Write("Please enter the hours worked: ");
				HoursWorked = Convert.ToDouble(Console.ReadLine());

				Console.Write("Please enter the pay rate: ");
				PayRate = Convert.ToDouble(Console.ReadLine());

				GrossPay = PayRate * HoursWorked;

				//give time and a half for hours above 40
				if (HoursWorked > 40)
				{
					GrossPay += PayRate * (HoursWorked - 40) * 1.5;
				}

				Tax = GrossPay * 0.40;
				NetPay = GrossPay - Tax;

				Console.WriteLine("Hours worked: {0:f}, Gross pay: {1:C}, Net pay: {2:C}, Tax: {3:C}",
					HoursWorked, GrossPay, NetPay, Tax);
			}
			catch (Exception e)
			{
				Console.WriteLine("Something bad happened: " + e.Message);
			}
			finally
			{
				Console.ReadKey();
			}
		}
	}
}
