using GotheTollway.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GotheTollway.API.Controllers
{
    public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        [HttpPost]
        [Route("api/invoice/createInvoice")]
        public async Task<IActionResult> CreateInvoice([FromQuery] string registrationNumber)
        {
            if(string.IsNullOrEmpty(registrationNumber)) 
            {
                return BadRequest("RegistrationNumber cannot be null or empty.");
            }

            var invoice = await _invoiceService.CreateInvoice(registrationNumber);
            
            return invoice.Result.IsSuccess ? Ok(invoice.Value) : BadRequest(invoice.Result.Error);
        }
    }
}
