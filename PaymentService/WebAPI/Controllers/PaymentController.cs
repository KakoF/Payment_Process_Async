using Domain.Interfaces.Application;
using Domain.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;
using WebAPI.Filters.SwaggerFilters;

namespace WebAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly ILogger<PaymentController> _logger;
		private readonly IPaymentService _service;

		public PaymentController(ILogger<PaymentController> logger, IPaymentService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpPost]
		[TypeFilter(typeof(IdempotencyFilter))]
		[SwaggerHeader("Idempotency-Key", "Chave para controle de idempotência", true)]
		public async Task<PaymentSended> CreatePaymentAsync([FromBody] CreatePaymentRequest request)
		{
			var idempotencyKey = Request.Headers["Idempotency-Key"].FirstOrDefault()!;
			return await _service.Handle(request, Guid.Parse(idempotencyKey));
		}
	}
}
