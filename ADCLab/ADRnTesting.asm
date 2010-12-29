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

LCDIR		=	0x8000			; LCD instruction register
LCDDR		=	0x8001			; LCD data register

SPDR		=	0x102A			;Data Register
SPCR		=	0x1028			;Control Register
SPSR		=	0x1029			;Status Register
DDRD		=	0x1009			;Data Direction Register
PORTD		=	0x1008			;Port D

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
				
				jsr		adc_init
				jsr		spi_init
				
loop:			
				ldaa	#0
				staa	ADCTL
waitforCCF:		ldaa	ADCTL
				bpl		waitforCCF
				
				ldaa	ADR1				
				staa	LED					;show the hex value on the LED
				ldab	#100
				jsr		dodelay
				
				ldaa	ADR2
				staa	LED
				jsr		dodelay
				
				ldaa	ADR3
				staa	LED
				jsr		dodelay
				
				ldaa	ADR4
				staa	LED
				ldab	#200
				jsr		dodelay				
				
				bra		loop

				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
.include 'delay.asm'
;.include 'sci.asm'
;.include 'LCD.asm'
.include 'spi.asm'
.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Points:			.dw		2048,	2447,	2831,	3185,	3496,	3750
				.dw		3940,	4056,	4095,	4056,	3940,	3750
				.dw		3496,	3185,	2831,	2447,	2048,	1648
				.dw		1264,	910,	599,	345,	155,	39
				.dw		0,		39,		155,	345,	599,	910
				.dw		1264,	1648

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

sciRxSz:		.ds	23		;String to be recieved from the SCI
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
				