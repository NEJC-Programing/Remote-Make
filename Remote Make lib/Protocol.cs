using System;
using System.Collections.Generic;
using System.Text;

namespace Remote_Make
{
    class Protocol
    {
        private static int ProtocolPort = 6060;
        public void Init()
        {

        }
        public void Init(int port)
        {
            ProtocolPort = port;
        }
    }
}
