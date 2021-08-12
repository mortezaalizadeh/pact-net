using System;
using System.Runtime.InteropServices;

namespace PactNet.Native.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct InteractionHandle
    {
        public readonly UIntPtr Pact;
        public readonly UIntPtr Interaction;
    }
}