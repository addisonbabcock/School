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
$outString = $guestName."<br>".$guestEmail."<br>".$guestComments."<p>";
fwrite ($fp, $outString);
fclose ($fp);

?>
Entry Added!
</h1>
</body>
</html>
