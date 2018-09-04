using System;
using System.Collections.Generic;

namespace Pegasus
{
    public class SequenceQueue<T> where T : IConvertible
    {
        public T Sequence { get; private set; }

        private readonly Queue<T> queue = new Queue<T>();

        public SequenceQueue(T sequence)
        {
            Sequence = sequence;
        }

        public T Dequeue()
        {
            if (queue.Count > 0)
                return queue.Dequeue();

            Sequence = (T)(dynamic)(Convert.ToDouble(Sequence) + 1d);
            return Sequence;
        }

        public void Enqueue(T value)
        {
            queue.Enqueue(value);
        }
    }
}
