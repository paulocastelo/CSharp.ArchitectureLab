namespace ErrorHandling.Sample.Exceptions;

public sealed class NotFoundException(string detail) : Exception(detail);
