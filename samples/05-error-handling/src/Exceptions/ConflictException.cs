namespace ErrorHandling.Sample.Exceptions;

public sealed class ConflictException(string detail) : Exception(detail);
