using System.Diagnostics.CodeAnalysis;

namespace Domain.Abstractions;

/// <summary>
///     A monad representing an optional value
/// </summary>
/// <typeparam name="T">The internal value.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "By design.")]
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "If you can think of something better...")]
public readonly struct Option<T>
{
    private Option(T value) => Value = value;

    public static Option<T> Create(T value) => value is null ? None : new Option<T>(value);

    public T Value { get; }

    public bool HasValue => Value is not null;

    public static readonly Option<T> None = new(default!);

    public T ValueOrDefault(T elseValue) => HasValue ? Value : elseValue;

    public Option<TResult> Select<TResult>(Func<T, TResult> map) => HasValue
        ? Option<TResult>.Create(map(Value))
        : Option<TResult>.None;

    public static implicit operator Option<T>(T value) => Create(value);
}
