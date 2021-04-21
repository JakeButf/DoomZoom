using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindWakerHD_Rewrite
{
    public class GameInput
    {
        public uint InputAddress = 0x102F48A8;

        public bool IsDpadDownDown(TCPGecko Gecko)
        {
            return Gecko.peek(InputAddress).ToString() == "256";
        }

        public bool IsDpadLeftDown(TCPGecko Gecko)
        {
            return Gecko.peek(InputAddress).ToString() == "2048";
        }

        public bool IsDpadRightDown(TCPGecko Gecko)
        {
            return Gecko.peek(InputAddress).ToString() == "1024";
        }
    }
}
