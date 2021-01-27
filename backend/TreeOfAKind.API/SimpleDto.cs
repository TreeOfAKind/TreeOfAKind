using System;

namespace TreeOfAKind.API
{
    public class IdDto
    {
        public Guid Id { get; set; }
    }

    public class UriDto
    {
        public Uri Uri { get; set; }
    }

    public class IdUriDto
    {
        public Guid Id { get; set; }
        public Uri Uri { get; set; }
    }
}
