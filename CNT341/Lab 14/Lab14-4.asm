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
				
				jsr		LCDInit
				clra				;A will be used to track the graph len
				clrb				;B will be used to track SW0
									;and for the blocking delay
				ldx		#Prompt		;display a prompt
				jsr		LCDGoDDData
				jsr		LCDDDString
				jsr		LoadCGData
				
MainLoop:
				ldab	#10			;delay 100 ms each loop
				jsr		dodelay
				jsr		getsw0		;if switch 0 is down, grow
				cmpb	#0x01
				beq		IncGraph
DecGraph:							;switch 0 is up, shrink
				cmpa	#0x00
				beq		GraphBuilt	;dont allow the graph to go below 0
				deca
				bra		GraphBuilt
IncGraph:							;switch 0 is down, grow
				cmpa	#80
				beq		GraphBuilt	;dont allow the graph to go above 80
				inca
				bra		GraphBuilt
GraphBuilt:		
				psha
				psha
				ldaa	#0x40		;show the graph on the second line
				clrb				;b will now track the current char
				jsr		LCDGoDDData
				pula
DisplayLoop:	cmpa	#5
				bge		ShowFive
				jsr		LCDSetChar	;Show a 0,1,2,3 or 4
				clra
				bra		DisplayLoopChk
ShowFive:							;Show a 5 and make the graph smaller
				psha
				ldaa	#5
				jsr		LCDSetChar
				pula
				suba	#5
DisplayLoopChk:	incb
				cmpb	#16
				beq		DoneDisplay
				bra		DisplayLoop

DoneDisplay:	pula
				bra		MainLoop
								
LoadCGData:
				pshx
				psha
				
				clra
				jsr		LCDGoCGData
								
				ldx		#Bar0
LoadCGDataLoop:	ldaa	0,x
				inx
				jsr		LCDSetChar
				cpx		#Bar5 + 8
				bne		LoadCGDataLoop
LoadCGDataDone:
				pula
				pulx
				rts				

				; ********************************************

;.------------------------------------------------------------------------.
;|                            INCLUDES                                    |
;`------------------------------------------------------------------------'

.include 'porta.asm'
.include 'delay.asm'
;.include 'sci.asm'
.include 'LCD.asm'

;.------------------------------------------------------------------------.
;|                            CONSTANT  DATA                              |
;`------------------------------------------------------------------------'

Bar0:			.db		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
Bar1:			.db		0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10
Bar2:			.db		0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18, 0x18
Bar3:			.db		0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C, 0x1C
Bar4:			.db		0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E, 0x1E
Bar5:			.db		0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F, 0x1F
Prompt:			.asciz	" Press Switch 0 "

;.------------------------------------------------------------------------.
;|                             VARIABLES                                  |
;`------------------------------------------------------------------------'

.if TARGETROM == 1
	.ORG    0
.endif

;.------------------------------------------------------------------------.
;|                           RESET VECTOR                                 |
;`------------------------------------------------------------------------'

				.AREA	RESETVEC (ABS)

				.if TARGETROM == 1
					.org	RESETVEC		 ;Place the reset vector so that
					.DW		Main		     ;we can run this from power-up.
				.endif
				