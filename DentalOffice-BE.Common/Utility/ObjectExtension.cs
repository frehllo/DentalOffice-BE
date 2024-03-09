using System.Diagnostics.CodeAnalysis;

namespace DentalOffice_BE.Common.Utility;

public static class ObjectExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? o) => o ?? throw new InvalidOperationException();

    [return: NotNull]
    public static T ThrowIfNull<T>(this T? o, string exceptionMessage) => o ?? throw new InvalidOperationException(exceptionMessage);

    [return: NotNull]
    public static T ThrowIfNull<T, TException>(this T? o) where TException : Exception, new() => o ?? throw new TException();

    [return: NotNull]
    public static T ThrowIfNull<T, TException>(this T? o, string message) where TException : Exception, new() => o ?? throw (Activator.CreateInstance(typeof(TException), new object?[] { message }) as TException).ThrowIfNull();

    [return: NotNull]
    public static string ThrowIfNullOrWhiteSpace(this string? o) => string.IsNullOrWhiteSpace(o) ? throw new InvalidOperationException() : o;

    [return: NotNull]
    public static string ThrowIfNullOrWhiteSpace(this string? o, string exceptionMessage) => string.IsNullOrWhiteSpace(o) ? throw new InvalidOperationException(exceptionMessage) : o;
}
