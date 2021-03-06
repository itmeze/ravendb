﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Raven.Client.Util
{
	public class AsyncCountdownEvent
	{
		private readonly AsyncManualResetEvent resetEvent = new AsyncManualResetEvent();
		private volatile int count;

		public AsyncCountdownEvent(int initialCount)
		{
			if (initialCount <= 0) 
				throw new ArgumentOutOfRangeException("initialCount");
			count = initialCount;
		}

		public bool Active
		{
			get { return count > 0; }
		}

		public int Count
		{
			get { return count; }
		}

		public Task WaitAsync() { return resetEvent.WaitAsync(); }

		public bool Signal()
		{
			if (count <= 0)
				return false;

#pragma warning disable 420
			int newCount = Interlocked.Decrement(ref count);
#pragma warning restore 420
			if (newCount == 0)
				resetEvent.Set();
			if (newCount < 0)
				return false;

			return true;
		}
	}
}