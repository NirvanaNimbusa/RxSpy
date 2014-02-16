﻿using System;
using System.Threading;
using freakcode.frequency;

namespace RxSpy.Events
{
    internal abstract class Event : IEvent
    {
        static long counter = 0;

        readonly EventType _type;
        readonly long _id;
        readonly long _ts;

        public EventType Type { get { return _type; } }
        public long EventId { get { return _id; } }
        public long EventTime { get { return _ts; } }

        Event(long id, long ts)
        {
            _id = id;
            _ts = ts;
        }

        protected Event(EventType type)
            : this(Interlocked.Increment(ref counter), Monotonic.Time())
        {
            _type = type;
        }

        public static OperatorCreatedEvent OperatorCreated(OperatorInfo operatorInfo)
        {
            return new OperatorCreatedEvent(operatorInfo);
        }

        public static Event OnNext(OperatorInfo operatorInfo, Type type, object value)
        {
            return new OnNextEvent(operatorInfo, type, value);
        }

        public static Event OnError(OperatorInfo operatorInfo, Exception error)
        {
            return new OnErrorEvent(operatorInfo, error);
        }

        public static Event OnCompleted(OperatorInfo operatorInfo)
        {
            return new OnCompletedEvent(operatorInfo);
        }

        internal static Event Subscribe(OperatorInfo child, OperatorInfo parent)
        {
            return new SubscribeEvent(child, parent);
        }

        internal static Event Unsubscribe(long subscriptionId)
        {
            return new UnsubscribeEvent(subscriptionId);
        }
    }
}