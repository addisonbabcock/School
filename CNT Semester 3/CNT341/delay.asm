;****************************************************************************
;*	delay.asm																*
;*																			*
;*	a subroutine to waste x ms												*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; dodelay																	;
;	Purpose		: Delays B * 10 milliseconds								;
;	Modifies	: Nothing													;
;	Accepts		: B - How many 10 milliseconds to delay						;
;	Return		: Nothing													;
;	Last mod	: 13/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

				;delays B * 10 ms
dodelay:		pshx
				psha
				tba
				beq		donedelay
delayouter:		ldx		#0x0800
delayloop:		dex
				bne		delayloop
				deca
				bne		delayouter
donedelay:		pula
				pulx
				rts