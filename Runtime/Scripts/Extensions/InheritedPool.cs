// ----------------------------------------------------------------------------
// -- Project : https://github.com/instance-id/Busy_Richard_URP         --
// -- instance.id 2021 | http://github.com/instance-id | http://instance.id  --
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace instance.id.Extensions
{

	public class InheritedPool<T> where T : class
	{
		private readonly int maxItemsOfSameType;

		private readonly Dictionary<Type, Stack<T>> pool;

		public int Count
		{
			get { return pool.Count; }
		}

		public bool Remove(T remove)
		{
			Stack<T> stack;
			var type = typeof(T);
			if (pool.TryGetValue(type, out stack))
			{
				var list = stack.ToList();
				if (list.Remove(remove))
				{
					pool[type] = new Stack<T>(list);
					return true;
				}
			}

			return false;
		}

		public InheritedPool(int capacity, int setMaxItemsOfSameType)
		{
			pool = new Dictionary<Type, Stack<T>>(capacity);
			maxItemsOfSameType = setMaxItemsOfSameType;
		}

		public bool TryGet<TResult>(out TResult result) where TResult : class, T
		{
			Stack<T> stack;
			if (pool.TryGetValue(typeof(TResult), out stack))
			{
				if (stack.Count > 0)
				{
					result = stack.Pop() as TResult;
					return true;
				}
			}

			result = null;
			return false;
		}

		public bool TryGet(Type type, out object result)
		{
			Stack<T> stack;
			if (pool.TryGetValue(type, out stack))
			{
				if (stack.Count > 0)
				{
					result = stack.Pop();
					return true;
				}
			}

			result = null;
			return false;
		}

		public bool Contains(T item)
		{
			Stack<T> stack;
			return pool.TryGetValue(typeof(T), out stack) && stack.Contains(item);
		}

		public void Pool(ref T disposing)
		{
#if !DEV_MODE || !TEST_DISABLE_POOLING
			var type = disposing.GetType();

			Stack<T> stack;
			if (!pool.TryGetValue(type, out stack))
			{
				stack = new Stack<T>();
				pool[type] = stack;
			}

#if DEV_MODE || SAFE_MODE
			if(stack.Contains(disposing))
			{
				UnityEngine.Debug.LogError("PolymorphicPool.Dispose was called for item " + StringUtils.ToString(disposing) + " of type "+ StringUtils.ToStringSansNamespace(typeof(T)) + " but pool already contained the same item!");
				disposing = null;
				return;
			}
#endif

			int count = stack.Count;
			if (count >= maxItemsOfSameType)
			{
#if DEV_MODE
				UnityEngine.Debug.LogWarning("PolymorphicPool.Dispose was called for item " + StringUtils.ToString(disposing) + " of type " + StringUtils.ToStringSansNamespace(typeof(T)) + " but pool already contained "+ count + " instances of the same item!");
#endif
				return;
			}

			stack.Push(disposing);

#if DEV_MODE && DEBUG_POOLED_COUNT
			UnityEngine.Debug.Log("PolymorphicPool.Dispose was called for item " + StringUtils.ToString(disposing) + " of type " + StringUtils.ToString(typeof(T)) + ". Pool now contains "+ stack.Count + " instances of said type.");
#endif
#endif

			disposing = null;
		}

		public void Clear()
		{
			pool.Clear();
		}
	}
}
