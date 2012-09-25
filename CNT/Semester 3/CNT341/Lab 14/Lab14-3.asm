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

TARGETROM = 1		; 0 == RAM, 1 == EEPROM

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
				
				jsr		LCDInit

				;send name on the first line, starting at 0x00
				clra
				jsr		LCDGoDDData
				ldx		#Name
				jsr		LCDDDString
				
				;send class on the second line, 0x40
				ldaa	#0x40
				jsr		LCDgoDDData
				ldx		#Class
				jsr		LCDDDString
				
				;send the mantra offscreen, 0x17
				ldaa	#0x17
				jsr		LCDGoDDData
				ldx		#Mantra1
				jsr		LCDDDString
				
				;second part of the mantra, 0x57
				ldaa	#0x57
				jsr		LCDGoDDData
				ldx		#Mantra2
				jsr		LCDDDString
				
				;delays will be 30 * 10ms = 2s
				ldab	#30

MainLoop:		
				jsr		LCDShiftLeft
				;now wait 300 milliseconds
				jsr		DoDelay
				bra		MainLoop
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
.include 'delay.asm'
;.include 'sci.asm'
.include 'LCD.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Name:			.asciz	"Addison Babcock"
Class:			.asciz	"CNT25 - 3K"
Mantra1:		.asciz	"Stuff goes here"
Mantra2:		.asciz	"...IN SPACE"

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				