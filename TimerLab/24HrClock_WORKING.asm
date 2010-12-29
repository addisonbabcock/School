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

SPDR		=	0x102A			;Data Register
SPCR		=	0x1028			;Control Register
SPSR		=	0x1029			;Status Register
DDRD		=	0x1009			;Data Direction Register
PORTD		=	0x1008			;Port D

TCNT		=	0x100E			;16-bit timer count register
TOC1		=	0x1016			;timer output compare 1
TOC2		=	0x1018			;timer output compare 2
TOC3		=	0x101A			;timer output compare 3
TOC4		=	0x101C			;timer output compare 4
TCTL1		=	0x1020			;timer control register 1
TCTL2		=	0x1021			;timer control register 2
TMSK1		=	0x1022			;timer mask register 1
TFLG1		=	0x1023			;timer flag register 1
TMSK2		=	0x1024			;timer mask register 2
TFLG2		=	0x1025			;timer flag register 2

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
				
				sei
				ldaa	#0b01000000			;set OC2 to toggle
				staa	TCTL1
				ldaa	#0b01000000			;enable IRQ from OC2
				staa	TMSK1
				
				clra						;init a couple vars
				staa	Ticks
				staa	RefreshTime
				
				ldd		TCNT
				addd	TOC2_Inc
				std		TOC2
				
				ldd		#ClearScreen		;clear the screen
				std		sciTxSz
				jsr		sciTxString
				
				ldx		#sciRxSz			;get the current time
				ldab	#8					;from the user in HH:MM:SS
				jsr		sciRxString
				
				ldaa	2,x					;adjust the top nibble of hours
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Hours
				
				ldaa	3,x					;adjust the bottom nibble of hours
				suba	#'0
				
				oraa	Hours				;add the top and bottom nibbles
				staa	Hours
				
				ldaa	5,x					;adjust the top nibble of minutes
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Minutes
				
				ldaa	6,x					;adjust the bottom nibble of minutes
				suba	#'0
				
				oraa	Minutes				;add the top and bottom nibbles
				staa	Minutes
				
				ldaa	8,x					;adjust the top nibble of seconds
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Seconds
				
				ldaa	9,x					;adjust the bottom nibble of seconds
				suba	#'0
				
				oraa	Seconds
				staa	Seconds				
				
				ldd		#CursorToTimePosition
				std		sciTxSz				;Reset the cursor every time the
											;seconds change
				
				cli							;all set! GO!
				
loop:			
				ldaa	RefreshTime
				cmpa	#1
				bne		loop				;wait for a tick
				
				clra						;acknowledge the newtick flag
				staa	RefreshTime
				
DisplayTime:	
				jsr		sciTxString			;reset the cursor position
				
				ldaa	Hours				;show the hours 1st
				jsr		BCDByteToASCII
				
				staa	sciTxBuf			;show the high digit of hours
				jsr		sciTxByte
				stab	sciTxBuf			;show the lower digit of hours
				jsr		sciTxByte
				
				ldaa	#':					;seperate hours and minutes with
				staa	sciTxBuf			;a colon :
				jsr		sciTxByte
				
				ldaa	Minutes				;we will be showing the minutes 1st
				jsr		BCDByteToASCII
				
				staa	sciTxBuf			;show the high nibble of minutes
				jsr		sciTxByte
				stab	sciTxBuf			;show the low nibble of minutes
				jsr		sciTxByte
				
				ldaa	#':					;show a colon for seperating
				staa	sciTxBuf			;minutes from seconds
				jsr		sciTxByte
				
				ldaa	Seconds				;now show the seconds
				jsr		BCDByteToASCII
				
				staa	sciTxBuf			;show the high nibble of seconds
				jsr		sciTxByte
				stab	sciTxBuf			;show the low nibble of seconds
				jsr		sciTxByte	
							
				jmp		loop	
				
OC2I_ISR:
				ldaa	#0b01000000			;ACK Interrupt
				staa	TFLG1
				
				ldaa	Ticks				;do the ticks need to be wrapped?
				cmpa	#0x99
				bne		DontWrapTicks
				
				clrb						;wrap ticks
				stab	Ticks
				
				ldaa	#1					;inform the mian program to kindly
				staa	RefreshTime			;refresh the screen
				
				ldaa	Seconds				;do the seconds need to be wrapped?
				cmpa	#0x59
				bne		DontWrapSeconds
				
				clrb						;wrap seconds
				stab	Seconds
				
				ldaa	Minutes				;do the minutes need to be wrapped?
				cmpa	#0x59
				bne		DontWrapMinutes
				
				clrb						;wrap minutes
				stab	Minutes
				
				ldaa	Hours
				cmpa	#0x23
				bne		DontWrapHours
				
				clrb						;wrap hours
				stab	Hours
				
				bra		OC2I_ISR_WrapUp		;the time has been updated
				
DontWrapTicks:
				adda	#1					;increment ticks and exit
				daa
				staa	Ticks
				bra		OC2I_ISR_WrapUp
				
DontWrapSeconds:
				adda	#1					;increment seconds and exit
				daa
				staa	Seconds
				bra		OC2I_ISR_WrapUp
				
DontWrapMinutes:
				adda	#1					;increment minutes and exit
				daa
				staa	Minutes
				bra		OC2I_ISR_WrapUp
				
DontWrapHours:
				adda	#1					;increment hours and exit
				daa
				staa	Hours
				bra		OC2I_ISR_WrapUp
				
OC2I_ISR_Wrapup:
				ldd		TOC2				;set up next interrupt
				addd	TOC2_INC
				std		TOC2
				
				ldaa	Ticks
				staa	LED
				
				
				rti
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; BCDByteToASCII															;
;	Purpose		: Translates a BCD byte into 2 ascii codes					;
;	Modifies	: A and B													;
;	Accepts		: A is the hex byte to be translated						;
;	Return		: The ASCII code in A for the upper nibble in A	and the		;
;		lower nibble in B.													;
;	Last mod	: Nov 7 2007 - Created										;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;				
BCDByteToASCII:
				tab
				lsra					;move MSB to LSB
				lsra
				lsra
				lsra
				cmpa	#0x0A
				bge		UseLettersA
				adda	#'0
				bra		TranslateB
UseLettersA:	suba	#0x0A
				adda	#'A
TranslateB:		andb	#0x0F			;clear out MSB, work on lower nibble
				cmpb	#0x0A
				bge		UseLettersB
				addb	#'0
				rts
UseLettersB:	subb	#0x0A
				addb	#'A
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
;.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

TOC2_INC:		.dw		12288		;((1/100 Hz)/813.802 ns) = 12288.0
ClearScreen:	.asciz	"\e[2J\e[?25l\e[H"
CursorToTimePosition:
				.asciz	"\e[H"

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

Hours:			.ds	1		;Holds the BCD variable for hours (0x00-0x23)
Minutes:		.ds	1		;Holds the BCD variable for minutes (0-0x59)
Seconds:		.ds	1		;Holds the BCD variable for seconds (0-0x59)
Ticks:			.ds	1		;Holds the BCD variable for hundredths (0-0x99)
RefreshTime:	.ds	1		;A flag to indicate to the main program that 
							;a second has elapsed and the screen should be
							;refreshed.

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
					.org	0xffe6
					.dw		OC2I_ISR
				.endif

;				.if TARGETROM == 0
					.org	0x7fe6
					.dw		OC2I_ISR
;				.endif