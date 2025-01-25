using System;
using System.Buffers;

namespace DataFac.Memory
{
    /// <summary>
    /// A helper struct that supports efficient building of ReadOnlySequence\<T\>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ReadOnlySequenceBuilder<T>
    {
        private ReadOnlyMemorySegment<T>? _first { get; set; } = null;
        private ReadOnlyMemorySegment<T>? _last { get; set; } = null;

        public ReadOnlySequenceBuilder() { }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory) : this()
        {
            _first = new ReadOnlyMemorySegment<T>(memory);
        }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2) : this()
        {
            _first = new ReadOnlyMemorySegment<T>(memory1);
            _last = _first.Append(memory2);
        }

        public ReadOnlySequenceBuilder(ReadOnlyMemory<T> memory1, ReadOnlyMemory<T> memory2, ReadOnlyMemory<T> memory3) : this()
        {
            _first = new ReadOnlyMemorySegment<T>(memory1);
            _last = _first.Append(memory2).Append(memory3);
        }

        public ReadOnlySequenceBuilder(params ReadOnlyMemory<T>[] buffers) : this()
        {
            if (buffers.Length >= 1)
            {
                _first = new ReadOnlyMemorySegment<T>(buffers[0]);
            }
            if (buffers.Length >= 2)
            {
                _last = _first!.Append(buffers[1]);
            }
            for (int i = 2; i < buffers.Length; i++)
            {
                _last = _last!.Append(buffers[i]);
            }
        }

        public void Add(ReadOnlyMemory<T> memory)
        {
            if (_first is null)
            {
                _first = new ReadOnlyMemorySegment<T>(memory);
            }
            else if (_last is null)
            {
                _last = _first.Append(memory);
            }
            else
            {
                _last = _last.Append(memory);
            }
        }

        public ReadOnlySequence<T> Build()
        {
            if (_first is null) return ReadOnlySequence<T>.Empty;
            if (_last is null) return new ReadOnlySequence<T>(_first.Memory);
            return new ReadOnlySequence<T>(_first, 0, _last, _last.Memory.Length);
        }
    }
}
