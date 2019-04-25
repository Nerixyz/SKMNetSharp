﻿using SKMNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client.Networking.Client
{
    /// <summary>
    /// SKMON an/abmelden
    /// </summary>
    public class SKMSync : CPacket
    {
        public override short Type => 13;

        private readonly Action action;
        private readonly int flags;
        private readonly byte SKMType;

        public override byte[] GetDataToSend(LightingConsole console)
        {
            byte[] steckbrief = new byte[10];
            for(int i = 0; i < 10; i++)
            {
                steckbrief[i] = (flags & (1 << i)) != 0 ? (byte)1 : (byte)0;
            }
            steckbrief[0] = SKMType;
            if (action == Action.BEGIN)
                return new ByteBuffer().Write((short)action).WriteShort(console.BdstNo).WriteShort(10).Write(steckbrief).ToArray();
            else
                return new ByteBuffer().Write((short)action).WriteShort(console.BdstNo).WriteShort(0)/*count = 0 -> no array needed { .Write(steckbrief) } */.ToArray();
        }

        public SKMSync(Action action)
        {
            this.action = action;
            this.flags = 0;
        }

        public SKMSync(int flags)
        {
            this.action = Action.BEGIN;
            this.flags = flags;
        }

        public SKMSync(SKMSteckbrief steckbrief, byte SKMType)
        {
            this.action = Action.BEGIN;
            System.Reflection.FieldInfo[] fields = steckbrief.GetType().GetFields();
            int flags = 0;
            for(int i = 0; i < fields.Length; i++)
            {
                flags |= ((bool)fields[i].GetValue(steckbrief) ? 1 : 0) << (i+1);
            }
            this.flags = flags;
            this.SKMType = SKMType;
        }

        public enum Action
        {
            BEGIN = 1,
            END,
            PING
        }
    }

    // nicht alles wird verwendet?!
    // auch wenn Bed = false werden Tastendaten gesendet
    public struct SKMSteckbrief
    {
        public bool Bedientasten;
        public bool BefMeldZeile;
        public bool FuncKeys;
        public bool LKI;
        public bool BlockInfo;
        public bool AZ_Zeilen;
        public bool ExtKeys;
        public bool AktInfo;
        public bool Steller;
    }
}
