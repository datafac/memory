# Datafac.Memory
Performant memory related types and codecs. Targets .NetStandard 2.0 and .NET 8 (LTS), 9, and 10 (LTS).

[![Build-Deploy](https://github.com/datafac/memory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/datafac/memory/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Datafac.Memory)
![GitHub License](https://img.shields.io/github/license/Datafac/Memory)
![NuGet Downloads](https://img.shields.io/nuget/dt/Datafac.Memory)
![GitHub Sponsors](https://img.shields.io/github/sponsors/psiman62)

*Breaking changes in v2.x: This major version release includes breaking changes over v1.x.*
- Codec methods have been simplified to only take a Span\<byte\> as the destination buffer. 
  This change was made to improve performance and reduce complexity.
- IFieldCodec and ITypedFieldCodec interfaces have been removed. Instead, codecs are now 
  implemented as static methods in the codec classes.

## Codecs
Endian-aware binary encoders and decoders for primitive types.

## Blocks
Structs that divide memory into a binary tree. Sizes from 1B to 8KB.

## Octets
An immutable reference type that wraps an ImmutableArray\<ReadOnlyMemory\<byte\>\>

## ReadOnlySequenceBuilder
A helper struct that supports efficient building of ReadOnlySequence\<T\>.

## ReadOnlyMemorySegment
An implementation of ReadOnlySequenceSegment\<T\>.

## ByteBufferWriter
An implementation of IBufferWriter\<byte\> that does not internally reallocate as the buffer grows.
Instead, it maintains a list of buffers and allocates new ones as needed.

## Bits32
Represents a 32-bit unsigned integer that provides methods for querying and manipulating individual 
bits in an immutable manner.

## How to sponsor
If you find this package useful, please consider sponsoring my work on GitHub 
at https://github.com/sponsors/Psiman62
or buy me a coffee at https://www.buymeacoffee.com/psiman62

## License
This project is licensed under the Apache-2.0 License - see the [LICENSE](LICENSE) file for details.
