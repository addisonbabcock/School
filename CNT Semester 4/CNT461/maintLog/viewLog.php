<?php
/* Connecting, selecting database */
$link = mysql_connect("localhost", "rmwin8h3", "l0lWUT")
	or die("Could not connect");
//print "My GuestBook - Connected successfully<br>";
mysql_select_db("rmwin8h3_maintLog") or die("Could not select database");

/* Performing SQL query */
$query = "SELECT * FROM logTable";
$result = mysql_query($query) or die("Query failed");

/* Printing results in HTML */
print "<html>\n";
print "<head><title>View log file</title></head>\n";
print "<body>\n";
//start the table and header
print "<table border=2 width=80%>\n";
print "<tr>\n";
print "\t<td>Date</td>\n";
print "\t<td>Operator</td>\n";
print "\t<td>Equipment</td>\n";
print "\t<td>Problem</td>\n";
print "</tr>";

//show the data one line per record
while ($line = mysql_fetch_array($result, MYSQL_ASSOC)) 
{
	print "\t<tr>\n";
	foreach ($line as $col_value) 
	{
		print "\t\t<td>$col_value</td>\n";
	}
	print "\t</tr>\n";
}
print "</table>\n";

print "</body>\n";
print "<a href='addLog.html'>Add a log entry</a>\n";
print "<a href='viewLogXML.php'>View log in XML</a>\n";
print "</html>";

/* Free resultset */
mysql_free_result($result);

/* Closing connection */
mysql_close($link);
?>