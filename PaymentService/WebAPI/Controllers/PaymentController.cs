using Domain.Interfaces.Application;
using Domain.Records;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<PaymentSended> CreatePaymentAsync([FromBody] CreatePaymentRequest request)
		{
			return await _service.Handle(request);
		}
	}
}
