using FluentValidation;
using MediatR;
using MovieReviewApi.Domain.Common;

namespace MovieReviewApi.Application.Behaviors
{
    public sealed class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context)));

            var errors = validationFailures
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new Error(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

                if (errors.Any())
                {
                    Console.WriteLine("Validation Errors Occurred:");
                    foreach (var error in errors)
                        Console.WriteLine($"Property: {error.Code}, Error: {error.Description}");

                    //reflection to get the instance of TResponse e.g Result<ActorDto>
                    var failureFactory = typeof(TResponse).GetMethod(
                        "Failure",
                        new[] { typeof(List<Error>) });

                    if (failureFactory is not null)
                    {
                    //Result<ActorDto>.Failure(List<errors>), first argument is null because Failure is Static 
                        return (TResponse)failureFactory.Invoke(null, new object[] { errors })!;
                    }

                    return default!;
                }
            

            var response = await next();

            return response;
        }
    }
}