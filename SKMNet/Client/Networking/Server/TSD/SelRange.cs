﻿using SKMNET.Client.Stromkreise;
using SKMNET.Client.Stromkreise.ML;
using System;

namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// Range-Daten des selektierten Parameters
    /// </summary>
    [Serializable]
    public class SelRange : SPacket
    {
        public ushort Fixture;
        public ushort Fixpar;
        public ushort Val16;
        public ushort Res1;
        public ushort Res2;
        public bool Last;
        public ushort Count;

        public SelRangeData[] Arr;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Fixture = buffer.ReadUShort();
            Fixpar = buffer.ReadUShort();
            Val16 = buffer.ReadUShort();
            Res1 = buffer.ReadUShort();
            Res2 = buffer.ReadUShort();
            Last = buffer.ReadUShort() != 0;
            Count = buffer.ReadUShort();
            Arr = new SelRangeData[Count];
            for(int i = 0; i < Count; i++)
            {
                Arr[i] = new SelRangeData(
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadByte(),
                    buffer.ReadUShort(),
                    buffer.ReadUShort(),
                    buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            SK sk = console.Stromkreise[Fixture];
            MlParameter param = sk?.Parameters.Find(x => x.ParNo == Fixpar);
            if (param is null)
                return Enums.Response.BadCmd;

            MlcParameter mlc = console.MLCParameters.Find(x => x.Number == Fixpar);
            //TODO remove?
#if DEBUG
            console.Logger?.Log(Count);
#endif
            
            // TODO inspection
            int i = 0;
            foreach(SelRangeData data in Arr)
            {
                param.Range = (Start: data.Start, End: data.End);
                param.DefaultVal = data.DefaultVal;
                param.Flags = data.Flags;
                param.Name = data.Name;

                mlc.Range = (start: data.Start, end: data.End);
                mlc.Default = data.DefaultVal;
                mlc.Flags = data.Flags;
                mlc.Name = data.Name;

                //increment par-pointer?
                i++;
                param = sk.Parameters.Find(x => x.ParNo == Fixpar + i);
            }
            return Enums.Response.OK;
        }

        [Serializable]
        public struct SelRangeData
        {
            public readonly byte Start;
            public readonly byte End;
            public readonly byte DefaultVal;
            public readonly byte Flags;
            public readonly ushort Res1;
            public readonly ushort Res2;
            public readonly string Name;

            public SelRangeData(byte start, byte end, byte defaultVal, byte flags, ushort res1, ushort res2, string name)
            {
                Start = start;
                End = end;
                DefaultVal = defaultVal;
                Flags = flags;
                Res1 = res1;
                Res2 = res2;
                Name = name;
            }
        }
    }
}
