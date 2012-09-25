;****************************************************************************
;*	spi.asm																	*
;*																			*
;*	Subroutines to be used for sending data to the DAC over the SPI			*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|							Equates											|
;`--------------------------------------------------------------------------'

SPDR			=	0x102A	;Data Register
SPCR			=	0x1028	;Control Register
SPSR			=	0x1029	;Status Register
DDRD			=	0x1009	;Data Direction Register
PORTD			=	0x1008	;Port D

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; spi_init																	;
;	Purpose		: Initiliazes the SPI										;
;	Modifies	: SPCR, DDRD												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 13/09/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
spi_init:
				psha			;Save A
				
				ldaa	#0x5C	;Enable the SPI and set the speed
				staa	SPCR
				
				ldaa	#0x38	;Make the proper pins inputs/outputs
				staa	DDRD
				
				pula			;Restore registers and exit
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; spi_data																	;
;	Purpose		: Sends data contained in AccD to the DAC through the SPI	;
;	Modifies	: Output of the DAC											;
;	Accepts		: the lower 12 bits of AccD at 1 mV / bit					;
;	Return		: Nothing													;
;	Last mod	: 13/09/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
spi_data:
				pshx			;Save registers
				psha
				
				staa	SPDR	;Send first byte to the DAC
				
wait1:			ldaa	SPSR	;Block until everything is sent
				bpl		wait1
				
				stab	SPDR	;Send second byte to the DAC
				
wait2:			ldaa	SPSR	;Block until all the bits are sent
				bpl		wait2
				
				ldx		#PORTD	;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				pula			;done!
				pulx
				rts