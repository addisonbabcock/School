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

TCNT		=	0x100E		;16-bit timer count register
TOC1		=	0x1016		;timer output compare 1
TOC2		=	0x1018		;timer output compare 2
TOC3		=	0x101A		;timer output compare 3
TOC4		=	0x101C		;timer output compare 4
TCTL1		=	0x1020		;timer control register 1
TCTL2		=	0x1021		;timer control register 2
TMSK1		=	0x1022		;timer mask register 1
TFLG1		=	0x1023		;timer flag register 1
TMSK2		=	0x1024		;timer mask register 2
TFLG2		=	0x1025		;timer flag register 2

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
				
				clra						;clear out the time
				staa	Hours
				staa	Minutes
				staa	Seconds
				staa	Ticks
				staa	NewTick
				
				ldd		TCNT
				addd	TOC2_Inc
				std		TOC2
				
				ldd		#ClearScreen		;clear the screen
				std		sciTxSz
				jsr		sciTxString
				ldd		#CursorToTimePosition
				std		sciTxSz				;Reset the cursor every time the
											;seconds change
				
				cli
				
loop:			

				
				ldaa	NewTick
				cmpa	#1
				bne		loop				;wait for a tick
				
				clra						;acknowledge the newtick flag
				staa	NewTick
				
				ldaa	Ticks
				adda	#1					;increment the ticks
				daa
				staa	LED
				staa	Ticks
				
				cmpa	#0
				bne		Loop				;if the ticks wraps to 0, then we
											;should increment seconds and
											;possibly minutes
											;if not we dont have to do anything
								
				clra						;wrap the ticks to 0, and inc secs
				staa	Ticks
				ldaa	Seconds
				adda	#1
				daa
				staa	Seconds
				
				cmpa	#0x60
				bne		DisplayTime			;if we dont have to wrap seconds
											;we can just show the time
											
				clra						;wrap the seconds to 0, inc minute
				staa	Seconds
				ldaa	Minutes
				adda	#1
				daa
				staa	Minutes
				
				cmpa	#0x60
				bne		DisplayTime			;if we dont have to wrap minutes
											;we can just show the time
											
				clra						;wrap minutes to 0, inc hours
				staa	Minutes
				ldaa	Hours
				adda	#1
				daa
				staa	Hours
				
				cmpa	#0x24				;dont let the hours go too high
				bne		DisplayTime
				
				clra
				staa	Hours
				
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
				
				ldd		TOC2				;set up next interrupt
				addd	TOC2_INC
				std		TOC2
				
				ldaa	#1
				staa	NewTick				;tell the main program about tick
				
				
				
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
NewTick:		.ds	1		;A flag to indicate to the main program that 
							;a tick has occured.

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