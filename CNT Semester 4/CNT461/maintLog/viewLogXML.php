<?php
/* Connecting, selecting database */
$link = mysql_connect("localhost", "rmwin8h3", "l0lWUT")
	or die("Could not connect");
//print "My GuestBook - Connected successfully<br>";
mysql_select_db("rmwin8h3_maintLog") or die("Could not select database");

/* Performing SQL query */
$query = "SELECT * FROM logTable";
$result = mysql_query($query) or die("Query failed");

//DTD
print "<?xml version='1.0' encoding='ISO-8859-1'?>\n";
print "<!DOCTYPE logfile [\n";
print "	<!ELEMENT logfile (record)*>\n";
print "	<!ELEMENT record (myDate, operator, equipment, problem)>\n";
print "	<!ELEMENT myDate (#PCDATA)>\n";
print "	<!ELEMENT operator (#PCDATA)>\n";
print "	<!ELEMENT equipment (#PCDATA)>\n";
print "	<!ELEMENT problem (#PCDATA)>\n";
print "]>\n\n";
print "<logfile>\n";

//show the data one line per record
while ($line = mysql_fetch_array($result, MYSQL_ASSOC)) 
{
	print "	<record>\n";
	print "		<myDate>" . $line ['myDate'] . "</myDate>\n";
	print "		<operator>" . $line ['operator'] . "</operator>\n";
	print "		<equipment>" . $line ['equipment'] . "</equipment>\n";
	print "		<problem>" . $line ['problem'] . "</problem>\n";
	print "	</record>\n";
}
print "</logfile>\n";
/* Free resultset */
mysql_free_result($result);

/* Closing connection */
mysql_close($link);
?>