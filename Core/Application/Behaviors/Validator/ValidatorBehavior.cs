using Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Behaviors.Validator
{
    public sealed class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var validationContext = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));
            var errors = validationResults.SelectMany(x => x.Errors).Where(x => x != null).Select(x => x.ErrorMessage).Distinct().ToArray();

            if (errors.Any())
                throw new ValidatorException(errors);

            return await next();


        }
    }
}
