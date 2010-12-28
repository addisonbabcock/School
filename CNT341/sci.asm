;****************************************************************************
;*	sci.asm																	*
;*																			*
;*	Subroutines to interface with the serial port							*
;****************************************************************************

;.--------------------------------------------------------------------------.
;|                           EQUATES SECTION								|
;`--------------------------------------------------------------------------'

BAUD		=	0x102B			; Baud Select Register
SCCR1		=	0x102C			; SCI Control Register 1
SCCR2		=	0x102D			; SCI Control Register 2
SCSR		=	0x102E			; SCI Status Register
SCDR		=	0x102F			; SCI Data Register

;.--------------------------------------------------------------------------.
;|							Subroutines										|
;`--------------------------------------------------------------------------'

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciInit																	;
;	Purpose		: Initialize the serial port to 8N1 @ 9600 baud				;
;	Modifies	: Nothing													;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 27/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

sciInit:
				psha
				pshx
				ldaa	#0b00000011		;9600 baud
				staa	BAUD
				ldaa	#0	
				staa	SCCR1
				ldaa	#0x0C
				staa	SCCR2
				pulx					;done
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciRXPeek																	;
;	Purpose		: Check to see if a byte has been recieved on the serial	;
;	Modifies	: B															;
;	Accepts		: Nothing													;
;	Return		: If a byte is waiting to be read, return 1 in B			;
;					Otherwise returns 0 in B								;
;	Last mod	: 28/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

sciRXPeek:
				pshx
				ldx		#SCSR
				brclr	0,x,#0b00100000,sciRxRegEmpty
				ldab	#0x01
				pulx
				rts
sciRxRegEmpty:	ldab	#0x00
				pulx
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciRXByte																	;
;	Purpose		: Recieves a byte from the serial port. Blocks the program	;
;					until a byte is recieved								;
;	Modifies	: sciRxBuf													;
;	Accepts		: Nothing													;
;	Return		: The byte recieved in sciRxBuf								;
;	Last mod	: 28/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

sciRXByte:		pshb
sciRXByteLoop:	jsr		sciRXPeek
				cmpb	#0x00
				beq		sciRXByteLoop
				ldab	SCDR
				stab	sciRxBuf
				pulb
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciTxByte																	;
;	Purpose		: Sends a byte through the serial port. Blocks the program	;
;					until the byte is sent									;
;	Modifies	: Nothing													;
;	Accepts		: The byte to be sent in sciTxBuf							;
;	Return		: Nothing													;
;	Last mod	: 28/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

sciTxByte:		psha
				pshx
				ldx		#SCSR
				
				;Wait until the link is clear
sciTxByteLoop:	brclr	0,x,#0b10000000,sciTxByteLoop
				ldaa	sciTxBuf	;Link is clear, send the byte
				staa	SCDR
				pulx				;Done
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciTXString																;
;	Purpose		: Transmits a null terminated string through the sci		;
;	Modifies	: Nothing													;
;	Accepts		: A pointer to a null terminated string in sciTxString		;
;	Return		: Nothing													;
;	Last mod	: 28/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

sciTxString:
				psha
				pshx
				ldx		sciTxSz
				
sciTxStringLoop:
				ldaa	0,x
				beq		sciTxStringDone
				staa	sciTxBuf
				jsr		sciTxByte
				inx
				bra		sciTxStringLoop
				
sciTxStringDone:
				pulx
				pula
				rts