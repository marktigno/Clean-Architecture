using Domain.Shared;

namespace Domain.Entities
{
    public static class TodoEntryError
    {
        public static Error EmptyOrNull = Error.Validation("TodoEntry.EmptyOrNull", "The todo entry is empty.");
        public static Error NotFoundEntry = Error.NotFound("TodoEntry.NotFound", "Todo entry not found.");
    }
}
