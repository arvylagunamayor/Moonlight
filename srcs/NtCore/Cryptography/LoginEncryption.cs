﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NtCore.Cryptography
{
    public class LoginEncryption : ICryptography
    {
        public byte[] Encrypt(string packet, bool session = false)
        {
            var output = new byte[packet.Length + 1];
            for (int i = 0; i < packet.Length; i++)
            {
                output[i] = (byte)((packet[i] ^ 0xC3) + 0xF);
            }

            output[output.Length - 1] = 0xD8;
            return output;
        }

        public IEnumerable<string> Decrypt(byte[] packet, int size)
        {
            var output = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                output.Append(Convert.ToChar(packet[i] - 0xF));
            }

            return new List<string> { output.ToString() };
        }
    }
}