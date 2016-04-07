using PacificBBExtranet.Services.Models;
using PacificBBExtranet.Services.Services.Synch;
using PacificBBExtranet.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

//http://www.mikesdotnetting.com/article/268/how-to-send-email-in-asp-net-mvc
namespace PacificBBExtranet.Web.Controllers
{
    public class SynchController : Controller
    {
        private readonly SynchService _synchService;
        private readonly string XmlServiceUrl = "https://supply-xml.booking.com/hotels/xml";
        public SynchController(SynchService propService)
        {
            _synchService = propService;
        }
        public SynchController() : this(ApplicationServices.SynchService)
        {
        }

        public ActionResult Index() {
            ViewBag.InstantUpdates = _synchService.InstantUpdates();
            return View();
        }
        public ActionResult RatesListForSynch()
        {
            var rooms = _synchService.RatesListForSynch();
            return View("~/Views/Synch/_RoomsToSynchList.cshtml",rooms);
        }
        public void BookingSynchRate(int rateID,bool isStandard, List<CalendarDateValueModel> calendarDates = null)
        {
            DateTime? fromD = null;
            DateTime? toD = null;
            if (calendarDates != null)
            {
                fromD = calendarDates.First().date;
                toD = calendarDates.Last().date.AddDays(1);
            }
            var XMlModelList = ApplicationServices.XMLMessagesService.GetCompleteRateAvailabiliyMessage(rateID,isStandard, fromD,toD);
            var xmlMessage = ApplicationServices.XMLMessagesService.GetBookingRateAvMessageUpdate(XMlModelList, rateID: rateID, IsStandardRate: isStandard);


            postXMLData(this.XmlServiceUrl+"/availability?xml=", xmlMessage.ToString());
        }

        [HttpGet]
        public string HotelRemap()
        {
            var remapXml = getXMLData(this.XmlServiceUrl + "/roomrates?xml="+ApplicationServices.XMLMessagesService.HotelXmlMessageForRemap());
            var ret =_synchService.MapHotelFromXML(remapXml);
            return remapXml.InnerXml;
        }

        public string postXMLData(string destinationUrl, string requestXml)
        {
            return sendXMLData(destinationUrl,requestXml,"POST");
        }



        public XmlDocument getXMLData(string destinationUrl)
        {
            HttpWebRequest req = WebRequest.Create(destinationUrl) as HttpWebRequest;

            XmlDocument xmlDoc = new XmlDocument();
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                xmlDoc.Load(resp.GetResponseStream());
            }

            saveMessageInfo(request: Regex.Split(destinationUrl, "xml=")[1], response: xmlDoc.OuterXml.ToString());

            return xmlDoc;

        }

        private string sendXMLData(string destinationUrl, string requestXml, string method)
        {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = method;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
              //  if (response.StatusCode == HttpStatusCode.OK)
               // {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();

                    saveMessageInfo(request: requestXml , response:responseStr);
                    return responseStr;
               // }
            }catch(Exception ex)
            {
                var asd = 3;
            }
            return null;
        }

        public void saveMessageInfo(string request, string response)
        {
            var firstStep = Regex.Split(request, @"<hotel_id>")[1];
            int bookingHotelID = Convert.ToInt32(Regex.Split(firstStep, @"</hotel_id>")[0]);

            _synchService.SaveMessageResult(bookingHotelID, request,response);
        }


        public string ActivateInstantUpdates() {
            return _synchService.ActiveInstantUpdates();
        }

        public async Task<ActionResult> SendEmail(string guestname,int id,string status,double price,string rname,string guest,string remark,string AD,string DD, string currency, string email)
        { /*
            if (ModelState.IsValid)
            {
                var message = new MailMessage();
                message.Body = text;

                
                /*using (MemoryStream stream = new MemoryStream())
                {
                    stream.Flush();
                    attachment.Save(stream);
                    stream.Flush();
                    stream.Position = 0;
                    Attachment data = new Attachment(stream, "ReservationXml", MediaTypeNames.Text.Xml);
                    message.Attachments.Add(data);
                }*/
                //try {
                //    /*--*/
                //    Chilkat.Mime mime = new Chilkat.Mime();
                //    mime.NewMultipartAlternative();
                  //  Chilkat.Mime mimeXml = new Chilkat.Mime();
                //    mimeXml.SetBodyFromXml(text);
                //    mime.AppendPart(mimeXml);

                //    Chilkat.Email email = new Chilkat.Email();

                //    //email.SetFromMimeObject(mime);
                //    email.Body = mime.ToString();

                //    email.Subject = "This is a test";
                //    email.From = "support@ddd.com";
                //    email.AddTo("ddddd", "alramo53@gmail.com");

                //    Chilkat.MailMan obj = new Chilkat.MailMan();
                //    obj.SmtpPort = 587;
                //    obj.SmtpPassword = "ot103147__**__";
                //    obj.SmtpSsl = true;
                //    obj.SmtpUsername = "alramo53@gmail.com";
                //    obj.SmtpHost = "smtp.gmail.com";

                //    obj.SendEmailAsync(email);
                //}catch(Exception ex)
                //{
                //    var asd = 3;
                //}
            /*
                message.Body =string.Format("<html><head></head><body><div><pre>{0}</pre></div></body></html>", text);

            
                
                message.To.Add(new MailAddress("prasant@pacificbedbank.com"));
                //message.To.Add(new MailAddress("cherie@pacificbedbank.com"));//replace with valid value
                message.Subject = "Extranet - Booking notification";
                //message.Body = string.Format(body, "", model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                
                try {
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                        //return RedirectToAction("Sent");
                    } 
                }catch(Exception ex)
                {
                    var asd = 3;
                }
            } */

            
            if (ModelState.IsValid)
            {
                var body = "Guest Name: " + guestname + "<br/>" + "BookingID: " + id + "<br/>" + "Booking Status: " + status + "<br/>" + "Price: " + price +" "+ currency +"<br/><br/>" + "Rooms Booked: " + rname + "<br/>" + "No. Guests: " + guest + "<br/><br/>" + " Arrival Date: " + AD + "<br/>" + " Departure Date: " + DD + "<br/><br/>" + "Special Requests: " + remark;
                var message = new MailMessage();
					 message.From = new MailAddress("cisintest1@gmail.com");
                message.To.Add(new MailAddress(email)); //replace with valid value
                message.Subject = "New PacificBedBank Channel Manager Booking.";
                message.Body = body;
                message.IsBodyHtml = true;		
						
                using (var smtp = new SmtpClient("smtp.googlemail.com", 587))
                {
						smtp.Credentials = new System.Net.NetworkCredential("cisintest1@gmail.com", "cisdownload");					
						await smtp.SendMailAsync(message);                    
                }
            }

            
            return null;
        }		

		public async Task<ActionResult> GetReservations() 
      {

            var url = "https://secure-supply-xml.booking.com/hotels/xml/reservations?xml=";
            string message = ApplicationServices.XMLMessagesService.ReservationsServiceMessage();

            var xmlDoc = getXMLData(url+message);

            // await SendEmail(xmlDoc.OuterXml);
            var guestname = "";
            var Status = "";
            var Remark = "";
            int ID = 0;
            var Currency = "";
            int Email = 0;

				//string strXML = "<reservations><reservation><commissionamount>83.70</commissionamount><currencycode>FJD</currencycode><customer><address></address><cc_cvc></cc_cvc><cc_expiration_date></cc_expiration_date><cc_name></cc_name><cc_number></cc_number><cc_type></cc_type><city>.</city><company></company><countrycode>in</countrycode><dc_issue_number></dc_issue_number><dc_start_date></dc_start_date><email></email><first_name>Deepak</first_name><last_name>Bhatt</last_name><remarks>I am travelling for business and I may be using a business credit card.You have a booker that prefers communication by email</remarks><telephone></telephone><zip></zip></customer><date>2016-03-2</date ><hotel_id>1537003</hotel_id><hotel_name>Test Hotel for PacificBedbank</hotel_name><id>849245633</id><room><arrival_date>2016-03-22</arrival_date><commissionamount>83.7</commissionamount><currencycode>FJD</currencycode><departure_date>2016-03-23</departure_date><extra_info>This triple room features air conditioning.</extra_info><facilities>Air Conditioning</facilities><guest_name>Deepak Bhatt</guest_name><id>153700304</id><info>Breakfast is included in the room rate.Children and Extra Bed Policy: All children are welcome.One child under 4 years stays free of charge when using existing beds. The maximum number of extra beds/ children's cots permitted in a room is 1.  Deposit Policy: No deposit will be charged.  Cancellation Policy: If cancelled or modified up to 1 day before the date of arrival,  no fee will be charged. If cancelled or modified later or in case of no-show, the total price of the reservation will be charged. </info><max_children>0</max_children><meal_plan>Breakfast is included in the room rate.</meal_plan><name>Deluxe Triple Room</name><numberofguests>3</numberofguests><price date='2016-03-22' rate_id='6235626'>558</price><remarks></remarks><roomreservation_id>909395503</roomreservation_id><smoking></smoking><totalprice>558</totalprice></room><status>new</status><time>11:56:44</time><totalprice>558</totalprice></reservation></reservations><!-- RUID: [UmFuZG9tSVYkc2RlIyh9YW+ahlwivvw0q+bycjh4BNTj72vOhD447GNMKVYmzsnlW7mLIS3N+e/fifmVa8JS3OUO4PHhQXPI] -->";
				//XmlDocument xmlDoc = new XmlDocument();
				//xmlDoc.LoadXml(strXML);

            foreach (XmlElement element in xmlDoc.GetElementsByTagName("reservation"))
            {
                XmlElement remark = (XmlElement)element.GetElementsByTagName("remarks").Item(0);
                XmlElement status = (XmlElement)element.GetElementsByTagName("status").Item(0);
                XmlElement firstname = (XmlElement)element.GetElementsByTagName("first_name").Item(0);
                XmlElement lastname = (XmlElement)element.GetElementsByTagName("last_name").Item(0);
                XmlElement id = (XmlElement)element.GetElementsByTagName("id").Item(0);
                XmlElement currency = (XmlElement)element.GetElementsByTagName("currencycode").Item(0);
                XmlElement emailID = (XmlElement)element.GetElementsByTagName("hotel_id").Item(0);

                guestname = Convert.ToString(firstname.InnerText + " " + lastname.InnerText);
                Status = Convert.ToString(status.InnerText);
                Remark = Convert.ToString(remark.InnerText);
                ID = Convert.ToInt32(id.InnerText);
                Currency = Convert.ToString(currency.InnerText);
                Email = Convert.ToInt32(emailID.InnerText);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(element.OuterXml);

                var roomName = "";
                var adate = "";
                var ddate = "";
                var guestnum = "";
                double price = 0;

                XmlNodeList info1 = xml.SelectNodes("reservation/room");
                foreach (XmlNode room1 in info1)
                {

                    roomName = roomName + room1.SelectSingleNode("name").InnerText + "   ||   ";
                    adate = adate + Convert.ToString(room1.SelectSingleNode("arrival_date").InnerText) + "   ||   ";
                    ddate = ddate + Convert.ToString(room1.SelectSingleNode("departure_date").InnerText) + "   ||   ";
                    guestnum = guestnum + Convert.ToString(room1.SelectSingleNode("numberofguests").InnerText) + "   ||   ";
                    price = price + Convert.ToDouble(room1.SelectSingleNode("totalprice").InnerText);

                }
                //await SendEmail(email.OuterXml);

                string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                SqlConnection connection = new SqlConnection(constr);
                connection.Open();

                 SqlCommand cmd = new SqlCommand("SELECT strBookEmail FROM tblResorts WHERE intResortID=(SELECT intResortID FROM tblExResort WHERE BookingHotelID="+Email+")", connection);
                 string email = Convert.ToString(cmd.ExecuteScalar());

                await SendEmail(guestname, ID, Status, price, roomName, guestnum, Remark, adate, ddate, Currency, email);
            }

                
            

            List<XmlElement> listtoEmail = _synchService.ProcessReservations(xmlDoc);



            var reservationsModels = _synchService.GetReservationList();
            return View("~/Views/Synch/Reservations.cshtml", reservationsModels);
        }

        


    }
}