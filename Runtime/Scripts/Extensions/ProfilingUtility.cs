// ----------------------------------------------------------------------------
// -- Project : https://github.com/instance-id/LocalPackages                 --
// -- instance.id 2020 | http://github.com/instance-id | http://instance.id  --
// https://gist.github.com/wotakuro/a42641e99707d4b1b624f61f75c6e1f4 -----------

using System.Collections;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;

#else
using UnityEngine;
#endif

namespace instance.id.Extensions
{
    /** Make inserting Profiler call to your code more easily.
 *
 * // use like this
 * void Test(){
 *   using( ProfilingUtil.Sample("TestFunction") ){
 *     // execute Something...
 *   }
 * }
 *
 */
    public class ProfilingUtility : System.IDisposable
    {
        public static ProfilingUtility Sample(string name)
        {
            return new ProfilingUtility(name);
        }

        private ProfilingUtility(string name)
        {
            Profiler.BeginSample(name);
        }

        public void Dispose()
        {
            Profiler.EndSample();
        }

        public static bool SampleMoveNext(string name, IEnumerator e)
        {
            using (var tmp = new ProfilingUtility(name + e.ToString()))
            {
                return e.MoveNext();
            }
        }
    }
}
