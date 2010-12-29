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
				
				jsr		lcdinit
				jsr		adc_init
				jsr		ShowPleasantries
				
loop:			
				ldaa	CursorToVoltPos	
				jsr		LCDGoDDData
				jsr		adc_sample
				ldab	#5				;5 bytes per table entry
				mul
				addd	#DataTable		;index into the table
				xgdx					;put the string location into x
				
				jsr		LCDDDString		;send the string to the LCD
				
				ldab	#20
				jsr		dodelay			;delay 200ms
				
				bra		loop
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ShowPleasantries															;
;	Purpose		: Displays a pleasant prompt on the LCD						;
;	Modifies	: The contents of the LCD									;
;	Accepts		: Nothing!													;
;	Return		: Nothing!													;
;	Last mod	: Oct 15 2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ShowPleasantries:
				pshx
				ldx		#Pleasantries
				jsr		LCDClear
				jsr		LCDDDString
				pulx
				rts
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
.include 'delay.asm'
;.include 'sci.asm'
.include 'LCD.asm'
;.include 'spi.asm'
.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Pleasantries:		.asciz	"DC Voltage: "
				
CursorToVoltPos:	.db	0b10001100

DataTable:			
					.asciz	"0.00"
					.asciz	"0.02"
					.asciz	"0.04"
					.asciz	"0.06"
					.asciz	"0.08"
					.asciz	"0.10"
					.asciz	"0.12"
					.asciz	"0.14"
					.asciz	"0.16"
					.asciz	"0.18"
					.asciz	"0.20"
					.asciz	"0.22"
					.asciz	"0.24"
					.asciz	"0.26"
					.asciz	"0.28"
					.asciz	"0.30"
					.asciz	"0.32"
					.asciz	"0.34"
					.asciz	"0.36"
					.asciz	"0.38"
					.asciz	"0.40"
					.asciz	"0.42"
					.asciz	"0.44"
					.asciz	"0.46"
					.asciz	"0.48"
					.asciz	"0.50"
					.asciz	"0.52"
					.asciz	"0.54"
					.asciz	"0.56"
					.asciz	"0.58"
					.asciz	"0.60"
					.asciz	"0.62"
					.asciz	"0.64"
					.asciz	"0.66"
					.asciz	"0.68"
					.asciz	"0.70"
					.asciz	"0.72"
					.asciz	"0.74"
					.asciz	"0.76"
					.asciz	"0.78"
					.asciz	"0.80"
					.asciz	"0.82"
					.asciz	"0.84"
					.asciz	"0.86"
					.asciz	"0.88"
					.asciz	"0.90"
					.asciz	"0.92"
					.asciz	"0.94"
					.asciz	"0.96"
					.asciz	"0.98"
					.asciz	"1.00"
					.asciz	"1.02"
					.asciz	"1.04"
					.asciz	"1.06"
					.asciz	"1.08"
					.asciz	"1.10"
					.asciz	"1.12"
					.asciz	"1.14"
					.asciz	"1.16"
					.asciz	"1.18"
					.asciz	"1.20"
					.asciz	"1.22"
					.asciz	"1.24"
					.asciz	"1.26"
					.asciz	"1.28"
					.asciz	"1.30"
					.asciz	"1.32"
					.asciz	"1.34"
					.asciz	"1.36"
					.asciz	"1.38"
					.asciz	"1.40"
					.asciz	"1.42"
					.asciz	"1.44"
					.asciz	"1.46"
					.asciz	"1.48"
					.asciz	"1.50"
					.asciz	"1.52"
					.asciz	"1.54"
					.asciz	"1.56"
					.asciz	"1.58"
					.asciz	"1.60"
					.asciz	"1.62"
					.asciz	"1.64"
					.asciz	"1.66"
					.asciz	"1.68"
					.asciz	"1.70"
					.asciz	"1.72"
					.asciz	"1.74"
					.asciz	"1.76"
					.asciz	"1.78"
					.asciz	"1.80"
					.asciz	"1.82"
					.asciz	"1.84"
					.asciz	"1.86"
					.asciz	"1.88"
					.asciz	"1.90"
					.asciz	"1.92"
					.asciz	"1.94"
					.asciz	"1.96"
					.asciz	"1.98"
					.asciz	"2.00"
					.asciz	"2.02"
					.asciz	"2.04"
					.asciz	"2.06"
					.asciz	"2.08"
					.asciz	"2.10"
					.asciz	"2.12"
					.asciz	"2.14"
					.asciz	"2.16"
					.asciz	"2.18"
					.asciz	"2.20"
					.asciz	"2.22"
					.asciz	"2.24"
					.asciz	"2.26"
					.asciz	"2.28"
					.asciz	"2.30"
					.asciz	"2.32"
					.asciz	"2.34"
					.asciz	"2.36"
					.asciz	"2.38"
					.asciz	"2.40"
					.asciz	"2.42"
					.asciz	"2.44"
					.asciz	"2.46"
					.asciz	"2.48"
					.asciz	"2.50"
					.asciz	"2.52"
					.asciz	"2.54"
					.asciz	"2.56"
					.asciz	"2.58"
					.asciz	"2.60"
					.asciz	"2.62"
					.asciz	"2.64"
					.asciz	"2.66"
					.asciz	"2.68"
					.asciz	"2.70"
					.asciz	"2.72"
					.asciz	"2.74"
					.asciz	"2.76"
					.asciz	"2.78"
					.asciz	"2.80"
					.asciz	"2.82"
					.asciz	"2.84"
					.asciz	"2.86"
					.asciz	"2.88"
					.asciz	"2.90"
					.asciz	"2.92"
					.asciz	"2.94"
					.asciz	"2.96"
					.asciz	"2.98"
					.asciz	"3.00"
					.asciz	"3.02"
					.asciz	"3.04"
					.asciz	"3.06"
					.asciz	"3.08"
					.asciz	"3.10"
					.asciz	"3.12"
					.asciz	"3.14"
					.asciz	"3.16"
					.asciz	"3.18"
					.asciz	"3.20"
					.asciz	"3.22"
					.asciz	"3.24"
					.asciz	"3.26"
					.asciz	"3.28"
					.asciz	"3.30"
					.asciz	"3.32"
					.asciz	"3.34"
					.asciz	"3.36"
					.asciz	"3.38"
					.asciz	"3.40"
					.asciz	"3.42"
					.asciz	"3.44"
					.asciz	"3.46"
					.asciz	"3.48"
					.asciz	"3.50"
					.asciz	"3.52"
					.asciz	"3.54"
					.asciz	"3.56"
					.asciz	"3.58"
					.asciz	"3.60"
					.asciz	"3.62"
					.asciz	"3.64"
					.asciz	"3.66"
					.asciz	"3.68"
					.asciz	"3.70"
					.asciz	"3.72"
					.asciz	"3.74"
					.asciz	"3.76"
					.asciz	"3.78"
					.asciz	"3.80"
					.asciz	"3.82"
					.asciz	"3.84"
					.asciz	"3.86"
					.asciz	"3.88"
					.asciz	"3.90"
					.asciz	"3.92"
					.asciz	"3.94"
					.asciz	"3.96"
					.asciz	"3.98"
					.asciz	"4.00"
					.asciz	"4.02"
					.asciz	"4.04"
					.asciz	"4.06"
					.asciz	"4.08"
					.asciz	"4.10"
					.asciz	"4.12"
					.asciz	"4.14"
					.asciz	"4.16"
					.asciz	"4.18"
					.asciz	"4.20"
					.asciz	"4.22"
					.asciz	"4.24"
					.asciz	"4.26"
					.asciz	"4.28"
					.asciz	"4.30"
					.asciz	"4.32"
					.asciz	"4.34"
					.asciz	"4.36"
					.asciz	"4.38"
					.asciz	"4.40"
					.asciz	"4.42"
					.asciz	"4.44"
					.asciz	"4.46"
					.asciz	"4.48"
					.asciz	"4.50"
					.asciz	"4.52"
					.asciz	"4.54"
					.asciz	"4.56"
					.asciz	"4.58"
					.asciz	"4.60"
					.asciz	"4.62"
					.asciz	"4.64"
					.asciz	"4.66"
					.asciz	"4.68"
					.asciz	"4.70"
					.asciz	"4.72"
					.asciz	"4.74"
					.asciz	"4.76"
					.asciz	"4.78"
					.asciz	"4.80"
					.asciz	"4.82"
					.asciz	"4.84"
					.asciz	"4.86"
					.asciz	"4.88"
					.asciz	"4.90"
					.asciz	"4.92"
					.asciz	"4.94"
					.asciz	"4.96"
					.asciz	"4.98"
					.asciz	"5.00"
					.asciz	"5.02"
					.asciz	"5.04"
					.asciz	"5.06"
					.asciz	"5.08"
					.asciz	"5.10"

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
				