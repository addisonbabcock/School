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

SPDR			=	0x102A	;Data Register
SPCR			=	0x1028	;Control Register
SPSR			=	0x1029	;Status Register
DDRD			=	0x1009	;Data Direction Register
PORTD			=	0x1008	;Port D

TCNT		=	0x100E		;16-bit timer count register
TOC1		=	0x1016		;timer output compare 1
TOC2		=	0x1018		;timer output compare 2
TOC3		=	0x101A		;timer output compare 3
TOC4		=	0x101C		;timer output compare 4
TCTL1		=	0x1020		;timer control register 1
TCTL2		=	0x1021		;timer control register 2
TMSK1		=	0x1022		;timer mask register 1
TFLG1		=	0x1023		;timer flag register 1
TMSK2		=	0x1024		;timer mask register 2
TFLG2		=	0x1025		;timer flag register 2

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
				
				ldaa	#0b01010100			;set OC2, 3 and 4 to toggle
				staa	TCTL1
				ldaa	#0b01110000			;enable IRQ from OC2, 3 and 4
				staa	TMSK1
				
				ldd		TCNT				;set up 3kHz wave
				addd	TOC3_INC			
				std		TOC3
				ldd		TCNT				;set up 25 Hz wave
				addd	TOC2_INC			
				std		TOC2
				ldd		TCNT				;set up 500Hz 80% wave
				addd	TOC4_INC1
				std		TOC4

				cli							;turn on IRQs				
				jmp		ROMSTART			;return to normal duties
				
OC2I_ISR:
				ldaa	#0b01000000			;ACK Interrupt
				staa	TFLG1
				
				ldd		TOC2				;set up next interrupt
				addd	TOC2_INC
				std		TOC2
				
				rti
				
OC3I_ISR:
				ldaa	#0b00100000			;ACK Interrupt
				staa	TFLG1
				
				ldd		TOC3				;set up next interrupt
				addd	TOC3_INC
				std		TOC3
				
				rti
				
OC4I_ISR1:
				ldaa	#0b00010000			;Ack Interrupt
				staa	TFLG1
				
				ldd		#OC4I_ISR2			;alternate ISRs
				std		0x7fe2
				
				ldd		TOC4				;set up next interrupt
				addd	TOC4_INC2
				std		TOC4
				
				rti
				
OC4I_ISR2:		
				ldaa	#0b00010000			;Ack interrupt
				staa	TFLG1
			
				ldd		#OC4I_ISR1			;alternate ISRs
				std		0x7fe2
				
				ldd		TOC4				;set up next interrupt
				addd	TOC4_INC1
				std		TOC4
				
				rti
				
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
;.include 'delay.asm'
;.include 'sci.asm'
;.include 'LCD.asm'
;.include 'spi.asm'
;.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

TOC2_INC:		.dw		24576		;((1/25 Hz)/813.802 ns)/2 = 24576.0
TOC3_INC:		.dw		205			;((1/3000 Hz)/813.802ns)/2 = 204.8
TOC4_INC1:		.dw		1966		;500Hz = 2ms per cycle, 1.6ms high
									;and 0.4ms low. 1.6ms = 1966.1 cycles
TOC4_INC2:		.dw		492			;0.4ms = 492 cycles

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

;sciRxSz:		.ds	23		;String to be recieved from the SCI
;sciRxBuf:		.ds	1		;Byte recieve through the SCI
;sciTxBuf:		.ds	1		;Send this byte through the SCI
;sciTxSz:		.ds	2		;Pointer to the null terminated string to be sent

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
					.org	0xffe4
					.dw		OC3I_ISR
					.dw		OC2I_ISR
				.endif

;				.if TARGETROM == 0
					.org	0x7fe2
					.dw		OC4I_ISR1
					.dw		OC3I_ISR
					.dw		OC2I_ISR
;				.endif