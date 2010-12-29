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
				
				;sci_init placed inline
				ldaa	#0x5C	;Enable the SPI and set the speed
				staa	SPCR
				
				ldaa	#0x38	;Make the proper pins inputs/outputs
				staa	DDRD
				;end of sci_init
				
				ldx		#PORTD	;point x to PORTD
				
mainloop:
				ldd		#2048	;point #1
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
												
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#2447	;point #2
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
												
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#2831	;point #3
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3185	;point #4
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3496	;point #5
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3750	;point #6
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3940	;point #7
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#4056	;point #8
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#4095	;point #9
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
								
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#4056	;point #10
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3940	;point #11
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3750	;point #12
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3496	;point #13
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#3185	;point #14
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#2831	;point #15
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#2447	;point #16
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#2048	;point #17
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#1648	;point #18
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#1264	;point #19
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#910	;point #20
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#599	;point #21
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#345	;point #22
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#155	;point #23
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#39		;point #24
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#0		;point #25
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#39		;point #26
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#155	;point #27
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#345	;point #28
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#599	;point #29
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#910	;point #30
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#1264	;point #31
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				ldd		#1648	;point #32
				staa	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				stab	SPDR
				
				brn		.
				brn		.
				brn		.
				brn		.
				nop
				ldaa	SPSR
				
				;clear and set SS* to change the DAC output
				bclr	0,x,#0b00100000
				bset	0,x,#0b00100000
				
				jmp		mainloop	;start again
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
;.include 'delay.asm'
;.include 'sci.asm'
;.include 'LCD.asm'
;.include 'spi.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

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
				.endif
				