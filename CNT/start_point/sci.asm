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
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; sciRXString																;
;	Purpose		: Recieves a null terminated string from the SCI. Requires	;
;				  a buffer of max length + 3 bytes.							;
;	Modifies	: The contents of the buffer.								;
;	Accepts		: A pointer to a null terminated string in X, the max		;
;				  string length in B.										;
;	Return		: Nothing													;
;	Last mod	: 17/09/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
sciRXString:
				pshy			;Save the registers we will be using
				pshx
				pshb
				psha
				
				pshx
				puly			;Set x to point to the start of characters
				inx				;and y to point to the start of the buffer
				inx
				
				stab	0,y		;set the max chars for the buffer
				
				clr		1,y		;there are no chars in the buffer
				
sciRxStringNext:
				jsr		sciRxByte	;get a char from the SCI
				ldaa	sciRxBuf
				
				cmpa	#0x0D	;Is the new char a return key?
				beq		sciRXStringReturn
				
				cmpa	#0x08	;Is the new char a backspace?
				beq		sciRxStringBS
				
				cmpb	1,y		;compare max size to current len
				beq		sciRxStringNext	;string is full, get another char
				
				staa	0,x		;buffer can accept another char, save it
				staa	sciTxBuf
				jsr		sciTxByte	;echo the byte to the screen
				inx				;move x to point to the next empty char
				ldaa	1,y		;increment the character count
				inca
				staa	1,y
				
				bra		sciRxStringNext		;get another char
				
sciRxStringBS:
				ldaa	1,y
				beq		sciRxStringNext	;dont erase from an empty buffer
				
				dex			;go a char back
				deca			;one less char in the counter
				staa	1,y
				
				pshx			;save x while we clean up the terminal
				ldx		#sciBackspace	;Erase the character on the screen
				stx		sciTxSz
				jsr		sciTxString
				pulx			;restore x to point to the character buffer
				
				bra		sciRxStringNext		;grab a new character
				
sciRxStringReturn:
				clr		0,x		;add the null
				
				pula			;restore registers
				pulb
				pulx
				puly
				
				rts				;done
				
				
;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'
sciBackspace:	.asciz	"\e[D\e[K"