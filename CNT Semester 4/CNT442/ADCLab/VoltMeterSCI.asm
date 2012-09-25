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

SPDR			=	0x102A	;Data Register
SPCR			=	0x1028	;Control Register
SPSR			=	0x1029	;Status Register
DDRD			=	0x1009	;Data Direction Register
PORTD			=	0x1008	;Port D

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
				jsr		adc_init
				jsr		ShowPleasantries
				
loop:				
				jsr		adc_sample
				ldab	#2				;d = sample * 2
				mul
				ldx		#0x64			;d = d / 100
				idiv
				pshx					;save hundreds to the stack
				ldx		#0x0A			;d = d / 10
				idiv
				pshx					;save tens to the stack
				pshb					;save ones to the stack
				psha					;maintaining empty byte for consistancy
				
				ldx		#CursorToVoltPos
				stx		sciTxSz
				jsr		sciTxString		;set the terminal to display voltage
				
				tsx
				ldaa	5,x				;load the hundreds
				jsr		HexByteToASCII
				staa	sciTxBuf
				jsr		sciTxByte		;show the hundreds
				
				ldaa	#'.
				staa	sciTxBuf
				jsr		sciTxByte		;show a period
				
				ldaa	3,x				;load the tens
				jsr		HexByteToASCII
				staa	sciTxBuf
				jsr		sciTxByte		;show the tens
				
				ldaa	1,x				;load the ones
				jsr		HexByteToASCII
				staa	sciTxBuf
				jsr		sciTxByte		;show the ones
				
				pulb					;clean up the stack
				pula
				pulx
				pulx
				bra		loop
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ShowPleasantries															;
;	Purpose		: Displays a colorful static screen on the terminal			;
;	Modifies	: The contents of the terminal screen						;
;	Accepts		: Nothing!													;
;	Return		: Nothing!													;
;	Last mod	: Oct 15 2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ShowPleasantries:
				pshx
				ldx		#Pleasantries
				stx		sciTxSz
				jsr		sciTxString
				pulx
				rts
				
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
.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Pleasantries:		.ascii	"\e[?25l\e[2J"
					.ascii	"\e[3;4H\e[30mMicro Design 2 - Lab #3"
					.ascii	"\e[4;6H\e[31mDigital Voltmeter"
					.ascii	"\e[5;5H\e[32mby Addison Babcock"
					.asciz	"\e[7;4H\e[34mVoltage Reading:     V"
				
CursorToVoltPos:	.ascii	"\e[7;21H\e[30m\e[41m"

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
				