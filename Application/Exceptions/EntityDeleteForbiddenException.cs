using System;

namespace Application.Exceptions
{
    public class EntityDeleteForbiddenException : Exception
    {
        public EntityDeleteForbiddenException() { }

        public EntityDeleteForbiddenException(string entity) : base($"You don't have permission to delete this {entity}.") { }
    }
}