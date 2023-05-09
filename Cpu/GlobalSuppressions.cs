// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Execution.Exceptions.ProgramExecutionException")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Execution.Exceptions.ProgramExecutionException")]

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Instructions.Exceptions.UnknownOpcodeException")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Instructions.Exceptions.UnknownOpcodeException")]

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Instructions.Exceptions.UnknownInstructionException")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Instructions.Exceptions.UnknownInstructionException")]

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Opcodes.Exceptions.DuplicateOpcodeException")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Opcodes.Exceptions.DuplicateOpcodeException")]

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Opcodes.Exceptions.MisconfiguredOpcodeException")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Opcodes.Exceptions.MisconfiguredOpcodeException")]

[assembly: SuppressMessage("Style", "IDE0230:Use UTF-8 string literal", Justification = "Instruction OP Codes must be legible", Scope = "namespaceanddescendants", Target = "~N:Cpu")]
