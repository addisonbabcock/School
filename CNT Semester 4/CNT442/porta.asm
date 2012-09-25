;****************************************************************************
;*	porta.asm																*
;*																			*
;*	porta subroutines for the HC11											*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|							EQUATES SECTION									|
;`--------------------------------------------------------------------------'

PORTA		=	0x1000			; porta data
PACTL		=	0x1026			; portA control

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; makebit7output															;
;	Purpose		: Configures porta to use bit 7 as an output (led2)			;
;	Modifies	: porta control status bit 7								;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
makebit7output:	psha
				ldaa	pactl
				oraa	#0x80
				staa	pactl
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; makebit7input																;
;	Purpose		: Configures porta to use bit 7 as an input (led2)			;
;	Modifies	: porta control status bit 7								;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
makebit7input:	psha
				ldaa	pactl
				anda	#0b01111111
				staa	pactl
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; TurnOnLedx																;
;	Purpose		: Turns on LEDx, (for led2, assumes bit 7 is configured for	;
;				  use as an output											;
;	Modifies	: LEDx status												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
turnonled0:		psha
				ldaa	porta
				oraa	#0b00100000
				staa	pORTA
				pula
				rts
				
turnonled1:		psha
				ldaa	porta
				oraa	#0b01000000
				staa	porta
				pula
				rts
				
turnonled2:		psha
				ldaa	porta
				oraa	#0b10000000
				staa	porta
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; TurnOffLedx																;
;	Purpose		: Turns off LEDx, (for led2, assumes bit 7 is configured	;
;				  for use as an output										;
;	Modifies	: LEDx status												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
turnoffled0:	psha
				ldaa	porta
				anda	#0b11011111
				staa	porta
				pula
				rts
				
turnoffled1:	psha
				ldaa	porta
				anda	#0b10111111
				staa	porta
				pula
				rts
				
turnoffled2:	psha
				ldaa	porta
				anda	#0b01111111
				staa	porta
				pula
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ToggleLedx																;
;	Purpose		: Toggles LEDx (for led2, assumes bit 7 is configured		;
;				  for use as an output)
;	Modifies	: LEDx status												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;	
toggleled0:		psha
				ldaa	porta
				anda	#0b00100000
				beq		turnon0
				jsr		turnoffled0
				pula
				rts
turnon0:		jsr		turnonled0
				pula
				rts
				
toggleled1:		psha
				ldaa	porta
				anda	#0b01000000
				beq		turnon1
				jsr		turnoffled1
				pula
				rts
turnon1:		jsr		turnonled1
				pula
				rts
				
toggleled2:		psha
				ldaa	porta
				anda	#0b10000000
				beq		turnon2
				jsr		turnoffled2
				pula
				rts
turnon2:		jsr		turnonled2
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; GetSWx																	;
;	Purpose		: Gets the status of SWx									;
;	Modifies	: B															;
;	Accepts		: Nothing													;
;	Return		: The status of SWx in B, 01 = down, 00 = up				;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;						
getsw0:			psha
				ldaa	porta
				anda	#0x01
				beq		SWDOWN
				bra		swup
				
getsw1:			psha
				ldaa	porta
				anda	#0b00000010
				beq		SWDOWN
				bra		swup
				
getsw2:			psha
				ldaa	porta
				anda	#0b00000100
				beq		SWDOWN
				bra		swup
				
swdown:			ldab	#0x01
				pula
				rts
				
swup:			ldab	#0x00
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; DebounceSWx																;
;	Purpose		: Returns the debounced state of Switch X					;
;	Modifies	: B															;
;	Accepts		: Nothing													;
;	Return		: The debounced status of SWx in B, 01 = down, 00 = up		;
;	Last mod	: 13/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
debouncesw0:	psha			;save a
				ldaa	#0x02
sw0loop:		pshb
				ldab	#0x01
				jsr		dodelay
				pula
				jsr		getsw0
				cba				;compare the previous state of the 
								;switch to the current state
				beq		return  ;the states are the same
				bra		sw0loop ;the states are different
				
debouncesw1:	psha			;save a
				ldaa	#0x02
sw1loop:		pshb
				ldab	#0x01
				jsr		dodelay
				pula
				jsr		getsw1
				cba				;compare the previous state of the 
								;switch to the current state
				beq		return  ;the states are the same
				bra		sw1loop ;the states are different
				
debouncesw2:	psha			;save a
				ldaa	#0x02
sw2loop:		pshb
				ldab	#0x01
				jsr		dodelay
				pula
				jsr		getsw2
				cba				;compare the previous state of the 
								;switch to the current state
				beq		return  ;the states are the same
				bra		sw2loop ;the states are different
				
return:			pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; WaitForSW																	;
;	Purpose		: Waits for one of the PORTA switches to be pressed			;
;	Modifies	: B															;
;	Accepts		: Nothing													;
;	Return		: Which switch was pressed in B. SW0 = 0, SW1 = 1, SW2 = 2	;
;	Last mod	: 14/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
WaitForSW:		jsr		WaitForSWUp
WaitForSWLoop:	jsr		debounceSW0
				cmpb	#0x01
				beq		returnSW0
				jsr		debounceSW1
				cmpb	#0x01
				beq		returnSW1
				jsr		debounceSW2
				cmpb	#0x01
				beq		returnSW2
				bra		WaitForSWLoop
				
returnSW0:		ldab	#0x00
				rts
				
returnSW1:		ldab	#0x01
				rts
				
returnSW2:		ldab	#0x02
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; WaitForSWUp																;
;	Purpose		: Waits for all of the PORTA switches to be released		;
;	Modifies	: Nothing													;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 14/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
WaitForSWUp:	pshb
WaitForSWUpLoop:
				jsr		DebounceSW0
				cmpb	#0x00
				bne		WaitForSWUpLoop
				jsr		DebounceSW1
				cmpb	#0x00
				bne		WaitForSWUpLoop
				jsr		DebounceSW2
				cmpb	#0x00
				bne		WaitForSWUpLoop
				pulb
				rts