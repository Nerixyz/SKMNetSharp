namespace SKMNET.Client.Networking.Server.TSD
{
    /// <summary>
    /// Parameterdefinitionen (Name usw.)
    /// </summary>
    public class ParDef : SPacket
    {

        public bool Last;
        public ushort Count;
        public ParDefData[] Data;

        public override SPacket ParsePacket(ByteBuffer buffer)
        {
            Last = buffer.ReadUShort() != 0;
            Count = buffer.ReadUShort();
            Data = new ParDefData[Count];
            for(int i = 0; i < Count; i++)
            {
                Data[i] = new ParDefData(
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadShort(),
                    buffer.ReadString(8));
            }
            return this;
        }

        public override Enums.Response ProcessPacket(LightingConsole console, int type)
        {
            console.MLCParameters.Clear();
            foreach(ParDefData data in Data)
            {
                console.MLCParameters.Add(new Stromkreise.ML.MLCParameter(data.Parno, data.DispMode, data.ParName));
            }
            return Enums.Response.OK;
        }

        public struct ParDefData
        {
            public readonly short Parno;
            public readonly Enums.SelRangeDisp DispMode;
            public readonly short DispOrder;
            public readonly short Reserve2;
            public readonly string ParName;

            public ParDefData(short parno, short dispMode, short dispOrder, short reserve2, string parName)
            {
                Parno = parno;
                DispMode = (Enums.SelRangeDisp)dispMode;
                DispOrder = dispOrder;
                Reserve2 = reserve2;
                ParName = parName;
            }
        }
    }
}
