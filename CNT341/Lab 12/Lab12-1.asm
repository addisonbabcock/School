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
				
				;the LED is used for debuging purposes in this program
				;it indicates which part of the unlocking sequence we are in
				;0x = part x of either sequence
				;1x = part x of locking sequence
				;2x = part x of unlocking sequence
				
locksafe:		jsr		turnonled1			;set the state of the safe as locked
				jsr		turnoffled0
				bra		STAGE1
				
Stage1:			ldaa	#0x01
				staa	led
				jsr		WaitForSW
				cmpb	#0x00
				beq		Stage2				;A 0 was entered, go to the next part of the sequence
				bra		Stage1				;1 or 2 was entered, invalid sequence
				
Stage2:			ldaa	#0x02				;0 was entered previously
				staa	led
				jsr		WaitForSW
				cmpb	#0x02
				beq		UnlockStage3		;if a 2 was entered, continue checking for unlock command
				cmpb	#0x00
				beq		LockStage3			;if another 0 was entered, continue checking for lock safe command
				bra		Stage1				;1 was entered, invalid sequence
				
LockStage3:		ldaa	#0x13				;0 was entered previously
				staa	led
				jsr		WaitForSW
				cmpb	#0x00
				beq		LockSafe			;if a 0 was entered, lock safe command was entered
				cmpb	#0x02
				beq		UnlockStage3		;if a 2 was entered, this is actually the unlocking sequence
				bra		Stage1				;1 entered, invalid sequence
				
UnlockStage3:	ldaa	#0x23				;2 was entered previously
				staa	led
				jsr		WaitForSW
				cmpb	#0x02
				beq		UnlockStage4		;if another 2 was entered, continue checking for unlocking
				cmpb	#0x00
				beq		Stage2				;a 0 was entered, start the sequence at stage 2
				bra		stage1				;a 1 was entered, invalid sequence
				
unlockstage4:	ldaa	#0x24				;2 was entered previously
				staa	led
				jsr		waitforsw
				cmpb	#0x01
				beq		unlockstage5		;if a 1 was entered, continue checking for unlocking
				bra		stage1
				
unlockstage5:	ldaa	#0x25				;1 was entered previously
				staa	led
				jsr		waitforsw
				cmpb	#0x00
				beq		unlocksafe			;unlock command was succesfully entered
				bra		stage1
				
unlocksafe:		jsr		turnoffled1			;set the state of the safe as unlocked
				jsr		turnonled0
				bra		stage1
				
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
				