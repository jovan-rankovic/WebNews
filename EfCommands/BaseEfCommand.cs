using EfDataAccess;

namespace EfCommands
{
    public abstract class BaseEfCommand
    {
        protected readonly WebNewsContext Context;

        protected BaseEfCommand(WebNewsContext context)
            => Context = context;
    }
}