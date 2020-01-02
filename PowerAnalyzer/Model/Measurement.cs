using System;
using System.Collections.Generic;
using System.Text;

namespace PowerAnalyzer.Model
{
    class Measurement : IComparable<Measurement>, IComparable
    {
        public DateTime Timestamp { get; private set; }
        public double Value { get; private set; }
        public bool IsDay => 7 <= Timestamp.Hour && Timestamp.Hour < 19;

        public Measurement(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public int CompareTo(Measurement other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Timestamp.CompareTo(other.Timestamp);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is Measurement other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Measurement)}");
        }
    }
}
