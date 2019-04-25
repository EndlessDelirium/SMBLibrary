using System;
using System.Threading;

namespace Utilities
{
    public class CountdownLatch
    {
        private int m_count;
        private EventWaitHandle m_waitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);

        public CountdownLatch()
        {
        }

        public void Increment()
        {
#if WindowsCE
            int count = Interlocked2.Increment(ref m_count);
#else
            int count = Interlocked.Increment(ref m_count);
#endif
            if (count == 1)
            {
                m_waitHandle.Reset();
            }
        }

        public void Add(int value)
        {
#if WindowsCE
            int count = Interlocked2.Add(ref m_count, value);
#else
            int count = Interlocked.Add(ref m_count, value);
#endif
            if (count == value)
            {
                m_waitHandle.Reset();
            }
        }

        public void Decrement()
        {
#if WindowsCE
            int count = Interlocked2.Decrement(ref m_count);
#else
            int count = Interlocked.Decrement(ref m_count);
#endif
            if (m_count == 0)
            {
                m_waitHandle.Set();
            }
            else if (count < 0)
            {
                throw new InvalidOperationException("Count must be greater than or equal to 0");
            }
        }

        public void WaitUntilZero()
        {
            m_waitHandle.WaitOne();
        }
    }
}
