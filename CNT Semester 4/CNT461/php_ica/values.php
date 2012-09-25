<?php
header ("Cache-Control: no-cache, must-revalidate");
header ("Expires: Mon, 26 Jul 1997 05:00:00 GMT");

$rpm = rand (3550, 3650);
$temp = rand (238, 263);
$volt = rand (216, 264);

$myXML = "<?xml version='1.0' encoding='iso-8859-1'?>\n";
$myXML .= "<rpmLayer>RPM=$rpm</rpmLayer>\n";
$myXML .= "<tempLayer>Temp=$temp</tempLayer>\n";
$myXML .= "<voltLayer>Voltage=$volt</voltLayer>\n";
echo $myXML;
?>