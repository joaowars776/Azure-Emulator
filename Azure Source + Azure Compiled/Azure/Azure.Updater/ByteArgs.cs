using System;

namespace Azure.Updater
{
    public class ByteArgs : EventArgs
    {
        public int downloaded { get; set; }

        public int total { get; set; }
    }
}