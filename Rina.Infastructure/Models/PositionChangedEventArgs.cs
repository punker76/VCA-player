using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rina.Infastructure.Models
{
    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(TimeSpan position, Double relativePosition)
        {
            Position = position;
            RelativePositioin = relativePosition;
        }

        public TimeSpan Position { get; private set; }
        public Double RelativePositioin { get; private set; }
    }
}
