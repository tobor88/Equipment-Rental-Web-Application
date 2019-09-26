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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutFormModel e)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Requestermessage = new MimeMessage();
                    Requestermessage.From.Add(new MailboxAddress("Laptop Form", "laptopform@osbornepro.com"));
                    Requestermessage.To.Add(new MailboxAddress(e.Name, e.Email));
                    Requestermessage.Subject = "Check Out Request Made";

                    var builder = new BodyBuilder();
                    var image = builder.LinkedResources.Add(@"C:\Users\Public\source\repos\CheckOutForm\CheckOutForm\wwwroot\images\logobannercolored.png");
                    image.ContentId = MimeUtils.GenerateMessageId();

                    builder.HtmlBody = string.Format($@"<p><strong>{e.Name},</strong></p>
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
                        <img src=""cid:{0}""></center>", image.ContentId);  // End mailbody

                    builder.Attachments.Add(@"C:\Users\Public\Documents\LaptopPolicy.docx");
                    Requestermessage.Body = builder.ToMessageBody();


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(e.Name, e.Email));
                    message.To.Add(new MailboxAddress("Laptop Form", "laptopform@osbornepro.com"));
                    message.Subject = "Check Out Request";

                    var builder1 = new BodyBuilder();
                    var image1 = builder1.LinkedResources.Add(@"C:\Users\Public\source\repos\CheckOutForm\CheckOutForm\wwwroot\images\logobannercolored.png");
                    image1.ContentId = MimeUtils.GenerateMessageId();

                    builder1.HtmlBody = string.Format($@"<p><strong>HR and IT,</strong></p>
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
                        <img src=""cid:{1}""></center>", image1.ContentId);  // End mailbody

                    message.Body = builder1.ToMessageBody();

                    var Managermessage = new MimeMessage();
                    Managermessage.From.Add(new MailboxAddress(e.Name, e.Email));
                    Managermessage.To.Add(new MailboxAddress("", e.Manager));
                    Managermessage.Subject = "Check Out Request Made";

                    var builder2 = new BodyBuilder();
                    var image2 = builder2.LinkedResources.Add(@"C:\Users\Public\source\repos\CheckOutForm\CheckOutForm\wwwroot\images\logobannercolored.png");
                    image2.ContentId = MimeUtils.GenerateMessageId();
                    // Set the plain-text version of the message text
                    builder2.HtmlBody = string.Format($@"<p><strong>{e.Manager},</strong></p>
                <p>{e.Name} has submitted a device request form to HR. The details are listed below. <br>
                If you wish to make any changes please alert HR and IT as well as {e.Name}. <br>
                If you wish to cancel this request click the Deny Request link at the bottom of this email to autogenerate the email to send. <br>
                If you click deny and Microsoft's Mail app opens change your default email application to Outlook. This can be done by following the instructions <a href=""https://helpdesk.osbornepro.com/articles/file-does-not-have-an-application-associated-with-it"">HERE</a> or <a href=""https://helpdesk.osbornepro.com/articles/change-default-applicaiton"">HERE</a>

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

                        <img src=""cid:{2}""></center>", image2.ContentId);  // End mailbody
                                                                             // Use to attach a file    builder2.Attachments.Add(@"C:\Users\Public\source\repos\CheckOutForm\CheckOutForm\wwwroot\images\logobanner.png");
                    Managermessage.Body = builder2.ToMessageBody();

                    //Start email Thread
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    smtp.Connect("smtp2go.com", 2525, SecureSocketOptions.StartTls);
                    smtp.Timeout = 3000;
                    smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    smtp.Send(message);
                    smtp.Send(Managermessage);
                    smtp.Send(Requestermessage);
                    smtp.Disconnect(true);
                } // End try
                catch (Exception)
                {
                    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
