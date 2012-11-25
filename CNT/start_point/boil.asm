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

PORTA		=	0x1000			; Port A data
PACTL		=	0x1026			; Port A control

LCDIR		=	0x8000			; LCD instruction register
LCDDR		=	0x8001			; LCD data register

SPDR		=	0x102A			;Data Register
SPCR		=	0x1028			;Control Register
SPSR		=	0x1029			;Status Register
DDRD		=	0x1009			;Data Direction Register
PORTD		=	0x1008			;Port D

TCNT		=	0x100E			;16-bit timer count register
TOC1		=	0x1016			;timer output compare 1
TOC2		=	0x1018			;timer output compare 2
TOC3		=	0x101A			;timer output compare 3
TOC4		=	0x101C			;timer output compare 4
TCTL1		=	0x1020			;timer control register 1
TCTL2		=	0x1021			;timer control register 2
TMSK1		=	0x1022			;timer mask register 1
TFLG1		=	0x1023			;timer flag register 1
TMSK2		=	0x1024			;timer mask register 2
TFLG2		=	0x1025			;timer flag register 2

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
				
				jsr		sciInit
				jsr		adc_Init
				jsr		spi_init
				jsr		lcdInit
				
				ldx		#CharacterData
				ldab	#6*8
				jsr		LCDCreate		;Load the bar graph chars into CGRAM
				
				sei
				ldaa	#0b01000000			;set OC2 to toggle
				staa	TCTL1
				ldaa	#0b01000000			;enable IRQ from OC2
				staa	TMSK1
				
				clra						;init a couple vars
				staa	Ticks
				staa	RefreshTime
				staa	PumpRefreshTime
				
				ldd		TCNT
				addd	TOC2_Inc
				std		TOC2
				
				;get the initial speed from the user
				ldd		#SpeedPrompt		;show the prompt
				std		sciTxSz
				jsr		sciTxString
				
				ldx		#sciRxSz			;get 2 bytes saying how fast to go
				ldab	#2
				jsr		sciRxString
				
				;we now have the ascii representation of the desired speed
				;in hex. now we need to decode the ascii and combine the 2
				;chars to form 1 byte
				ldaa	2,x					;load both chars
				ldab	3,x
				
				jsr		ASCIIByteToHex		;turn A into 4 bit hex
				psha						;swap A and B
				pshb
				pula
				pulb
				jsr		ASCIIByteToHex
				
				lslb						;push B into the upper nibble
				lslb
				lslb
				lslb
				
				aba							;mash the two together
				staa	DesSpeed			;save the result
				;desired speed set!;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
				
				;get the initial time;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
				ldd		#TimePrompt			;clear the screen
				std		sciTxSz				;and show a prompt
				jsr		sciTxString
								
				ldx		#sciRxSz			;get the current time
				ldab	#8					;from the user in HH:MM:SS
				jsr		sciRxString
				
				;we now have the time of day from the user stored the RxBuffer
				;in ASCII, to be usable we need to decode it into BCD and store
				;the result in the appropriate variables
				
				ldaa	2,x					;adjust the top nibble of hours
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Hours
				
				ldaa	3,x					;adjust the bottom nibble of hours
				suba	#'0
				
				oraa	Hours				;add the top and bottom nibbles
				staa	Hours
				
				ldaa	5,x					;adjust the top nibble of minutes
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Minutes
				
				ldaa	6,x					;adjust the bottom nibble of minutes
				suba	#'0
				
				oraa	Minutes				;add the top and bottom nibbles
				staa	Minutes
				
				ldaa	8,x					;adjust the top nibble of seconds
				suba	#'0
				lsla
				lsla
				lsla
				lsla
				staa	Seconds
				
				ldaa	9,x					;adjust the bottom nibble of seconds
				suba	#'0
				
				oraa	Seconds
				staa	Seconds	
				
				;time initialized!	;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
				
				ldaa	#0
				staa	TxPend				;start TxTask as free
				ldaa	#1
				staa	Repaint				;show the screen
				staa	RefreshTime			;show the time
				staa	NewSpeed			;show the speed
				staa	PumpRefreshTime
				
				ldaa	#0x23
				staa	PumpHours
				ldaa	#0x59
				staa	PumpMinutes
				ldaa	#0x50
				staa	PumpSeconds
				ldaa	#0x0
				staa	PumpTicks
				
				;the time of day has been set up, let the real time loop go!
				cli							;all set! GO!
				wai
				
loop:			
				jsr		TurnOnLED0			;activity on!
				jsr		Task1ShowTime		;show the time
				jsr		Task2SCITx			;send stuff over SCI
				jsr		Task3RepaintScreen	;show a fancy screen
				jsr		Task4SCIRx			;get input
				jsr		Task5Status			;update the status of the screen
				;jsr		Task6MotorControl	;update the motors speed
				;jsr		Task7ShowVoltages	;bar graph on the LCD
				jsr		Task8PumpTimer
				jsr		Task9ShowPumpTime
				jsr		TurnOffLED0			;activity off
				wai
				bra		loop
				
Task1ShowTime:
				ldaa	RefreshTime
				cmpa	#1
				bne		DoneDisplayTime		;wait for a tick
				
				ldaa	TxPend
				cmpa	#0
				bne		DoneDisplayTime		;wait for the TxTask to be free
				
				ldaa	#1
				staa	TxPend				;task 1 is now using TxTask

				ldy		#CursorToTimePosition	;y will point at the const data
				ldx		#TxBuffer			;x will point at the TxBuffer

Task1BuildBuffer:
				ldaa	0,y					;load a byte from the const data
				cmpa	#0
				beq		Task1DoneBuildingBuffer	;null char?
				
				staa	0,x					;char is not null, save it
				iny							;and move on to the next char
				inx
				bra		Task1BuildBuffer
				
Task1DoneBuildingBuffer:
				ldaa	Hours				;translate the hours to ASCII
				jsr		BCDByteToASCII	
				
				staa	0,x					;save the ASCII to the TxBuffer
				stab	1,x
				
				ldaa	#':					;seperate with a colon
				staa	2,x
				
				ldaa	Minutes				;translate minutes to ASCII
				jsr		BCDByteToASCII
				
				staa	3,x					;save the ASCII to the TxBuffer
				stab	4,x
				
				ldaa	#':					;seperate with a colon
				staa	5,x
				
				ldaa	Seconds
				jsr		BCDByteToASCII		;translate seconds to ASCII
				
				staa	6,x					;save the ASCII to the TxBuffer
				stab	7,x
				
				clr		8,x					;set the null char
				clr		RefreshTime			;done refreshing time
				
				ldx		#TxBuffer			;tell the SCI task where to go
				stx		TxPointer
DoneDisplayTime:
				rts	
				
Task2SCITx:
				ldaa	TxPend				;is there nothing to send?
				cmpa	#0
				beq		Task2Done
				
				ldaa	SCSR				;is the SCI ready?
				anda	#0b10000000
				cmpa	#0
				beq		Task2Done
				
				ldx		TxPointer			;get the next byte
				ldaa	0,x

				staa	SCDR				;send the byte

				cmpa	#0					;is this the end of the string?
				beq		Task2Null

				inx
				stx		TxPointer
				bra		Task2Done
				
Task2Null:
				clra						;done transmitting, clear pending flag
				staa	TxPend

Task2Done:
				rts							;done all we can do this tick

Task3RepaintScreen:
				ldaa	Repaint				;is there a need to repaint?
				cmpa	#1
				bne		Task3Done
				
				ldaa	TxPend				;can we repaint?
				cmpa	#0
				bne		Task3Done
				
				;all checks passed,proceed with screen repainting
				ldaa	#3
				staa	TxPend
				
				ldx		#StaticScreen
				stx		TxPointer
				
				ldaa	#0					;repaint has been set up,
				staa	Repaint				;dont do it again!
				
				ldaa	#0x00				;"Vi:"
				jsr		LCDGoDDData	
				ldaa	#'V
				jsr		LCDSetChar
				ldaa	#'i
				jsr		LCDSetChar
				ldaa	#':
				jsr		LCDSetChar
				
				ldaa	#0x40				;"Vo:"
				jsr		LCDGoDDData
				ldaa	#'V
				jsr		LCDSetChar
				ldaa	#'o
				jsr		LCDSetChar
				ldaa	#':
				jsr		LCDSetChar
Task3Done:
				rts
				
Task4SCIRx:
				ldaa	SCSR
				anda	#0b00100000			;check to see if key received
				cmpa	#0
				beq		Task4Done			;no key, dont do anything
				
				ldaa	SCDR
				anda	#0b11011111			;get key and uppercase
				
				cmpa	#'S
				beq		Task4Stop			;key was 'S'?
				
				cmpa	#'R
				beq		Task4Repaint		;key was 'R'?
				
				cmpa	#'U
				beq		Task4Up				;key was 'U'?
				
				cmpa	#'D
				beq		Task4Down			;key was 'D'?
				
				cmpa	#'Z
				beq		Task4Zero
				
				rts							;unknown key, do nothing
				
Task4Stop:									;stop command
				ldaa	#0
				staa	DesSpeed			;set the speed to 0,
				ldaa	#1
				staa	NewSpeed			;redraw the speed
				rts
Task4Repaint:								;repaint command
				ldaa	#1
				staa	Repaint				;redraw the screen
				staa	NewSpeed			;redraw the speed since it will be cleared
				rts
Task4Up:									;increase speed command
				ldaa	DesSpeed
				cmpa	#0xff				;check if speed is maxed
				beq		Task4Done
				inca						;increase speed
				staa	DesSpeed
				ldaa	#1
				staa	NewSpeed			;redraw speed
				rts
Task4Down:									;command to decrease speed
				ldaa	DesSpeed
				cmpa	#0
				beq		Task4Done			;check to see if stopped already
				deca						;not stopped, slow down
				staa	DesSpeed
				ldaa	#1
				staa	NewSpeed			;redraw speed
				rts
				
Task4Zero:
				ldaa	#0
				staa	PumpHours
				staa	PumpMinutes
				staa	PumpSeconds
				staa	PumpTicks
				staa	LED
				ldaa	#1
				staa	PumpRefreshTime
				rts
				
Task4Done:		rts

Task5Status:
				ldaa	NewSpeed			;do we need to update the speed?
				cmpa	#0
				beq		Task5Done
				
				ldaa	TxPend				;is TxTask busy?
				cmpa	#0
				bne		Task5Done
				
				ldaa	#5
				staa	TxPend
				
				;build cursor information
				ldy		#CursorToSpeedPosition	;y will point at the const data
				ldx		#TxBuffer			;x will point at the TxBuffer

Task5BuildBuffer:
				ldaa	0,y					;load a byte from the const data
				cmpa	#0
				beq		Task5DoneBuildingBuffer	;null char?
				
				staa	0,x					;char is not null, save it
				iny							;and move on to the next char
				inx
				bra		Task5BuildBuffer
				
Task5DoneBuildingBuffer:
				;cursor information built, now place the speed into the buffer
				ldaa	DesSpeed
				jsr		BCDByteToASCII		;routine also works with hex
				staa	0,x					;MS digit
				stab	1,x					;LS digit
				clra						;null char
				staa	2,x
				staa	NewSpeed			;speed sent to the screen
				
				ldx		#TxBuffer
				stx		TxPointer
				
Task5Done:		rts

Task8PumpTimer:
				jsr		getsw0
				cmpb	#0
				beq		Task8Done
				
				ldaa	PumpTicks				;do the ticks need to be wrapped?
				cmpa	#0x99
				bne		PDontWrapTicks
				
				clrb						;wrap ticks
				stab	PumpTicks
				
				ldaa	#1					;inform the mian program to kindly
				staa	PumpRefreshTime			;refresh the screen
				
				ldaa	PumpSeconds				;do the seconds need to be wrapped?
				cmpa	#0x59
				bne		PDontWrapSeconds
				
				clrb						;wrap seconds
				stab	PumpSeconds
				
				ldaa	PumpMinutes				;do the minutes need to be wrapped?
				cmpa	#0x59
				bne		PDontWrapMinutes
				
				clrb						;wrap minutes
				stab	PumpMinutes
				
				ldaa	PumpHours
				cmpa	#0x23
				bne		PDontWrapHours
				
				clrb						;wrap hours
				stab	PumpHours
				
				bra		Task8Done		;the time has been updated
				
PDontWrapTicks:
				adda	#1					;increment ticks and exit
				daa
				staa	PumpTicks
				bra		Task8Done
				
PDontWrapSeconds:
				adda	#1					;increment seconds and exit
				daa
				staa	PumpSeconds
				bra		Task8Done
				
PDontWrapMinutes:
				adda	#1					;increment minutes and exit
				daa
				staa	PumpMinutes
				bra		Task8Done
				
PDontWrapHours:
				adda	#1					;increment hours and exit
				daa
				staa	PumpHours
				bra		Task8Done
				
Task8Done:
				
				ldaa	PumpTicks
				staa	LED
				
				rts
				
Task9ShowPumpTime:
				ldaa	PumpRefreshTime
				cmpa	#1
				bne		PDoneDisplayTime		;wait for a tick
				
				ldaa	TxPend
				cmpa	#0
				bne		PDoneDisplayTime		;wait for the TxTask to be free
				
				ldaa	#1
				staa	TxPend				;task 1 is now using TxTask

				ldy		#CursorToPumpPosition	;y will point at the const data
				ldx		#TxBuffer			;x will point at the TxBuffer

Task9BuildBuffer:
				ldaa	0,y					;load a byte from the const data
				cmpa	#0
				beq		Task9DoneBuildingBuffer	;null char?
				
				staa	0,x					;char is not null, save it
				iny							;and move on to the next char
				inx
				bra		Task9BuildBuffer
				
Task9DoneBuildingBuffer:
				ldaa	PumpHours				;translate the hours to ASCII
				jsr		BCDByteToASCII	
				
				staa	0,x					;save the ASCII to the TxBuffer
				stab	1,x
				
				ldaa	#':					;seperate with a colon
				staa	2,x
				
				ldaa	PumpMinutes				;translate minutes to ASCII
				jsr		BCDByteToASCII
				
				staa	3,x					;save the ASCII to the TxBuffer
				stab	4,x
				
				ldaa	#':					;seperate with a colon
				staa	5,x
				
				ldaa	PumpSeconds
				jsr		BCDByteToASCII		;translate seconds to ASCII
				
				staa	6,x					;save the ASCII to the TxBuffer
				stab	7,x
				
				clr		8,x					;set the null char
				clr		PumpRefreshTime			;done refreshing time
				
				ldx		#TxBuffer			;tell the SCI task where to go
				stx		TxPointer
PDoneDisplayTime:
				rts	


OC2I_ISR:
				ldaa	#0b01000000			;ACK Interrupt
				staa	TFLG1
				
				ldaa	Ticks				;do the ticks need to be wrapped?
				cmpa	#0x99
				bne		DontWrapTicks
				
				clrb						;wrap ticks
				stab	Ticks
				
				ldaa	#1					;inform the mian program to kindly
				staa	RefreshTime			;refresh the screen
				
				ldaa	Seconds				;do the seconds need to be wrapped?
				cmpa	#0x59
				bne		DontWrapSeconds
				
				clrb						;wrap seconds
				stab	Seconds
				
				ldaa	Minutes				;do the minutes need to be wrapped?
				cmpa	#0x59
				bne		DontWrapMinutes
				
				clrb						;wrap minutes
				stab	Minutes
				
				ldaa	Hours
				cmpa	#0x23
				bne		DontWrapHours
				
				clrb						;wrap hours
				stab	Hours
				
				bra		OC2I_ISR_WrapUp		;the time has been updated
				
DontWrapTicks:
				adda	#1					;increment ticks and exit
				daa
				staa	Ticks
				bra		OC2I_ISR_WrapUp
				
DontWrapSeconds:
				adda	#1					;increment seconds and exit
				daa
				staa	Seconds
				bra		OC2I_ISR_WrapUp
				
DontWrapMinutes:
				adda	#1					;increment minutes and exit
				daa
				staa	Minutes
				bra		OC2I_ISR_WrapUp
				
DontWrapHours:
				adda	#1					;increment hours and exit
				daa
				staa	Hours
				bra		OC2I_ISR_WrapUp
				
OC2I_ISR_Wrapup:
				ldd		TOC2				;set up next interrupt
				addd	TOC2_INC
				std		TOC2
				
				;ldaa	Ticks
				;staa	LED
				
				rti
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; ASCIIByteToHex															;
;	Purpose		: Translates an ASCII byte to its hex code.					;
;	Modifies	: A															;
;	Accepts		: Accumulator A. This is the byte to be translated.			;
;	Return		: The hex value in the lower nibble of A					;
;	Last mod	: 09/19/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
ASCIIByteToHex:
				cmpa	#0x40			;0x30 - 0x39 is 0 - 9
				blo		IsANumber
				cmpa	#0x61
				blo		IsUpperCase		;0x41 - 0x... is A-Z
				
				suba	#0x57			;lower case
				rts
				
IsANumber:		
				suba	#0x30
				rts
				
IsUpperCase:	
				suba	#0x37
				rts

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; BCDByteToASCII															;
;	Purpose		: Translates a BCD byte into 2 ascii codes					;
;	Modifies	: A and B													;
;	Accepts		: A is the hex byte to be translated						;
;	Return		: The ASCII code in A for the upper nibble in A	and the		;
;		lower nibble in B.													;
;	Last mod	: Nov 7 2007 - Created										;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;				
BCDByteToASCII:
				tab
				lsra					;move MSB to LSB
				lsra
				lsra
				lsra
				cmpa	#0x0A
				bge		UseLettersA
				adda	#'0
				bra		TranslateB
UseLettersA:	suba	#0x0A
				adda	#'A
TranslateB:		andb	#0x0F			;clear out MSB, work on lower nibble
				cmpb	#0x0A
				bge		UseLettersB
				addb	#'0
				rts
UseLettersB:	subb	#0x0A
				addb	#'A
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; TurnOnLedx																;
;	Purpose		: Turns on LEDx, (for led2, assumes bit 7 is configured for	;
;				  use as an output											;
;	Modifies	: LEDx status												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
turnonled0:		psha
				ldaa	porta
				oraa	#0b00100000
				staa	pORTA
				pula
				rts
				
turnonled1:		psha
				ldaa	porta
				oraa	#0b01000000
				staa	porta
				pula
				rts
				
turnonled2:		psha
				ldaa	porta
				oraa	#0b10000000
				staa	porta
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; TurnOffLedx																;
;	Purpose		: Turns off LEDx, (for led2, assumes bit 7 is configured	;
;				  for use as an output										;
;	Modifies	: LEDx status												;
;	Accepts		: Nothing													;
;	Return		: Nothing													;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
turnoffled0:	psha
				ldaa	porta
				anda	#0b11011111
				staa	porta
				pula
				rts
				
turnoffled1:	psha
				ldaa	porta
				anda	#0b10111111
				staa	porta
				pula
				rts
				
turnoffled2:	psha
				ldaa	porta
				anda	#0b01111111
				staa	porta
				pula
				rts
				
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
; GetSWx																	;
;	Purpose		: Gets the status of SWx									;
;	Modifies	: B															;
;	Accepts		: Nothing													;
;	Return		: The status of SWx in B, 01 = down, 00 = up				;
;	Last mod	: 07/03/2007												;
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;						
getsw0:			psha
				ldaa	porta
				anda	#0x01
				beq		SWDOWN
				bra		swup
				
getsw1:			psha
				ldaa	porta
				anda	#0b00000010
				beq		SWDOWN
				bra		swup
				
getsw2:			psha
				ldaa	porta
				anda	#0b00000100
				beq		SWDOWN
				bra		swup
				
swdown:			ldab	#0x01
				pula
				rts
				
swup:			ldab	#0x00
				pula
				rts
				
				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

;.include 'porta.asm'
;.include 'delay.asm'
.include 'sci.asm'
.include 'LCD.asm'
.include 'spi.asm'
.include 'adc.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

TOC2_INC:		.dw		12288		;((1/100 Hz)/813.802 ns) = 12288.0

TimePrompt:		.asciz	"\e[2J\e[HEnter the time in HH:MM:SS format: "
SpeedPrompt:	.asciz	"\e[2J\e[HEnter the initial motor speed: "
CursorToTimePosition:
				.asciz	"\e[17;63H"
CursorToPumpPosition:
				.asciz	"\e[17;37H"
CursorToSpeedPosition:
				.asciz	"\e[13;27H"
				
StaticScreen:	.ascii	"\e[2J\e[?25l\e[02;33HMicro Design 2"
				.ascii	"\e[03;29HReal-time Programming"
				.ascii	"\e[04;30HBy Addison Babcock"
				.ascii	"\e[06;37HMenu"
				.ascii	"\e[07;05H----------------------------------------------------------------------"
				.ascii	"\e[08;06HS . . . . . . . . . . . . . . . . . . . . . . . . . . Stop the Motor"
				.ascii	"\e[09;06HR . . . . . . . . . . . . . . . . . . . . . . . . Repaint the Screen"
				.ascii	"\e[10;06HU . . . . . . . . . . . . . . . . . . . . . . . Increase Motor Speed"
				.ascii	"\e[11;06HD . . . . . . . . . . . . . . . . . . . . . . . Decrease Motor Speed"
				.ascii	"\e[12;06HZ . . . . . . . . . . . . . . . . . . . . . . . . . .Zero pump timer"
				.ascii	"\e[13;06HDesired Motor Speed>"
				.ascii	"\e[16;37HPump Timer"
				.ascii	"\e[16;59HRTL Time of Day"
				.asciz	"\e[17;63HHH:MM:SS"
				
CharacterData:
				.db	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
				.db 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
				.db 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18
				.db 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C
				.db 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E
				.db 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F

;
;							    Micro Design 2
;							Real-Time Programming
;							 By Addison Babcock
;
;									Menu
;	----------------------------------------------------------------------
;	 S . . . . . . . . . . . . . . . . . . . . . . . . . . Stop the Motor
;	 R . . . . . . . . . . . . . . . . . . . . . . . . Repaint the Screen
;    U . . . . . . . . . . . . . . . . . . . . . . . Increase Motor Speed
;	 D . . . . . . . . . . . . . . . . . . . . . . . Decrease Motor Speed
;
;	 Desired Motor Speed> __
;
;
;														  RTL Time of Day
;														      12:04:23    

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

TxPend:			.ds 1		;If non zero, there is a transmission in progress
							;and the number represents the task sending data
							
TxPointer:		.ds	2		;Pointer which holds the address of the next
							;byte to send.
							
TxBuffer:		.ds	50		;The SCI transmit buffer

Hours:			.ds	1		;Holds the BCD variable for hours (0x00-0x23)
Minutes:		.ds	1		;Holds the BCD variable for minutes (0-0x59)
Seconds:		.ds	1		;Holds the BCD variable for seconds (0-0x59)
Ticks:			.ds	1		;Holds the BCD variable for hundredths (0-0x99)
RefreshTime:	.ds	1		;A flag to indicate to the main program that 
							;a second has elapsed and the screen should be
							;refreshed.
Repaint:		.ds	1		;a flag to indicate that the screen needs to be
							;repainted
NewSpeed:		.ds	1		;a flag to indicate that the desired motor speed
							;has changed and needs to be redrawn
DesSpeed:		.ds	1		;the users desired motor speed
Vin:			.ds	1		;A copy of the sampled motor speed 00-ff
Vout:			.ds	1		;A copy of the value sent to the motor 00-ff
Diff:			.ds	1		;Difference between desired speed and actual speed

PumpTicks:		.ds	1
PumpSeconds:	.ds	1
PumpMinutes:	.ds	1
PumpHours:		.ds	1
PumpRefreshTime:.ds	1

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

;				.if TARGETROM == 1
;					.org	RESETVEC		 ;Place the reset vector so that
;					.DW		Main		     ;we can run this from power-up.
;					.org	0xffe6
;					.dw		OC2I_ISR
;				.endif
;
;				.if TARGETROM == 0
					.org	0x7fe6
					.dw		OC2I_ISR
;				.endif