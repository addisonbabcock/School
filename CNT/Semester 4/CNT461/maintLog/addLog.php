<html>
<head><title>Adding log entry...</title></head>
<body>
<?php
//Set up and connect
$server="localhost";	//Servername
$user="rmwin8h3";	//Username
$pass="l0lWUT";     //Password

//Make a connection
$hookup = mysql_connect($server, $user, $pass) or die ("DAMMIT!");

//Select the database
$debase=@mysql_select_db(rmwin8h3_maintLog, $hookup) or die ("Sorry can't connect with the database.");

//echo date ("F j, Y, g:i a") ."<br>";
//echo $operator . "<bR>";
//echo $equipment . "<br>";
//echo $problem . "<br>";

//Variables from form become data for mySQL.
$sql="INSERT INTO logTable (myDate, operator, equipment, problem)
VALUES('".date ("F j, Y, g:i a")."','$operator','$equipment','$problem')";

//Use the query function to send record to MySQL.
mysql_query($sql, $hookup) or die ("couldnt add");
?>
<h1>Record added succesfully!</h1>
<a href="viewLog.php">View log file</a><br>
<a href="viewLogXML.php">View log in XML</a><br>
<a href="addLog.html">Add another record</a><br>
</body>
</html>