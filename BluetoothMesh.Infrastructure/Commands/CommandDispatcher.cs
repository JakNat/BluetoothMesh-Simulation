using Autofac;
using System;

namespace BluetoothMesh.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public void Dispatch<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command),
                    $"Command: '{typeof(T).Name}' can not be null.");
            }

            //wyjmujemy z kontenera klase ktora dziedziczy ICommandHandler<T>
            //np przy SendCommand command dostaniemy klase SendHandler
            var handler = _context.Resolve<ICommandHandler<T>>();
            handler.Handle(command);
        }
    }
}
