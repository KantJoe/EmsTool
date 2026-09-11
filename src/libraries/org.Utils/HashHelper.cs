using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.Utils
{
    public static class HashHelper
    {
        private static UInt32[] CRC32Table;

        public static int CRC16(byte[] data, int len)
        {
            int xda;
            int xdapoly;
            int i, j, xdabit;
            xda = 0xFFFF;
            xdapoly = 0xA001;
            for (i = 0; i < len; i++)
            {
                xda ^= data[i];
                for (j = 0; j < 8; j++)
                {
                    xdabit = (int)(xda & 0x01);
                    xda >>= 1;
                    if (xdabit == 1)
                        xda ^= xdapoly;
                }
            }

            return xda;
        }

        public static uint GetCRC32Code(byte[] buff)
        {
            InitializeCRC32Table();

            uint iCount = (uint)buff.Length;
            uint crc = 0xFFFFFFFF;
            for (uint i = 0; i < iCount; i++)
            {
                crc = (crc >> 8) ^ CRC32Table[(crc ^ buff[i]) & 0xFF];
            }

            return (crc ^ 0xFFFFFFFF);
        }

        public static void InitializeCRC32Table()
        {
            if (CRC32Table?.Any() == true)
            {
                return;
            }

            uint Crc;
            int i, j;
            CRC32Table = new uint[256];
            for (i = 0; i < 256; i++)
            {
                Crc = (uint)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                    {
                        Crc = (Crc >> 1) ^ 0xEDB88320;  // 0x04C11DB7 位逆转得到 0xEDB88320.
                    }
                    else
                    {
                        Crc >>= 1;
                    }
                }

                CRC32Table[i] = Crc;
            }
        }
    }

}
