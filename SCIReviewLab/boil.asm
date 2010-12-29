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

LCDIR		=	0x8000			; LCD instruction register
LCDDR		=	0x8001			; LCD data register

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
				
				jsr		LCDInit		;Init the lcd and load the chars
				ldab	#64
				ldx		#Char0
				jsr		LCDCreate
				
				clra
				jsr		LCDHome		;Set cursor to top left
				
				;Show all the chars on the LCD				
Show_Chars:		cmpa	#8			;Done showing the chars?
				beq		Done_Showing_Chars
				jsr		LCDSetChar	;No, send another one
				inca
				bra		Show_Chars
				
Done_Showing_Chars:
				clra
				
loop:			
				psha
				ldaa	#0x4F			;Set the cursor to the bottom right
				jsr		LCDGoDDData
				pula
				jsr		LCDSetChar	;Send a character
				
				ldab	#20			;Delay for 200 ms
				jsr		DoDelay
				
				inca				;Prepare for the next char
				cmpa #8				;A should wrap at 8
				beq		Done_Showing_Chars
				bra		loop
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
.include 'delay.asm'
;.include 'sci.asm'
.include 'LCD.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Char0:			.db		0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04
Char1:			.db		0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00
Char2:			.db		0x00, 0x00, 0x04, 0x00, 0x00, 0x04, 0x00, 0x00
Char3:			.db		0x00, 0x00, 0x00, 0x04, 0x04, 0x00, 0x00, 0x00
Char4:			.db		0x00, 0x00, 0x00, 0x0E, 0x0E, 0x00, 0x00, 0x00
Char5:			.db		0x00, 0x00, 0x0E, 0x1B, 0x1B, 0x0E, 0x00, 0x00
Char6:			.db		0x0E, 0x1F, 0x00, 0x11, 0x11, 0x00, 0x1F, 0x0E
Char7:			.db		0x1B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1B

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

sciRxBuf:		.ds	1		;Byte recieve through the SCI
sciTxBuf:		.ds	1		;Send this byte through the SCI
sciTxSz:		.ds	2		;Pointer to the null terminated string to be sent

countflag:		.ds	1		;Are we counting ATM?
count:			.ds	1		;How high have we counted

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				