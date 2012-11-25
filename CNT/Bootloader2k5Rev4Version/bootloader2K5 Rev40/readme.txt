Bootloader 2K5
==============

Files:

Bootloader2k5.exe	: Main program
memmap.bld		: Memory Map Image File (must be set for *your* target device)
defport.txt		: Default COMM port setup file, adjust as needed

bootext.s19		: External EEPROM Program Loader
bootint.s19		: Internal EEPROM Program Loader
bootram.s19		: RAM Program Loader
bootfla.s19		: External FLASH Program Loader

micro11.s19		: GB's Micro 11 Monitor

readme.txt		: *this

[The s19 folder contains test programs, and is not required by Bootloader2K5.]

Notes:

This version of bootloader is designed to operate with
any XTAL frequency device. It is also known to work
with A1/E1 parts @ 4.9152MHz.

Program loaders are < 256 bytes in size to operate with
A family parts.

All files should remian in the same folder as the Bootloader2k5.exe
file.