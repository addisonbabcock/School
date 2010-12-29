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
				
				jsr		sciInit
				
loop:			
				ldx		#ClearScreen			;Clear the screen
				stx		sciTxSz
				jsr		scitxString
				
				ldx		#InputPrompt			;Show the prompt
				stx		sciTxSz
				jsr		sciTxString
				
				ldx		#sciRxSz				;Get a string
				ldab	#20
				jsr		sciRxString
				
				ldx		#ShowString				;Get ready to show the string
				stx		sciTxSz
				jsr		sciTxString
				
				ldx		#sciRxSz				;Show the string
				inx
				inx
				stx		sciTxSz
				jsr		sciTxString
				
				ldx		#ShowStrlen				;Get ready to show the length
				stx		sciTxSz
				jsr		sciTxString
				
				ldx		#sciRxSz				;Get the length
				ldaa	1,x
				ldab	1,x
				anda	#0xF0					;A will contain the MSByte
				lsra
				lsra
				lsra
				lsra
				andb	#0x0F					;B will contain the LSByte
				jsr		HexByteToASCII			;Turn A into an ASCII code
				staa	sciTxBuf				;Send A over the SCI
				jsr		sciTxByte
				tba								;Now working on the LSB
				jsr		HexByteToASCII			;Turn A into an ASCII code
				staa	sciTxBuf				;Send it over the SCI
				jsr		sciTxByte
				
				ldx		#KeyPrompt				;Show the prompt for a key
				stx		sciTxSz
				jsr		sciTxString
				
				jsr		sciRxByte				;Wait for key press
				
				bra		loop					;do it all again!
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; HexByteToASCII															;
;	Purpose		: Translates a hex byte to it's ascii code					;
;	Modifies	: A															;
;	Accepts		: lower nibble of A. This is the byte to be translated		;
;	Return		: The ASCII code in A										;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;			
HexByteToASCII:
				cmpa	#0x0A
				bge		UseLetters		;if a >= 0x0A, use the ascii value for A-F
				adda	#'0				;a < 0x0A, use ascii values for 0-9
				rts
UseLetters:		suba	#0x0A
				adda	#'A
				rts

				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
;.include 'delay.asm'
.include 'sci.asm'
;.include 'LCD.asm'
;.include 'spi.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

ClearScreen:	.asciz	"\e[H\e[2J"
InputPrompt:	.ascii	"\e[4;8HEnter your input string on keyboard"
				.ascii	"\e[5;8HMaximum number of characters = 20"
				.asciz	"\e[6;8H"
ShowString:		.asciz	"\e[10;8HString entered: "
ShowStrLen:		.asciz	"\e[11;8HString length: "
KeyPrompt:		.asciz	"\e[12;8HPress any key to continue..."

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
				