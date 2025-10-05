using Domain.Exceptions;
using Domain.Interfaces.Infrastructure.Idempotency;
using Domain.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
	public class IdempotencyFilter : IAsyncActionFilter
	{
		private readonly IIdempotencyService _idempotencyService;

		public IdempotencyFilter(IIdempotencyService idempotencyService)
		{
			_idempotencyService = idempotencyService;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var httpContext = context.HttpContext;
			var idempotencyKey = httpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

			if (string.IsNullOrEmpty(idempotencyKey))
				throw new DomainException("Idempotency-Key header is required");

			var paymentSended = await _idempotencyService.AlreadyProcessAsync(Guid.Parse(idempotencyKey));
			if (paymentSended != null)
			{
				context.Result = new OkObjectResult(paymentSended);
				return;
			}

			var resultContext = await next();

			await _idempotencyService.SetNewProcessAsync(Guid.Parse(idempotencyKey));
		}
	}
}
