namespace ErrorHandling.Sample.Exceptions;

public sealed class BusinessRuleException(string detail) : Exception(detail);
