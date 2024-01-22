using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;


namespace BLL
{
    public static class Extension
    {

        public static AspNetUser FetchUserProfile(this int aspNetUser_Id)
        {
            return
            (new Common()).db.AspNetUsers.FirstOrDefault(x => x.Id == aspNetUser_Id);
        }
        public static void activityLog(this AspNetUser userProfile, string action, string detail, long? recordId, string tableName)
        {
            try
            {
                ActivityLog activityLog = new ActivityLog();
                activityLog.Action = action;
                activityLog.CreateDate = DateTime.UtcNow;
                activityLog.Detail = detail;
                activityLog.RecordId = recordId;
                activityLog.TableName = tableName;
                activityLog.User_Id = userProfile.Id;
                var db = (new Common()).db;
                db.ActivityLogs.Add(activityLog);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }
        public static bool SendEmail(this EmailTemplate emailTemplate)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["Port"]);
                client.Host = WebConfigurationManager.AppSettings["ServerHost"];
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["Email"], WebConfigurationManager.AppSettings["Password"]);
                mailMessage.From = new MailAddress(WebConfigurationManager.AppSettings["DisplayEmail"].ToString(), WebConfigurationManager.AppSettings["DisplayName"]);
                if (!String.IsNullOrEmpty(emailTemplate.To))
                {
                    emailTemplate.To.Split(',').ToList().ForEach(x =>
                    {
                        if (!String.IsNullOrWhiteSpace(x))
                            mailMessage.To.Add(new MailAddress(x));
                    });
                }
                if (!String.IsNullOrEmpty(emailTemplate.CC))
                {
                    emailTemplate.CC.Split(',').ToList().ForEach(x =>
                    {
                        if (!String.IsNullOrWhiteSpace(x))
                            mailMessage.CC.Add(new MailAddress(x));
                    });
                }
                if (!String.IsNullOrEmpty(emailTemplate.BCC))
                {
                    emailTemplate.BCC.Split(',').ToList().ForEach(x =>
                    {
                        if (!String.IsNullOrWhiteSpace(x))
                            mailMessage.Bcc.Add(new MailAddress(x));
                    });
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = emailTemplate.Subject;
                mailMessage.Body = emailTemplate.MessageBody;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static CallBackData ToDataTable<T>(this List<T> list, Paging paging)
        {
            CallBackData callBackData = new CallBackData();
            callBackData.Data.data = list;
            callBackData.Data.draw = paging.Draw;
            callBackData.Data.recordsTotal = callBackData.Data.data.Count;
            callBackData.Data.recordsFiltered = (callBackData.Data.data.Count != 0 ? ((int)callBackData.Data.data[0].GetType().GetProperty("Total").GetValue(callBackData.Data.data[0], null)) : 0);

            return callBackData;
        }
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static Paging FetchPaging(this HttpRequestBase request)
        {
            Paging paging = new Paging();
            try
            {
                paging.Draw = Convert.ToInt32(request.QueryString["sEcho"]);
                paging.SearchJson = Convert.ToString(request.QueryString["SearchJson"]);
                paging.DisplayLength = Convert.ToInt32(request.QueryString["iDisplayLength"]);
                paging.DisplayStart = Convert.ToInt32(request.QueryString["iDisplayStart"]);
                paging.SortColumn = Convert.ToInt32(request.QueryString["iSortCol_0"]);
                paging.Search = Convert.ToString(request.QueryString["sSearch"]);
                paging.SortOrder = Convert.ToString(request.QueryString["sSortDir_0"]);
            }
            catch { }

            return paging;
        }
        public static DateTime? DbDate(this string date, bool toUtc = false)
        {

            if (!string.IsNullOrEmpty(date))
            {
                //var asd=  DateTime.UtcNow.ToUniversalTime();
                //  var singapore = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                // var ss= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, singapore);
                DateTime dt = DateTime.Parse(date, CultureInfo.GetCultureInfo("en-GB"));
                if (toUtc)
                    return DateTimeOffset.Parse(string.Format("{0:MM/dd/yyyy}", dt)).UtcDateTime;

                else
                    return dt;// DateTime.Parse(date, CultureInfo.GetCultureInfo("en-GB"));
            }
            else
                return null;
        }
        public static string ViewDate(this DateTime date, double timeZoneHours = 0, bool isAddTime = false)
        {
            if (date == DateTime.MinValue)
            {
                return String.Empty;
            }
            else if (!ReferenceEquals(date, null))
            {
                if (((DateTime)date).Hour != 0)
                {
                    date = ((DateTime)date).AddHours(timeZoneHours);
                }
            }
            if (isAddTime)
                return string.Format("{0:dd/MM/yyyy hh:mm:ss tt}", date);

            return string.Format("{0:dd/MM/yyyy}", date);
        }
        public static string FetchUniquePath(this string directoryPath, string imageName)
        {
            string extension = Path.GetExtension(imageName);
            string fileName = DateTime.UtcNow.Ticks.ToString();

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            int i = 0;
            while (File.Exists(directoryPath + "/" + fileName + i + extension))
                i++;

            return (fileName + i + extension);
        }
    };
}
