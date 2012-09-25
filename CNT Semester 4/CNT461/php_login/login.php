<?php
session_start ();

echo "<html>";
echo "<head><title>Log in complete</title></head>";
echo "<body>";
$_SESSION ['status'] = 0;
if ($usern == "guest" && $passw == "guest")
{
	echo "<h1>You are logged in with guest permissions.</h1>";
	$_SESSION ['status'] = 1;
}

if ($usern == "operator" && $passw == "operator")
{
	echo "<h1>You are logged in with operator permissions.</h1>";
	$_SESSION ['status'] = 2;
}

if ($usern == "programmer" && $passw == "programmer")
{
	echo "<h1>You are logged in with programmer permissions.</h1>";
	$_SESSION ['status'] = 3;
}

if ($_SESSION ['status'] == 0)
{
	echo "<h1>You could not be logged in. Please <a href='index.html'>try again.</a></h1>";
	session_destroy ();
}
else
{
	echo "<a href='viewstatus.php'>Proceed...</a>";
}
echo "</body></html>";
?>