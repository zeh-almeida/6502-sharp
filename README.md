# 6502 Sharp
*6502Sharp* is an [6502 Emulator](https://en.wikipedia.org/wiki/MOS_Technology_6502) written purely on [.NET](https://docs.microsoft.com/en-us/dotnet/core/introduction).

## Yet another 6502 emulator?
While there are a grat amount of existing, supported and very well-maintained 6502 emulators, most of them are written in `C/C++`.

Not only that but most of them are very "machine friendly" but not really "human friendly", this means that code is very optimized to run as fast and as precise as possible.

The objective of this project is to present a alternative: while aiming at the original hardware, the goal is to provide an implementation which is as "human-friendly" as possible.

At the same time, this is a learning project: most people with high-level background tremble and shake when mentioning assembly, binary and any other low-level term, let's change that.

There are many techniques used to reach the objective:
* Object Oriented design
* Code documentation
* Unit tests
* Integration tests

### Code documentation
Every `public` element of the project must be accompanied by a `docstring`. This allows for quick references and explains behaviors in a better way.

This can also be used to generate other documentation resources, which are always useful for users of this project.

### Unit tests
They aim is to have 100% unit test coverage. While full coverage does not guarantee the code is also 100% up to spec, it should cover pretty much all cases, specially the corner-cases.

### Integration tests
While unit tests aim to validate especific behaviors, the integration tests are supposed to guarantee a regular program execution.

This means the tests should simulate a regular 6502 program execution, making sure the emulator behaves as it should in more complex environments.

## Design philosophy
All CPUs are based on the [Turing Machine](https://en.wikipedia.org/wiki/Turing_machine) concept and can also be expressed by the [Finite State Machine](https://en.wikipedia.org/wiki/Finite-state_machine) concept as well.

The 6502 CPU is then implemented as a state where all memory, stack, registers and flags are contained and can be manipulated by a instruction.

### CPU State
The CPU state allows read/write operations on the memory, stack, register and flags

#### CPU Memory
The 6502 is a 8-bit machine but has a 16-bit address bus, allowing for a max of 65535 bytes of data.

The manipulation is performed by the *Memory Manager* and every instruction execution has access to it via the state.

#### CPU Stack
The 6502 has an 8-bit stack which is just an abstraction to memory manipulation and pointer maintenance.

The manipulation is performed by the *Stack Manager* and every instruction execution has access to it via the state.

Normally, the stack would allow only 8-bit values but because of some specific operations, this stack implementation also allows for 16-bit values to be manipulated.

#### CPU Registers
The 6502 has 5 registers:
* `Program Counter`
* `Stack Pointer`
* `Accumulator`
* `X Register` or `Index X`
* `Y Register` or `Index Y`

All registers are 8-bit values, except for the program counter which is 16-bit, because it must point to a memory location.

The manipulation is performed by the *Register Manager* and every instruction execution has access to it via the state.

#### CPU Flags
The 6502 has 7 flags:
* `Carry` flag
* `Zero` flag
* `Interrupt Disabled` flag
* `Decimal Mode` flag
* `Break Command` flag

All flags are binary in nature, either on or off.

The manipulation is performed by the *Flag Manager* and every instruction execution has access to it via the state.

### CPU Instructions
Based on their nature, it was decided to represent instructions as "small programs".

However, some instructions respond to many different opcodes, in which they manipulate values differently.

Most of the time the difference in execution is based on where to read from or write to, based on inputs, flags, registers  or memory values

Therefore, each instruction is modeled in a separate class, which encapsulates all different behaviors of the specific instruction respecting their specific opcode.

All instruction implementations will honor the `Decimal Mode` flag, if set.

#### Opcodes
Represent a variation of a specific instruction. In this implementation they are encapsulated with additional information such as clock cycles and length.

This is necessary because even though they represent the same instruction, the source and destination of data will change and therefore may take more machine cycles.

At the same time the amount of data needed to execute the instruction changes as well. Some instructions may require 0, 8 or 16-bit values to be executed, differing only by opcode.

#### Illegal Opcodes
There are many opcodes which are not part of the original 6502 documentation. Those opcodes are called "illegal".

Since the objective of this project is to simulate the hardware, all those instructions are implemented. There are some, however, which are branded "unstable".

This means that in hardware they may behave in unexpected ways. Sometimes they may depend on a "magic constant" or their behavior may be altered because of external factors such as temperature.

In those cases, the project's implementation will note the behavior change needed to execute the instruction. This may result in an implementation which will differ from the hardware but bacause of those opcode natures, compromises must be made.

### Cycle execution
In order to execute an instruction, the CPU must first fetch the opcode, read the additional data, if needed and then execute the instruction.

While doing so, the CPU must also be aware that instructions may take more than one cycle to execute and therefor must track those cycles in order to be more accurate.

While some instruction opcodes have varying cycles, those types of cycles are not currently supported. In the current implementation the highest cycle is used even when a lower value would suffice.

## Acknowledgements
This project was only made possible because of many different people and source materials:
* https://csdb.dk/release/index.php?id=173253
* https://masswerk.at/6502/6502_instruction_set.html
* https://www.esocop.org , search for `MOS 6510 Unintended Opcodes` as they asked not to link directly
* https://wiki.nesdev.org/w/index.php/INES
* http://fms.komkon.org/EMUL8/NES.html
* https://dopedesi.com/2021/06/06/6502-illegal-opcodes-demystified/
* http://skilldrick.github.io/easy6502/
* http://www.6502.org/
* http://people.ece.cornell.edu/land/courses/ece4760/FinalProjects/s2007/bcr22/final%20webpage/final.html

A honorable mention to http://www.obelisk.me.uk/6502/ which went offline during the making of this project. Thank you very much.

And many more, which will be added as the project gets updated.

## Maintenance
This is a pet project. While there are plans to expand it and make it closer to hardware, this not a production-grade project.

Pull requests and issues are welcome, especially those related to more tests and better implementation.

## How to run
For now, the only way to run the emulator is through the integration tests.

The emulator is contained in a library project, meaning it can be loaded in other programs but cannot run by itself.

Better ways of execution will be added, such as:
* dependency injection mechanisms
* console execution
* Additional hardware such [NES](https://en.wikipedia.org/wiki/Nintendo_Entertainment_System), for example