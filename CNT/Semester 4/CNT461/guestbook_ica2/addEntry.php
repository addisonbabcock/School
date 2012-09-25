<?php
header ("Cache-Control: no-cache, must-revalidate");
header ("Expires: Mon, 26 Jul 1997 05:00:00 GMT");
?>
<html>
<head>
<title>Guest Book</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>
<body>
<h1> 
<?php
$fp = fopen ("guestbook.txt", 'a');
if (!$fp)
{
	echo "File not found!";
	echo "</h1></body></html>";
	exit;
}
$locked = flock ($fp, LOCK_EX);
if (!$locked)
{
	echo "Could not lock file.";
	echo "</h1></body></html>";
	exit;
}

$validated = true;
$errorMsg = "ERROR!<br>One or more things was wrong with your entry</h1><br>";
$outString = "";

//check for name
if (!eregi ("[[:alnum:]][[:alnum:]]+", $guestName))
{
	$validated = false;
	$errorMsg = $errorMsg . "You need to provide a name (2 or more alpha numeric chars)<br>";
}
else
{
	$outString = $guestName . "<br>";
}

//check for email
if (!eregi ("([[:alnum:]]+)(@)([[:alnum:]]+)(\.[[:alnum:]])+", $guestEmail))
{
	$validated = false;
	$errorMsg = $errorMsg . "You need to provide an email address.<br>";
}
else
{
	$outString = $outString . $guestEmail . "<br>";
}

//add a timestamp
$outString = $outString . date ("F j, Y, g:i a") . "<br>";

//check for a comment
if (!eregi ("[[:alnum:]]+", $guestComment))
{
	$validated = false;
	$errorMsg = $errorMsg . "You need to provide a comment.<br>";
}
else
{
	$outString = $outString . $guestComment . "<p>";
}

if ($validated)
{
	fwrite ($fp, $outString);
	fclose ($fp);
}
else
{
	$errorMsg = $errorMsg . "</body></html>";
	echo $errorMsg;
	exit;	
}
?>
Entry Added! Press your back button to see your new entry! </h1>
</body>
</html>
