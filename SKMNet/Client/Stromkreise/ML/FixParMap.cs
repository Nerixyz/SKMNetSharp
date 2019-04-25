using System.Linq;

namespace SKMNET.Client.Stromkreise.ML
{
    /// <summary>
    /// Represents the mapping found in M665 for <see cref="Networking.Client.FixPar"/>
    /// </summary>
    public abstract class FixParMap
    {
        public abstract short DIMMER { get; }
        public abstract short PAN { get; }
        public abstract short TILT { get; }
        public abstract short COLOR { get; }
        public abstract short ZOOM { get; }
        public abstract short GOBO { get; }
        public abstract short IRIS { get; }
        public abstract short FOCUS { get; }
        public abstract short RED { get; }
        public abstract short GREEN { get; }
        public abstract short BLUE { get; }
        public abstract short SPEED { get; }
        public abstract short CYAN { get; }
        public abstract short MAGENTA { get; }
        public abstract short YELLOW { get; }
        public abstract short AMBER { get; }
        public abstract short HUE { get; }
        public abstract short SAT { get; }
        public abstract short WHITE { get; }
        public abstract short COL_TEMP { get; }

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
        public override short DIMMER   => 0;
        public override short PAN      => 1;
        public override short TILT     => 2;
        public override short COLOR    => 5;
        public override short ZOOM     => 7;
        public override short GOBO     => 9;
        public override short IRIS     => 15;
        public override short FOCUS    => 17;
        public override short RED      => 36;
        public override short GREEN    => 37;
        public override short BLUE     => 38;
        public override short SPEED    => 39;
        public override short CYAN     => 71;
        public override short MAGENTA  => 72;
        public override short YELLOW   => 73;
        public override short AMBER    => 127;
        public override short HUE      => 131;
        public override short SAT      => 133;
        public override short WHITE    => 152;
        public override short COL_TEMP => 161;

        public override short GetByName(string name)
        {
            return (short) GetType().GetFields().First(x => x.Name.Equals(name)).GetValue(this);
        }
    }
}
