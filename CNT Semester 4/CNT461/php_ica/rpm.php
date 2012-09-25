<?php
header ("Cache-Control: no-cache, must-revalidate");
header ("Expires: Mon, 26 Jul 1997 05:00:00 GMT");

$temp = rand (3550, 3650);

$myXML = "<?xml version='1.0' encoding='iso-8859-1'?>";
$myXML .= "<rpmLayer>RPM=$temp</rpmLayer>";
echo $myXML;
?>