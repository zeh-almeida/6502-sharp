; Keeps incrementing X
; And stores X in memory
; Until overflows.
; At interrupt, reads memory into Y
; Decrements from Y
; And stores Y in different memory.
; At the end, resets the interrupt address to stop the program.
.ORG	$0000
		CLI
        LDX #$00
		
INCX	INX
        STX $0400
        BNE INCX
		
		LDA #$FF	; Stops the program
		STA $FFFE
		STA $FFFF
		
		BRK
		
STI		SEI
		LDY $0400
		INY
		STY $0401
		CLI
        RTI
		
; Interrupt masks
.ORG	$FFFE
.WORD	STI			; NMI vector to STI.
		
.END