;****************************************************************************
;*	LCD.asm																	*
;*																			*
;*	Subroutines for writing to the LCD										*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LcdWait																	;
;	Purpose		: Waits until the LCD is free.								;
;	Modifies	: Nothing													;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDWait:		psha
LCDWaitLoop:	ldaa	LCDIR
				bmi		LCDWaitLoop	;wait if the MSB of LCDIR is set
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LcdInit																	;
;	Purpose		: Fully inits the LCD										;
;	Modifies	: LCD is cleared with the cursor set to 0,0					;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDInit:		
				psha
				jsr		LCDWait
				ldaa	#0x38		;8 bit, 2 line, 5*8 chars
				staa	LCDIR
				
				jsr		LCDWait
				ldaa	#0x0C		;display on, cursor off, no blink
				staa	LCDIR
				
				jsr		LCDWait
				ldaa	#0x06		; increment, no shift
				staa	LCDIR
				
				jsr		LCDReset	;clear and home the LCD
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDReset																	;
;	Purpose		: Resets the LCD to be cleared and the cursor is at home	;
;	Modifies	: LCD is cleared with the cursor set to 0,0					;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDReset:		
				jsr		LCDClear
				jsr		LCDHome
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDClear																	;
;	Purpose		: Clears the LCD											;
;	Modifies	: LCD is cleared											;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDClear:
				psha
				jsr		LCDWait
				ldaa	#0x01		;Clear display command
				staa	LCDIR
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDHome																	;
;	Purpose		: Sets the LCD cursor to home								;
;	Modifies	: LCD cursor												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDHome:
				psha
				jsr		LCDWait
				ldaa	#0x02		;Return home command
				staa	LCDIR
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDSetChar																;
;	Purpose		: Sends a character to the LCD								;
;	Modifies	: LCD cursor, LCD display									;
;	Accepts		: The character to be sent in A								;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDSetChar:
				jsr		LCDWait
				staa	LCDDR
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDShiftRight																;
;	Purpose		: Shifts the LCD display to the right						;
;	Modifies	: Position of the LCD window								;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDShiftRight:	
				psha
				ldaa	#0b00011100
				jsr		LCDWait
				staa	LCDIR
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDShiftLeft																;
;	Purpose		: Shifts the LCD display to the left						;
;	Modifies	: Position of the LCD window								;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDShiftLeft:
				psha
				ldaa	#0b00011000
				jsr		LCDWait
				staa	LCDIR
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDGoCGData																;
;	Purpose		: Sets the LCD to accept CG data							;
;	Modifies	: What the LCD is expecting to recieve						;
;	Accepts		: The address of the character that will be modified in A	;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDGoCGData:
				psha
				anda	#0x7F			;clear the msbit
				oraa	#0b01000000		;set bit 7
				jsr		LCDWait
				staa	LCDIR
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDGoDDData																;
;	Purpose		: Sets the LCD to accept display data						;
;	Modifies	: What the LCD is expecting to recieve						;
;	Accepts		: The address of the character that will be displayed		;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDGoDDData:
				psha
				oraa	#0b10000000		;set the msbit
				jsr		LCDWait
				staa	LCDIR
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDDDString																;
;	Purpose		: Send a string to the LCD									;
;	Modifies	: LCD cursor, LCD display									;
;	Accepts		: The address of the start of the string to be sent in X	;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDDDString:
				pshx
				psha
LCDDDStringLoop:
				ldaa	0,x
				cmpa	#0
				beq		LCDDDStringDone
				jsr		LCDSetChar
				inx
				bra		LCDDDStringLoop
LCDDDStringDone:
				pula
				pulx
				rts

;Is this routine needed????
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; LCDSetDDAddr																;
;	Purpose		: Sets the display data memory location						;
;	Modifies	: LCD cursor												;
;	Accepts		: LCD memory location in A									;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
LCDSetDDAddr:
				rts