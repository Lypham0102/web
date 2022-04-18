using Newtonsoft.Json.Linq;
using mvcMobileStore.Orther;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcMobileStore.Controllers
{
    public class ThanhToanMoMoController : Controller
    {
        // GET: ThanhToanMoMo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Payment()
        {
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOINCP20220412";
            string accessKey = "4WBCaRWvHYiUFhCn";
            string serectkey = "Pr6w0NnABCKmaVESOtjCxcTYoJfeyecG";
            string orderInfo = "Thanh toán MoMo";
            string returnUrl = "http://localhost:44315//ThanhToanMoMo/ComfirmPaymentClient";
            string notifyurl = "http://ba1adf48beba.ngrok.io/Home/SavePayment";

            string amount = "10000";
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;


            MomoSercurity crypto = new MomoSercurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };


            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());


            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());

        }
        public ActionResult ComfirmPaymentClient()
        {
            //hien thi thong bao cho nguoi dung
            return View();
        }
        public void SavePayment()
        {
            //cap nhat du lieu vao database
        }
    }
}