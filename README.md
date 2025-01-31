# Datafac.Memory
Performant memory related types and codecs. Targets .NetStandard 2.0 and .NET 8+.

[![Build-Deploy](https://github.com/datafac/memory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/datafac/memory/actions/workflows/dotnet.yml)

## Codecs
Endian-aware binary encoders and decoders for primitive types.

## Blocks
Structs that divide memory into a binary tree. Sizes from 1B to 8KB.

## Octets
An immutable reference type that wraps a ReadOnlyMemory\<byte\> buffer.

## ReadOnlySequenceBuilder
A helper struct that supports efficient building of ReadOnlySequence\<T\>.

## ReadOnlyMemorySegment
An implementation of ReadOnlySequenceSegment\<T\>.
