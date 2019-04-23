using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
  public class Debug
  {
    internal static ILogger s_Logger = null;

    public static ILogger unityLogger => s_Logger;

    public static bool isDebugBuild { get; }
  }
}
