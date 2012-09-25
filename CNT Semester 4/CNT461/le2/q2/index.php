<?php
/* Connecting, selecting database */
$link = mysql_connect("localhost", "cekjnenq", "eken9dmq")
	or die("Could not connect");
//print "My GuestBook - Connected successfully<br>";
mysql_select_db("cekjnenq_test") or die("Could not select database");

/* Performing SQL query */
$query = "SELECT * FROM labex3";
$result = mysql_query($query) or die("Query failed");

/* Printing results in HTML */
print "<html>\n";
print "<head><title>View table</title></head>\n";
print "<body>\n";
//start the table and header
print "<table border=2 width=80%>\n";
print "<tr>\n";
print "\t<td><b>Quantity</b></td>\n";
print "\t<td><b>Description</b></td>\n";
print "\t<td><b>Vendor</b></td>\n";
print "\t<td><b>Cost/Item</b></td>\n";
print "\t<td><b>Subtotal</b></td>\n";
print "</tr>";

//show the data one line per record
while ($line = mysql_fetch_array($result, MYSQL_ASSOC)) 
{
	print "\t<tr>\n";
	$i = 0;
	$subtotal = 0;
	$quantity = 0;
	$cost = 0;
	foreach ($line as $col_value) 
	{
		if ($i == 0)
		{
			$quantity = $col_value;
		}
		if ($i == 3)
		{
			$cost = $col_value;
		}
		print "\t\t<td>$col_value</td>\n";
		$i = $i + 1;
	}
	$subtotal = $quantity * $cost;
	print "\t\t<td>$subtotal</td>\n";
	print "\t</tr>\n";
}
print "</table>\n";

print "</body>\n";
print "</html>";

/* Free resultset */
mysql_free_result($result);

/* Closing connection */
mysql_close($link);
?>