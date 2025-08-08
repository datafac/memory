# Datafac.Memory
Performant memory related types and codecs. Targets .NetStandard 2.0 and .NET 8.0+.

[![Build-Deploy](https://github.com/datafac/memory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/datafac/memory/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Datafac.Memory)
![NuGet Downloads](https://img.shields.io/nuget/dt/Datafac.Memory)
![GitHub License](https://img.shields.io/github/license/Datafac/Memory)

## Codecs
Endian-aware binary encoders and decoders for primitive types.

## Blocks
Structs that divide memory into a binary tree. Sizes from 1B to 8KB.

## Octets
An immutable reference type that wraps a ReadOnlySequence\<byte\>.

## ReadOnlySequenceBuilder
A helper struct that supports efficient building of ReadOnlySequence\<T\>.

## ReadOnlyMemorySegment
An implementation of ReadOnlySequenceSegment\<T\>.
