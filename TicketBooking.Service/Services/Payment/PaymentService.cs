using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TicketBooking.Common.EnvironmentSetting;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.DataModel;
using TicketBooking.Service.Models;

namespace TicketBooking.Service.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IBillRepository billRepository;
        private IUnitOfWork unitOfWork;
        private readonly VnpaySettings vnpaySettings;
        private readonly IConfiguration configuration;
        private readonly IBookingRepository bookingRepo;

        public PaymentService(IOptions<VnpaySettings> options, IConfiguration _configuration, IBillRepository billRepository, IUnitOfWork unitOfWork, IBookingRepository bookingRepo)
        {
            this.vnpaySettings = options.Value;
            this.configuration = _configuration;
            this.billRepository = billRepository;
            this.unitOfWork = unitOfWork;
            this.bookingRepo = bookingRepo;
        }

        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var booking = await bookingRepo.GetById(model.BookingId);
            if (booking == null)
            {
                return "Invalid booking";
            }
            else if (booking.IsPaid == true)
            {
                return "This booking is paid";
            }
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = configuration["PaymentCallBack:ReturnUrl"];

            //pay.AddRequestData("vnp_Version", configuration["Vnpay:Version"]);
            //pay.AddRequestData("vnp_Command", configuration["Vnpay:Command"]);
            //pay.AddRequestData("vnp_TmnCode", configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Version", vnpaySettings.Version);
            pay.AddRequestData("vnp_Command", vnpaySettings.Command);
            pay.AddRequestData("vnp_TmnCode", vnpaySettings.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)booking.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            //pay.AddRequestData("vnp_CurrCode", configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_CurrCode", vnpaySettings.CurrCode);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            //pay.AddRequestData("vnp_Locale", configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_Locale", vnpaySettings.Locale);
            pay.AddRequestData("vnp_OrderInfo", $"{model.BookingId}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            //var paymentUrl =
            //    pay.CreateRequestUrl(configuration["Vnpay:BaseUrl"], configuration["Vnpay:HashSecret"]);
            var paymentUrl =
                pay.CreateRequestUrl(vnpaySettings.BaseUrl, vnpaySettings.HashSecret);

            return paymentUrl;
        }

        public async Task<PaymentResponseModel> PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response =await pay.GetFullResponseData(collections, configuration["Vnpay:HashSecret"]);
            await SavePayment(response);
            return response;
        }

        public async Task SavePayment(PaymentResponseModel response)
        {
            var booking = await bookingRepo.GetById(new Guid(response.OrderDescription));
            var bill = new Bill()
            {
                Amount = response.Amount,
                OrderId = response.OrderId,
                Description = response.OrderDescription,
                CreatedDate = response.PayDate,
                PaymentTranId = response.PaymentId,
                BankCode = response.BankCode,
                PayStatus = response.Success.ToString(),
                BookingId = booking.Id,
            };
            await billRepository.Add(bill);
            if(response.PaymentStatus== "Success") {
                booking.IsPaid= true;
            }
            booking.Bills.Add(bill);
            bookingRepo.Update(booking);
            await unitOfWork.CompletedAsync();
        }
    }
}
