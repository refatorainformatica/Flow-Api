using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Domain.Abstractions.Enumerations;

namespace Shared.Domain.Abstractions.Primitives
{
    public static class ResultExtensions
    {
        // Synchronous Methods
        public static Result<TOut> Bind<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Result<TOut>> func
        )
        {
            return result.IsSuccess ? func(result.Value) : Result<TOut>.Failure(result.Errors);
        }

        public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapping)
        {
            return result.IsSuccess
                ? Result<TOut>.Success(mapping(result.Value))
                : Result<TOut>.Failure(result.Errors);
        }

        public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value);
            }

            return result;
        }

        public static TOut Match<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> onSuccess,
            Func<List<Error>, TOut> onFailure
        )
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Errors);
        }

        public static Result<TOut> TryCatch<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> func,
            Error error
        )
        {
            try
            {
                return result.IsSuccess
                    ? Result<TOut>.Success(func(result.Value))
                    : Result<TOut>.Failure(result.Errors);
            }
            catch
            {
                return Result<TOut>.Failure(error);
            }
        }

        // Asynchronous Methods
        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> func
        )
        {
            return result.IsSuccess
                ? await func(result.Value)
                : Result<TOut>.Failure(result.Errors);
        }

        public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
            this Task<Result<TIn>> task,
            Func<TIn, Task<Result<TOut>>> func
        )
        {
            var result = await task;
            return result.IsSuccess
                ? await func(result.Value)
                : Result<TOut>.Failure(result.Errors);
        }

        public static async Task<Result<TOut>> MapAsync<TIn, TOut>(
            this Task<Result<TIn>> task,
            Func<TIn, TOut> mapping
        )
        {
            var result = await task;
            return result.IsSuccess
                ? Result<TOut>.Success(mapping(result.Value))
                : Result<TOut>.Failure(result.Errors);
        }

        public static async Task<Result<TIn>> TapAsync<TIn>(
            this Result<TIn> result,
            Func<TIn, Task> action
        )
        {
            if (result.IsSuccess)
            {
                await action(result.Value);
            }

            return result;
        }

        public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, Task<Result<TOut>>> func,
            Error error
        )
        {
            try
            {
                return result.IsSuccess
                    ? await func(result.Value)
                    : Result<TOut>.Failure(result.Errors);
            }
            catch (Exception ex)
            {
                var errors = new List<Error>
                {
                    new(ErrorType.Failure, ErrorType.Failure.ToString(), ex.Message),
                    new(error.Type, error.Code, error.Description),
                };
                return Result<TOut>.Failure(errors);
            }
        }

        public static async Task<Result<TOut>> TryCatchAsync<TIn, TOut>(
            this Task<Result<TIn>> task,
            Func<TIn, Task<Result<TOut>>> func,
            Error error
        )
        {
            try
            {
                var result = await task;
                return result.IsSuccess
                    ? await func(result.Value)
                    : Result<TOut>.Failure(result.Errors);
            }
            catch (Exception ex)
            {
                var errors = new List<Error>
                {
                    new(ErrorType.Failure, ErrorType.Failure.ToString(), ex.Message),
                    new(error.Type, error.Code, error.Description),
                };
                return Result<TOut>.Failure(errors);
            }
        }
    }
}
