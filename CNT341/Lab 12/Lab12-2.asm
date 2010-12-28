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

Init:			CLRA					;init counter
										;Init LED display
				jsr		makebit7output	;turn off all the leds
				jsr		turnoffled2
				jsr		turnoffled1
				jsr		turnoffled0
				staa	SW0Previous
				staa	SW1Previous
				staa	SW2Previous
LittleInit:		staa	Ones
				staa	Tens
				staa	led	
				
MainLoop:		jsr		DebounceSW0
				cmpb	#0x01
				beq		SW0Down
				stab	SW0Previous
				bra		DoneIncSecs
SW0Down:		ldaa	SW0Previous
				cba
				bne		IncSeconds				
DoneIncSecs:	jsr		DebounceSW1
				cmpb	#0x01
				beq		SW1Down
				stab	SW1Previous
				bra		DoneIncTens
SW1Down:		ldaa	SW1Previous
				cba
				bne		IncTens
DoneIncTens:	jsr		DebounceSW2
				cmpb	#0x01
				beq		SW2Down
				bra		MainLoop
SW2Down:		ldaa	SW2Previous
				cba
				bne		StartCountDown
				bra		MainLoop
				
StartCountDown:	jsr		DoCountDown
				bra		LittleInit
				
IncSeconds:		psha
				ldaa	Ones		;get the current second count
				adda	#0x01		;add one second
				daa					;Roll over at 9
				anda	#0b00001111
				staa	Ones		;save it
				jsr		CalcAndDisp
				ldaa	#0x01
				staa	SW0Previous
				pula
				bra		DoneIncSecs
				
IncTens:		psha
				ldaa	Tens		;get the current tens count
				adda	#0x01		;add ten seconds
				daa					;Roll over at 9
				anda	#0b00001111
				staa	Tens		;save it
				jsr		CalcAndDisp
				ldaa	#0x01
				staa	SW1Previous
				pula
				bra		DoneIncTens
				
DoCountDown:	psha
				pshb
				jsr		TurnOnLed2
CountDownLoop:	jsr		CalcAndDisp
				ldab	#100		;delay 1s
				jsr		doDelay
				ldaa	Ones		;if seconds = 0
				bne		DecSecs
				ldab	Tens			;if 0 = tens
				bne		DecTens
				bra		DoneCountDown				;done
										;else
DecTens:		ldaa	#9					;9->seconds
				staa	Ones
				subb	#1					;dec tens
				stab	Tens
				bra		CountDownLoop
									;else					
DecSecs:		suba	#1				;dec seconds
				staa	Ones
				bra		CountDownLoop
				
DoneCountDown:
				jsr		TurnOffLed2
				pulb
				pula
				rts

CalcAndDisp:	psha
				pshb
				ldaa	Ones		;get the counters
				ldab	Tens
				lslb				;move b to the MS nibble
				lslb
				lslb
				lslb
				aba					;combine the timer
				staa	led			;show the result
				pula
				pulb
				rts
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

.include 'porta.asm'
.include 'delay.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'
				
;DumbTable:		.db		0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15

;Copyright:		.ascii	"Copyright (c) 2002 by the person that wrote it. "
;				.asciz	"Hands off."

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

SW0Previous:	.ds	1
SW1Previous:	.ds	1
SW2Previous:	.ds 1
Ones:			.ds 1
Tens:			.ds 1

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				