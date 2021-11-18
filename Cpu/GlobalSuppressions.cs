// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Instructions.Exceptions.UnknownOpcodeException")]
[assembly: SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Not to be used in any way", Scope = "type", Target = "~T:Cpu.Execution.Exceptions.ProgramExecutionExeption")]
