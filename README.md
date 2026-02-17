# Datafac.Memory
Performant memory related types and codecs. Targets .NetStandard 2.0 and .NET 8 (LTS), 9, and 10 (LTS).

[![Build-Deploy](https://github.com/datafac/memory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/datafac/memory/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Datafac.Memory)
![GitHub License](https://img.shields.io/github/license/Datafac/Memory)
![NuGet Downloads](https://img.shields.io/nuget/dt/Datafac.Memory)
![GitHub Sponsors](https://img.shields.io/github/sponsors/psiman62)

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

## ByteBufferWriter
An implementation of IBufferWriter\<byte\> that does not internally reallocate as the buffer grows.
Instead, it maintains a list of buffers and allocates new ones as needed.

## How to sponsor
If you find this package useful, please consider sponsoring my work on GitHub 
at https://github.com/sponsors/Psiman62
or buy me a coffee at https://www.buymeacoffee.com/psiman62

## License
This project is licensed under the Apache-2.0 License - see the [LICENSE](LICENSE) file for details.
