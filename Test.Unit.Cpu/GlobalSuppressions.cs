// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming conventions", Scope = "namespaceanddescendants", Target = "~N:Test.Unit.Cpu")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Should avoid because of parallelism limits", Scope = "namespaceanddescendants", Target = "~N:Test.Unit.Cpu")]
[assembly: SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "Inline data for testing", Scope = "member", Target = "~M:Test.Unit.Cpu.Extensions.BitArrayExtensionsTest.AsEightBit_Input_Returns(System.Int32[],System.Byte)")]
[assembly: SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "Inline data for testing", Scope = "member", Target = "~M:Test.Unit.Cpu.Extensions.BitArrayExtensionsTest.AsSixteenit_Input_Returns(System.Int32[],System.UInt16)")]
