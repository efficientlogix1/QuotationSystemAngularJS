using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public class Common
    {
        public TriotechDbEntities db = new TriotechDbEntities();
        

        //public static List<ChatUsers> lstChatUser = new List<ChatUsers>();


    }
    //public class ChatUsers
    //{
    //    public string ChatId { get; set; }
    //    public string Id { get; set; }
    //    public long UserId { get; set; }
    //    public string UserName { get; set; }
    //    public string Email { get; set; }
    //    public string ConnectionId { get; set; }
    //}
    
    public class Message
    {
        public static string ErrorMessage = "Something went wrong. Please try again.";
        public bool Success = true;
        public string Action { get; set; }
        public bool Info { get; set; }
        public bool Warning { get; set; }
        public string MessageDetail { get; set; }
        public long ID { get; set; }

        public static string SaveMessage = "Record has been saved";
        public static string UpdateMessage = "Record has been updated";
        public static string ProfileImage = "ProfileImage has been updated";
        public static string ImageError = "Please Select an Image";
    }
    public class Paging
    {
        public int Draw { get; set; }
        public int DisplayStart { get; set; }
        public int DisplayLength { get; set; }
        public int SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }
        public string SearchJson { get; set; }
    }
    public class CallBackData
    {
        public Message msg = new Message();

        public JqueryDataTable Data = new JqueryDataTable();
    }
    public class JqueryDataTable
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public dynamic data { get; set; }
    }
    public class ReturnData
    {
        public Message msg = new Message();
        public dynamic Data { get; set; }

    }
    public class DropDownList
    {
        public long ID { get; set; }
        public string NameStr { get; set; }
        public string Name { get; set; }
    }

    public class BarCharts
    {
        public List<string> labels { get; set; }
        public List<BarChartData> datasets { get; set; }
    }
    public class BarChartData
    {
        public string label { get; set; }
        public List<string> backgroundColor { get; set; }
        public List<string> borderColor { get; set; }
        public int borderWidth { get; set; }
        public List<decimal?> data { get; set; }
    }

    public class LineCharts
    {
        public List<string> labels { get; set; }
        public List<LineChartData> datasets { get; set; }
    }
    public class LineChartData
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string borderCapStyle { get; set; }
        public List<decimal?> data { get; set; }
        public bool fill { get; set; }
        public double lineTension { get; set; }
        public List<string> borderDash { get; set; }
        public double borderDashOffset { get; set; }
        public string borderJoinStyle { get; set; }
        public string pointBorderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public int pointBorderWidth { get; set; }
        public int pointHoverRadius { get; set; }
        public string pointHoverBackgroundColor { get; set; }
        public string pointHoverBorderColor { get; set; }
        public int pointHoverBorderWidth { get; set; }
        public int pointRadius { get; set; }
        public int pointHitRadius { get; set; }
        public bool spanGaps { get; set; }
    }
    [Serializable]
    public class UploadFile
    {
        public long ID { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Description { get; set; }
        public Nullable<long> PatientID { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }

    }
}
