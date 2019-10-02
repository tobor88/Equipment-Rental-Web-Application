using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Utils;
using CheckOutForm.Models;
using MailKit.Security;

namespace CheckOutForm.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public ActionResult Index()
        {
            return View();
        }

        static bool mailSent = false;

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutFormModel e)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    SmtpClient client0 = new SmtpClient("mail.smtp2go.com", 2525);
                    SmtpClient client1 = new SmtpClient("mail.smtp2go.com", 2525);
                    SmtpClient client2 = new SmtpClient("mail.smtp2go.com", 2525);

                    MailAddress ithr = new MailAddress("laptopform@osbornepro.com", "OsbornePro Form ", System.Text.Encoding.UTF8);
                    MailAddress requester = new MailAddress(e.Email, e.Name, System.Text.Encoding.UTF8);
                    MailAddress manager = new MailAddress(e.Manager, "Manager", System.Text.Encoding.UTF8);

                    MailMessage message0 = new MailMessage(ithr, requester);
                    MailMessage message1= new MailMessage(requester, ithr);
                    MailMessage message2 = new MailMessage(ithr, manager);

                    Attachment policyDoc = new Attachment("C:\\Users\\Public\\Documents\\LaptopPolicy.docx");

                    message0.Body = string.Format($@"<p><strong>{e.Name},</strong></p>
                        <p>Your request has been received. You will be contacted after your request is processed.</p>
                        <p>Your Request details are listed below. The OsbornePro Laptop Policy is also attached to this email. <br>
                <br>
                            <strong>Name:</strong> {e.Name} <br>
                            <strong>Email:</strong> {e.Email}<br>
                            <strong>Laptop:</strong> {e.Sizes} inch Laptop Screen  <br>
                            <strong>Projector:</strong> {e.Projectors} Lumens <br>
                            <strong>HotSpots:</strong> {e.HotSpots} <br>
                <br>
                            <strong>Dates:</strong> {e.DateStart} to {e.DateEnd}   <br>
                <br>
                            <strong>Reason for Request:</strong> ""{e.Reasons}""   <br>
                <br>
                            <strong>Applications:  </strong>      <br>
                               <strong> - Office365: </strong>       {e.Office365}     <br>
                               <strong> - Bitwarden: </strong>             {e.Bitwarden}           <br>
                               <strong> - ProtonMail: </strong>      {e.ProtonMail}    <br>
                               <strong> - NordVPN: </strong>    {e.NordVPN}  <br>
                <br>
                            <strong>Message:</strong> ""{e.Information}""          <br>
                <br>
                        Thank you for your submission,            <br>
                        - OsbornePro IT Department            <br>
                        <img src=""cid:{0}""></center>");  // End mailbody

                    message0.Attachments.Add(policyDoc);
                    message0.Body += Environment.NewLine ;
                    message0.IsBodyHtml = true;
                    message0.BodyEncoding = System.Text.Encoding.UTF8;
                    message0.Subject = "Equipment Check Out Request Made";
                    message0.SubjectEncoding = System.Text.Encoding.UTF8;

                    message1.Body = string.Format($@"<p><strong>HR and IT,</strong></p>
                        <p>Equipment request was submitted for {e.Name}. The details are below. <br> To approve this request, forward this email to help@osbornepro.com or click the Apply link to autogenerate the approval email.</p>
                        <p>Your Request details are listed below.
                <br>
                            <strong>Name:</strong> {e.Name} <br>
                            <strong>Email:</strong> {e.Email}<br>
                            <strong>Laptop:</strong> {e.Sizes} inch Laptop Screen  <br>
                            <strong>Projector:</strong> {e.Projectors} Lumens <br>
                            <strong>HotSpots: </strong>{e.HotSpots} <br>
                <br>
                            <strong>Dates:</strong> {e.DateStart} to {e.DateEnd}   <br>
                <br>
                            <strong>Reason for Request:</strong> ""{e.Reasons}""   <br>
                <br>
                       <strong>Applications: </strong>                        <br>
                              <strong>  - Office365:</strong>      {e.Office365}     <br>
                               <strong> - Bitwarden:</strong>            {e.Bitwarden}           <br>
                               <strong> - ProtonMail:</strong>     {e.ProtonMail}    <br>
                               <strong> - NordVPN:</strong>   {e.NordVPN}  <br>
                <br>
                            <strong>Message:</strong> ""{e.Information}""          <br>
                <br>
                    </p>

                    <span>
                        <a href=""mailto:help@osbornepro.com?cc={e.Email}&subject=Approved%20Request&body={@e.Name}s%20Equipment%20Rental%20has%20been%20approved!%0A%0A{@e.Reasons}""><strong>Approve Request</strong>
                        <br> </a> 
                    </span>

                    <span>
                        <a href=""mailto:{e.Email}?cc={e.Manager}&subject=Denied%20Request&body=We%20are%20sorry%20{@e.Name}%20your%20Equipment%20Request%20has%20been%20denied.""><strong>Deny Request</strong>
                        <br> </a>
                    </span>

                    </div>
                        <img src=""cid:{1}""></center>");  // End mailbody
                    message1.Body += Environment.NewLine;
                    message1.IsBodyHtml = true;
                    message1.BodyEncoding = System.Text.Encoding.UTF8;
                    message1.Subject = "Equipment Check Out Request Made";
                    message1.SubjectEncoding = System.Text.Encoding.UTF8;

                    message2.Body = string.Format($@"<p><strong>{e.Manager},</strong></p>
                <p>{e.Name} has submitted a device request form to HR. The details are listed below. <br>
                If you wish to make any changes please alert HR and IT as well as {e.Name}. <br>
                If you wish to cancel this request click the Deny Request link at the bottom of this email to autogenerate the email to send. <br>
                If you click deny and Microsoft's Mail app opens change your default email application to Outlook. This can be done by following the instructions <a href=""https://helpdesk.usav.org/kb/articles/file-does-not-have-an-application-associated-with-it"">HERE</a> or <a href=""https://helpdesk.usav.org/kb/articles/change-default-applicaiton"">HERE</a>

                < br>
                            <strong>Name:</strong> {e.Name} <br>
                            <strong>Email:</strong> {e.Email} <br>
                            <strong>Laptop:</strong> {e.Sizes} inch Laptop Screen  <br>
                            <strong>Projector:</strong> {e.Projectors} Lumens      <br>
                            <strong>HotSpots:</strong> {e.HotSpots}                <br>
                <br>
                            <strong>Dates:</strong> {e.DateStart} to {e.DateEnd}   <br>
                <br>
                            <strong>Reason for Request:</strong> ""{e.Reasons}""   <br>
                <br>
                            <strong>Applications:</strong>                       <br>
                              <strong> - Office365: </strong>     {e.Office365}      <br>
                              <strong> - Bitwarden: </strong>           {e.Bitwarden}            <br>
                              <strong> - ProtonMail: </strong>    {e.ProtonMail}     <br>
                              <strong> - NordVPN: </strong>  {e.NordVPN}   <br>
                <br>
                            <strong>Message:</strong> ""{e.Information}""           <br>
                <br>
                If you have any concerns, updates, or changes to this request; or do not approve of this request; let HR and/or the IT Department know as soon as possible.</p>
                    
                    <span>
                        <a href=""mailto:hr@osbornepro.com?cc={e.Email}&subject=Manager%20denied%20Request&body=We%20are%20sorry%20{@e.Name}%20your%20Equipment%20Request%20has%20been%20denied.""><strong>Deny Request</strong>
                        <br> </a>
                    </span>

                        <img src=""cid:{2}""></center>");  // End mailbody
                    message2.Body += Environment.NewLine;
                    message2.IsBodyHtml = true;
                    message2.BodyEncoding = System.Text.Encoding.UTF8;
                    message2.Subject = "Equipment Check Out Request Made";
                    message2.SubjectEncoding = System.Text.Encoding.UTF8;

                    client0.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);
                    client1.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);
                    client2.SendCompleted += new
                    SendCompletedEventHandler(SendCompletedCallback);

                    string userState0 = "Emails sending to inform user their request was submitted... ";
                    string userState1 = "Emails sending to inform HR for approval of laptop request... ";
                    string userState2 = "Emails sending to the requesters manager informting them of the request... ";

                    client0.SendAsync(message0, userState0);
                    client1.SendAsync(message1, userState1);
                    client2.SendAsync(message2, userState2);

                    Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");

                    string answer = Console.ReadLine();

                    if (answer.StartsWith("c") && mailSent == false)
                    {
                        client0.SendAsyncCancel();
                        client1.SendAsyncCancel();
                        client2.SendAsyncCancel();
                    }

                    message0.Dispose();
                    message1.Dispose();
                    message2.Dispose();
                    
                    client0.Dispose();
                    client1.Dispose();
                    client2.Dispose();

                    Console.WriteLine("Goodbye.");
                } // End try
                catch (Exception)
                {
                    return View("Error");
                }
            }

            return View("LaptopPolicy");
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult LaptopPolicy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
