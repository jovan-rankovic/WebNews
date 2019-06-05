using System;

namespace Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string entity) : base($"{entity} not found.") { }
    }
}