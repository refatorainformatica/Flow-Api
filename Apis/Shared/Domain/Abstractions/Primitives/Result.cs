using System;
using System.Collections.Generic;

namespace Shared.Domain.Abstractions.Primitives
{
    public class Result<T>
    {
        private Result(bool isSuccess, Error error, T value = default!)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Errors.Add(error);
            Value = value;
        }

        private Result(bool isSuccess, List<Error> errors, T value = default!)
        {
            if (
                isSuccess && errors.Exists(error => error != Error.None)
                || !isSuccess && errors.Exists(error => error == Error.None)
            )
            {
                throw new ArgumentException("Invalid error");
            }

            IsSuccess = isSuccess;
            Errors = errors;
            Value = value;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public List<Error> Errors { get; } = [];

        public T Value { get; } = default!;

        public static Result<T> Success(T value) => new(true, Error.None, value);

        public static Result<T> Failure(Error error) => new(false, error);

        public static Result<T> Failure(List<Error> errors) => new(false, errors);

        public static implicit operator Result<T>(T value) => new(true, Error.None, value);

        public static implicit operator Result<T>(List<Error> errors) => new(false, errors);
    }
}
