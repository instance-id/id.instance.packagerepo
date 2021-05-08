// ----------------------------------------------------------------------------
// -- Project : https://github.com/instance-id/Busy_Richard_URP         --
// -- instance.id 2021 | http://github.com/instance-id | http://instance.id  --
// ----------------------------------------------------------------------------

using System;
using Unity.Collections;

namespace instance.id.Extensions
{
   public static class NativeMultiHashMapExtensions
   {
      public static NativeArray<TValue>? CopyValuesForKey<TKey, TValue>(this ref NativeMultiHashMap<TKey, TValue> map, TKey key)
         where TKey : struct, IEquatable<TKey>
         where TValue : struct {
         if (!map.ContainsKey(key)) return null;

         var count = map.CountValuesForKey(key);
         var iterator = map.GetValuesForKey(key);
         var values = new NativeArray<TValue>(count, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
         for (var i = 0; i < count && iterator.MoveNext(); i++) {
            values[i] = iterator.Current;
         }

         return values;
      }

   public static bool Remove<K, T>(this NativeMultiHashMap<K, T> HashMap, K Key, T Value) where T : struct, IEquatable<T> where K : struct, IEquatable<K> {
      if (HashMap.SelectIterator(Key, Value, out var It)) {
         HashMap.Remove(It);

         return true;
      }
      return false;
   }

   public static bool SelectIterator<K, T>(this NativeMultiHashMap<K, T> HashMap, Predicate<T> Operate, K Key, out NativeMultiHashMapIterator<K> Iterator) where T : struct where K : struct, IEquatable<K> {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         if (Operate(Value)) {
            Iterator = It;

            return true;
         }

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
      Iterator = new NativeMultiHashMapIterator<K>();

      return false;
   }

   public static bool SelectIterator<K, T>(this NativeMultiHashMap<K, T> HashMap, K Key, T Value, out NativeMultiHashMapIterator<K> Iterator) where T : struct, IEquatable<T> where K : struct, IEquatable<K> {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var ItValue, out var It); Success;) {
         if (Value.Equals(ItValue)) {
            Iterator = It;

            return true;
         }

         Success = HashMap.TryGetNextValue(out ItValue, ref It);
      }
      Iterator = new NativeMultiHashMapIterator<K>();

      return false;
   }

   public static void ForEeach<K, T>(this NativeMultiHashMap<K, T> HashMap, Predicate<T> Operate, K Key) where K : struct, IEquatable<K> where T : struct {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         if (!Operate(Value)) {
            break;
         }

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
   }

   public static void ForEeach<K, T, A0>(this NativeMultiHashMap<K, T> HashMap, Func<T, A0, bool> Operate, K Key, A0 Arg0) where K : struct, IEquatable<K> where T : struct {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         if (!Operate(Value, Arg0)) {
            break;
         }

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
   }

   public static void ForEeach<K, T>(this NativeMultiHashMap<K, T> HashMap, Action<T> Operate, K Key) where K : struct, IEquatable<K> where T : struct {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         Operate(Value);

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
   }

   public static void ForEeach<K, T, A0>(this NativeMultiHashMap<K, T> HashMap, Action<T, A0> Operate, K Key, A0 Arg0) where K : struct, IEquatable<K> where T : struct {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         Operate(Value, Arg0);

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
   }

   public static void ForEeach<K, T, A0, A1>(this NativeMultiHashMap<K, T> HashMap, Action<T, A0, A1> Operate, K Key, A0 Arg0, A1 Arg1) where K : struct, IEquatable<K> where T : struct {
      for (bool Success = HashMap.TryGetFirstValue(Key, out var Value, out var It); Success;) {
         Operate(Value, Arg0, Arg1);

         Success = HashMap.TryGetNextValue(out Value, ref It);
      }
   }
}
}
