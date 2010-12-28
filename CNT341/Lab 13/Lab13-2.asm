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
				
Init:			jsr		sciInit			;init serial port
				ldx		#InitScreen		;Get the screen ready
				stx		sciTxSz
				jsr		sciTxString
				
MainLoop:		
				ldx		#DisplayStatus	;Get the display ready to show
				stx		sciTxSz			;PA status
				jsr		sciTxString
				
				;work on SW 2
				jsr		GetSW2
				cmpb	#0x01
				beq		SendSW2Down
				
				;SW2 is up, send the up message
				ldx		#Up
				stx		sciTxSz
				jsr		sciTxString
			
CheckSW1:		;Work on SW1
				jsr		GetSW1			;get the state of SW1
				cmpb	#0x01			;if Switch 1 is down
				beq		SendSW1Down		;send the down message
				
				;SW1 is up, send the up message
				ldx		#Up
				stx		sciTxSz
				jsr		sciTxString
				
CheckSW0:
				jsr		GetSW0			;get the state of SW0
				cmpb	#0x01			;if switch 0 is down
				beq		SendSW0Down		;send the down message

				;SW0 is up, send the up message
				ldx		#Up
				stx		sciTxSz
				jsr		sciTxString

DisplayHexStatus:
				ldx		#DisplayHexVal	;Prepare the terminal to 
				stx		sciTxSz			;display the hex value
				jsr		sciTxString
				jsr		ShowPAHex		;Show the hex value
				
DisplayBinStatus:
				ldx		#DisplayBinVal	;Prepare the terminal to
				stx		sciTxSz			;display the binary value
				jsr		sciTxString
				jsr		ShowPABin		;Show the binary value
				
				ldab	#25				
				jsr		dodelay			;wait 250ms before refreshing
				bra		MainLoop

SendSW2Down:	;SW2 is down, send the down message
				ldx		#Down
				stx		sciTxSz
				jsr		sciTxString
				bra		CheckSW1

SendSW1Down:	;SW1 is down, send the down message
				ldx		#Down
				stx		sciTxSz
				jsr		sciTxString
				bra		CheckSW0

SendSW0Down:	;SW0 is down, send the down message
				ldx		#Down
				stx		sciTxSz
				jsr		sciTxString
				bra		DisplayHexStatus

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ShowPAHex																	;
;	Purpose		: Displays the value found in PORTA over the SCI in hex		;
;	Modifies	: SCI terminal cursor										;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ShowPAHex:
				psha
				pshb
				ldaa	PORTA
				ldab	PORTA
				anda	#0xF0	;A will contain the MSByte of porta
				lsra
				lsra
				lsra
				lsra
				andb	#0x0F	;B will contain the LSByte of porta
				
				jsr		HexByteToASCII	;Turn A into an ASCII code
				staa	sciTxBuf		;Send A over the SCI
				jsr		sciTxByte
				
				tba						;Now working on the LSB
				jsr		HexByteToASCII	;Turn A into an ASCII code
				staa	sciTxBuf		;Send it over the SCI
				jsr		sciTxByte
				
				pulb		;Done
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; HexByteToASCII															;
;	Purpose		: Translates a hex byte to it's ascii code					;
;	Modifies	: A															;
;	Accepts		: lower nibble of A. This is the byte to be translated		;
;	Return		: The ASCII code in A										;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;			
HexByteToASCII:
				cmpa	#0x0A
				bge		UseLetters		;if a >= 0x0A, use the ascii value for A-F
				adda	#'0				;a < 0x0A, use ascii values for 0-9
				rts
UseLetters:		suba	#0x0A
				adda	#'A
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ShowPABin																	;
;	Purpose		: Translates and displays (sci) the value of PORTA in bin	;
;	Modifies	: Nothing													;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 04/04/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ShowPABin:		
				psha
				pshb
				ldaa	PORTA		;Get PORTA status
				ldab	#0x09		;Will we be looping through all 8 bits
				
ShowPABinLoop:
				decb
				beq		ShowPABinDone
				lsla					;Put the MSB of PA into the carry flag
				bcc		ShowPABinZero	;A zero was placed into the Carry flag, show a zero
				
				psha					;A one was placed into the carry flag, show a one
				ldaa	#'1
				staa	sciTxBuf
				jsr		sciTxByte
				pula
				bra		ShowPABinLoop	;Do the next byte
				
ShowPABinZero:
				psha					;The MSB of PA was 0, show a 0
				ldaa	#'0
				staa	sciTxBuf
				jsr		sciTxByte
				pula
				bra		ShowPABinLoop	;Do the next byte
				
ShowPABinDone:	
				pulb
				pula
				rts

				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

.include 'porta.asm'
.include 'delay.asm'
.include 'sci.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

InitScreen:		.ascii	"\e[2J\e[H\e[2B\e[35m"
				.ascii	"                            Addison Babcock, CNT2K"
				.ascii	"\e[31m\e[5;1f"
				.asciz	"Port A Status : \e[7;1fPort A Value  : "
DisplayStatus:	.asciz	"\e[5;17f\e[K\e[34m"
DisplayHexVal:	.asciz	"\e[7;17f\e[K\e[32m"
DisplayBinVal:	.asciz	"\e[7;20f\e[K\e[36m"
Down:			.asciz	"DOWN "
Up:				.asciz	"UP "

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

sciRxBuf:		.ds	1		;Byte recieve through the SCI
sciTxBuf:		.ds	1		;Send this byte through the SCI
sciTxSz:		.ds	2		;Pointer to the null terminated string to be sent

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				