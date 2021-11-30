; Keeps incrementing X
; And stores X in memory
; Until overflows.
; At interrupt, reads memory into Y
; Decrements from Y
; And stores Y in different memory.
.ORG	$0000
        LDX #$00
INCX	INX
        STX $0400
        BNE INCX
		BRK
STI		LDY $0400
		INY
		STY $0401
        RTI
		
; Interrupt masks
.ORG	$FFFA
.WORD	STI			; NMI vector to STI.
		
.END