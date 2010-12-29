;****************************************************************************
;*	adc.asm																	*
;*																			*
;*	Subroutines to be used for sending data to the DAC over the SPI			*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|							Equates											|
;`--------------------------------------------------------------------------'

OPTION		=	0x1039			;Option register, contains ADPU and CSEL
ADCTL		=	0x1030			;Control register for the A/D subsystem
ADR1		=	0x1031			;A/D result register 1
ADR2		=	0x1032			;A/D result register 2
ADR3		=	0x1033			;A/D result register 3
ADR4		=	0x1034			;A/D result register 4

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; adc_init																	;
;	Purpose		: Initiliazes the A/D converter subsystem					;
;	Modifies	: OPTION, turns on the charge pump and sets the A/D to use	;
;				  ECLK instead of the internal RC clock						;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: Oct 4 2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
adc_init:
				pshx			;Save x
				psha
				pshb
				
				ldx		#OPTION		;Turn on the A/D
				bset	0,x,#0x80
				
				;Wait at least 100 micro seconds for the charge pump
				;to initialize
				fdiv				;165 cycles
				fdiv				;need at least 130 or so
				fdiv
				fdiv
				fdiv
				
				pulb
				pula
				pulx
				rts
				

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; adc_sample																;
;	Purpose		: Samples channel 0 of the ADC and returns the value in	A	;
;	Modifies	: Accumulator A												;
;	Accepts		: Nothing													;
;	Return		: The value sampled in AccA									;
;	Last mod	: Oct 4 2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
adc_sample:
				ldaa	#0			;No scanning, no multiple channel
									;channel 0
									
				staa	ADCTL
adc_sample_delay:
				ldaa	ADCTL		;Wait for the CCF to be set
				bpl		adc_sample_delay
				
				ldaa	ADR4		;return best result
				rts