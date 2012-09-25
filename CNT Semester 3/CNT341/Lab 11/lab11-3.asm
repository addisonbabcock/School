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
				
				ldaa	#0x00		;a will be used for previous state of SW0
				ldab	#0x00		;b will be used to track the current state of SW0
				ldx		#0x00		;0,x will be used to track button presses
				staa	0,x			;clear the push count
				
loop:			pshb				;put the previous state of the switch in a
				ldaa	0,x			;display the current push count on the led
				staa	led
				pula
				jsr		getsw0		;check the state of the switch
				cmpb	#0x01
				beq		sw0isdown
				bra		sw0isup
				
sw0isdown:		jsr		turnonled0
				cmpa	#0x00		;check for a switch transition
				beq		transition
				bra		loop
				
sw0isup:		jsr		turnoffled0
				bra		loop
				
transition:		jsr		toggleled1
				psha				;save a
				ldaa	0,x			;increment the push count
				adda	#0x01
				daa
				staa	0,x
				pula
				bra		loop
												
exit:			jmp		romstart
				
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

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				