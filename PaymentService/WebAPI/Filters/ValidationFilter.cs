using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WebAPI.Middlewares;

namespace WebAPI.Filters
{
	public class ValidationFilter<T> : IAsyncActionFilter where T : class
	{
		private readonly IValidator<T> _validator;

		public ValidationFilter(IValidator<T> validator)
		{
			_validator = validator;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

			if (model == null)
			{
				context.Result = new BadRequestObjectResult(new ErrorResponse((int)HttpStatusCode.BadRequest, "Some erros founded in input data"));
				return;
			}

			var result = await _validator.ValidateAsync(model);

			if (!result.IsValid)
			{
				var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
				context.Result = new BadRequestObjectResult(new ErrorResponse((int)HttpStatusCode.BadRequest, "Some erros founded in input data", errors));
				return;
			}

			await next();
		}
	}
}

