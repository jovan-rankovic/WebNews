using System;

namespace Application.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException() { }

        public EntityAlreadyExistsException(string entity) : base($"{entity} already exists.") { }
    }
}