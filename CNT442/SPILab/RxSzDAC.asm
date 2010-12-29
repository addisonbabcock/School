				.title by Simon Walker
;**************************************************************************
;* Sample.asm                                                             *
;*                                                                        *
;* The purpose of this program is to demonstrate source file page layout  *
;* and basic AS6811 assembler syntax.                                     *
;**************************************************************************

;.------------------------------------------------------------------------.
;|                           EQUATES SECTION                              |
;`------------------------------------------------------------------------'

LED			=	0xC000			; 2-digit LED display.
RAMSTART	=	0x1040			; first byte of large RAM block
RAMEND		=	0x7FFF			; end of RAM
RESETVEC	=	0xFFFE			; reset vector for normal modes
ROMSTART	=	0xC000			; Lowest EPROM address.
ROMEND		=	0xFFFF			; Highest EPROM address.
STACKTOP	=	0x0FFF			; top of stack (0x0FFF - 0x0100)

LCDIR		=	0x8000			; LCD instruction register
LCDDR		=	0x8001			; LCD data register

;.------------------------------------------------------------------------.
;|                           TARGET CONTROL                               |
;`------------------------------------------------------------------------'

; set the following variable to 1 to place program in EEPROM
; set the following variable to 0 to place program in RAM

TARGETROM = 0		; 0 == RAM, 1 == EEPROM

;.------------------------------------------------------------------------.
;|                               MAIN                                     |
;`------------------------------------------------------------------------'

				.MODULE MAIN				
				.AREA   StartUp (ABS)
								
				.if TARGETROM == 1
					.ORG    ROMSTART
				.else
					.ORG	RAMSTART					
				.endif

Main:			
				.if TARGETROM == 1
					LDS		#STACKTOP		; must have stack!
				.endif
				
				; place your program here ********************
				
				jsr		sciInit			;init the interfaces
				jsr		spi_init
				
loop:
				ldx		#ClearScreen	;Clear the screen
				stx		sciTxSz
				jsr		sciTxString
				
				ldx		#Prompt			;Prompt for a voltage
				stx		sciTxSz
				jsr		scitxString
				
				ldx		#sciRxSz		;Get the voltage desired
				ldab	#3
				jsr		sciRxString
				
				;translate the string into hex bytes
				inx						;point x to the chars
				inx
				
				ldaa	0,x
				jsr		ASCIIByteToHex	;translate the 1st byte
				staa	0,x
				
				ldaa	1,x
				jsr		ASCIIByteToHex	;translate the 2nd byte
				staa	1,x
				
				ldaa	2,x
				jsr		ASCIIByteToHex	;translate the 3rd byte
				staa	2,x
				
				;now pack the bytes for transmition to the DAC
				;The first byte is ready for use because the top nibble is junk
				ldaa	1,x				;A will store the 2nd nibble
				lsla					;move the 2nd nibble into position
				lsla
				lsla
				lsla
				oraa	2,x				;pack the 2nd and 3rd nibbles together
				staa	1,x				;Save the result!
				
				;we now have a 12 bit value ready to send to the DAC
				;so send it!
				ldd		0,x
				jsr		spi_data				
				
				jmp		loop
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ASCIIByteToHex															;
;	Purpose		: Translates an ASCII byte to its hex code.					;
;	Modifies	: A															;
;	Accepts		: Accumulator A. This is the byte to be translated.			;
;	Return		: The hex value in the lower nibble of A					;
;	Last mod	: 09/19/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;			
ASCIIByteToHex:
				cmpa	#0x40			;0x30 - 0x39 is 0 - 9
				blo		IsANumber
				cmpa	#0x61
				blo		IsUpperCase		;0x41 - 0x... is A-Z
				
				suba	#0x57			;lower case
				rts
				
IsANumber:		
				suba	#0x30
				rts
				
IsUpperCase:	
				suba	#0x37
				rts
				
;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
;.include 'delay.asm'
.include 'sci.asm'
;.include 'LCD.asm'
.include 'spi.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

ClearScreen:	.asciz	"\e[H\e[2J"
Prompt:			.asciz	"Enter the voltage desired: "

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

sciRxSz:		.ds	23		;String to be recieved from the SCI
sciRxBuf:		.ds	1		;Byte recieve through the SCI
sciTxBuf:		.ds	1		;Send this byte through the SCI
sciTxSz:		.ds	2		;Pointer to the null terminated string to be sent

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				