using System.Linq;
using SKMNET.Client.Networking.Client.Ext;

namespace SKMNET.Client.Stromkreise.ML
{
    /// <summary>
    /// Represents the mapping found in M665 for <see cref="FixPar"/>
    /// </summary>
    public abstract class FixParMap
    {
        public abstract short Dimmer { get; }
        public abstract short Pan { get; }
        public abstract short Tilt { get; }
        public abstract short Color { get; }
        public abstract short Zoom { get; }
        public abstract short Gobo { get; }
        public abstract short Iris { get; }
        public abstract short Focus { get; }
        public abstract short Red { get; }
        public abstract short Green { get; }
        public abstract short Blue { get; }
        public abstract short Speed { get; }
        public abstract short Cyan { get; }
        public abstract short Magenta { get; }
        public abstract short Yellow { get; }
        public abstract short Amber { get; }
        public abstract short Hue { get; }
        public abstract short Sat { get; }
        public abstract short White { get; }
        public abstract short ColTemp { get; }

        /// <summary>
        /// Gets the fixpar_t for a given name
        /// </summary>
        /// <param name="name">Parametername</param>
        /// <returns>fixpar_t</returns>
        public abstract short GetByName(string name);

        public static FixParMap Default { get; } = new DeafultFixParMap();

        public static FixParMap Active { get; set; } = Default;
    }

    public class DeafultFixParMap : FixParMap
    {
        public override short Dimmer   => 0;
        public override short Pan      => 1;
        public override short Tilt     => 2;
        public override short Color    => 5;
        public override short Zoom     => 7;
        public override short Gobo     => 9;
        public override short Iris     => 15;
        public override short Focus    => 17;
        public override short Red      => 36;
        public override short Green    => 37;
        public override short Blue     => 38;
        public override short Speed    => 39;
        public override short Cyan     => 71;
        public override short Magenta  => 72;
        public override short Yellow   => 73;
        public override short Amber    => 127;
        public override short Hue      => 131;
        public override short Sat      => 133;
        public override short White    => 152;
        public override short ColTemp => 161;

        public override short GetByName(string name)
        {
            return (short) GetType().GetFields().First(x => x.Name.Equals(name)).GetValue(this);
        }
    }
}
