// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/4/Fill.asm

// Runs an infinite loop that listens to the keyboard input. 
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel. When no key is pressed, 
// the screen should be cleared.

(READ)
//i=SCREEN
@SCREEN
D=A
@i
M=D
//Read keyboard
@KBD
D=M
@WHITEN
D;JEQ
@BLACKEN
D;JGT
  
(WHITEN)
  //if(i==SCREEN+8k-1) read again
  @SCREEN
  D=A
  @8192
  D=D+A
  @i
  D=M-D
  @READ
  D;JEQ
  //RAM[i]=-1
  @i
  A=M
  M=0
  //i++
  @i
  M=M+1
  //continue
  @WHITEN
  0;JMP

(BLACKEN)
  //if(i==SCREEN+8k) read again
  @SCREEN
  D=A
  @8192
  D=D+A
  @i
  D=M-D
  @READ
  D;JEQ
  //RAM[i]=0
  @i
  A=M
  M=-1
  //i++
  @i
  M=M+1
  //continue
  @BLACKEN
  0;JMP
    

      

    
  

