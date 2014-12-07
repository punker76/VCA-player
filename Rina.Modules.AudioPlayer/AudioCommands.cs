using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.Composition;

namespace Rina.Modules.AudioPlayer
{
    public static class AudioCommands
    {
        private readonly static CompositeCommand changeStateCommand = new CompositeCommand();
        private readonly static CompositeCommand moveNextCommand = new CompositeCommand();
        private readonly static CompositeCommand movePrevCommand = new CompositeCommand();

        public static CompositeCommand ChangeStateCommand { get { return changeStateCommand; } }
        public static CompositeCommand MoveNextCommand { get { return moveNextCommand; } }
        public static CompositeCommand MovePrevCommand { get { return movePrevCommand; } }
    }

    [Export( typeof(AudioCommandsProxy))]
    public class AudioCommandsProxy
    {
        public virtual CompositeCommand ChangeStateCommand
        {
            get { return AudioCommands.ChangeStateCommand; }
        }
        public virtual CompositeCommand MoveNextCommand
        {
            get { return AudioCommands.MoveNextCommand; }
        }
        public virtual CompositeCommand MovePrevCommand
        {
            get { return AudioCommands.MovePrevCommand; }
        }
    }
}
