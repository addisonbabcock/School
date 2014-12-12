using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

/// <summary>
/// Summary description for MyWebService
/// </summary>
[WebService(Namespace = "BAISTServices", Description="My First Web Service", Name="ababcock1BAIS3150Service")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class MyWebService : System.Web.Services.WebService {

    public MyWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
	{
        return "Hello World";
    }

	[WebMethod]
	public DataSet GetCustomersByCountry(string country)
	{
		DataSet dataSet = new DataSet("Northwind");
		DataTable table = new DataTable("Customers");

		using (var conn = new SqlConnection(new Helpers().GetNorthwindConnectionString()))
		{
			conn.Open();
			using (var command = new SqlCommand("ababcock1GetCustomersByCountry", conn))
			{
				command.CommandType = CommandType.StoredProcedure;

				var countryParam = new SqlParameter();
				countryParam.ParameterName = "@Country";
				countryParam.SqlDbType = SqlDbType.VarChar;
				countryParam.SqlValue = country;
				countryParam.Direction = ParameterDirection.Input;
				command.Parameters.Add(countryParam);

				var reader = command.ExecuteReader();
				table.Load(reader);
				dataSet.Tables.Add(table);
			}
		}

		return dataSet;
	}

	[WebMethod]
	public bool IsItPrime(int number)
	{
		double dNumber = number;
		for (int i = 2; i < number; ++i)
		{
			double denominator = i;
			if (dNumber / denominator == number / i)		//yes... yes... floating point casting.. whatever
			{
				return false;
			}
		}

		return true;
	}

	[WebMethod]
	public string BinaryToDecimal(string binary)
	{
		double dec = 0;

		for (int i = 0; i < binary.Length; ++i)
		{
			if (binary[i] == '1')
			{
				dec += Math.Pow(2, binary.Length - i - 1);
			}
		}

		return dec.ToString();
	}

	[WebMethod]
	public int MathematicalMaximum(int a, int b)
	{
		double da = a;
		double db = b;
		double ave = (da + db) / 2;
		double diff = Math.Abs((da - db) / 2);
		return (int)(ave + diff);
	}

	public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
	{
		const int BitsInLong = 64;
		const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		if (radix < 2 || radix > Digits.Length)
			throw new ArgumentException("The radix must be >= 2 and <= " +
				Digits.Length.ToString());

		if (decimalNumber == 0)
			return "0";

		int index = BitsInLong - 1;
		long currentNumber = Math.Abs(decimalNumber);
		char[] charArray = new char[BitsInLong];

		while (currentNumber != 0)
		{
			int remainder = (int)(currentNumber % radix);
			charArray[index--] = Digits[remainder];
			currentNumber = currentNumber / radix;
		}

		string result = new String(charArray, index + 1, BitsInLong - index - 1);
		if (decimalNumber < 0)
		{
			result = "-" + result;
		}

		return result;
	}

	[WebMethod]
	public string ToBase(long value, int _base)
	{
		return DecimalToArbitrarySystem(value, _base);
	}
}
