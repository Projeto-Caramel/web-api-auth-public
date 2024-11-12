using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Base
{
    [ExcludeFromCodeCoverage]
    public class BaseService
    {
        protected void ValidateEntity<T, TEntity>(TEntity entity)
            where T : AbstractValidator<TEntity>, new()
        {
            var validatorInstance = new T();
            var result = validatorInstance.Validate(entity);

            if (!result.IsValid)
                throw new BusinessException(
                    result.Errors.Select(x => x.ErrorMessage).ToArray(),
                    StatusProcess.InvalidRequest,
                    HttpStatusCode.UnprocessableEntity);
        }
    }
}
