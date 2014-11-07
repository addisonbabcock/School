
using System.Data.SqlClient;

public class Helpers
{
    private const string connectionString = "Server=SERVIN8TOR\\sqlexpress; Database=ababcock1_BAIS3150_StudentsDemo; Integrated Security=SSPI";

    public string GetConnectionString()
	{
        return connectionString;
	}
}
