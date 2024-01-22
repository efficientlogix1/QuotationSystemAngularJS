using System;
using DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
  public  class DepartmentLogic
    {


        public TriotechDbEntities db = new TriotechDbEntities();
        public Message Save(Department department, AspNetUser userprofile)
        {
            Message msg = new Message();
            try
            {
                if (department.ID != 0)
                {
                    msg.Action = "Update";
                    Department founddepartment = db.Departments.FirstOrDefault(x => x.ID == department.ID);
                    founddepartment.Name = department.Name;
                    founddepartment.IsActive = department.IsActive;
                    db.Entry(founddepartment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                else
                {
                    msg.Action = "Save";
                    db.Departments.Add(department);
                    db.SaveChanges();

                }
                msg.MessageDetail = "Department (" + department.Name + ") has been " + msg.Action + "d";
                userprofile.activityLog(msg.Action, "<b>Department (" + department.Name + ")</b> has been " + msg.Action + "d", department.ID, "Location");
            }
            catch (Exception ex)
            {
                msg.Success = false;
                msg.MessageDetail = Message.ErrorMessage;
                userprofile.activityLog(msg.Action, msg.MessageDetail, department.ID, "Department");
                //userProfile.LogError(ex, "BLL/UserTypeLogic/Save");
            }

            return msg;
        }
        public CallBackData Fetch(Paging paging)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                CommonFilters filters = paging.SearchJson.Deserialize<CommonFilters>();
                List<FetchDepartment_Result> listDepartment = db.FetchDepartment(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, filters.Search, filters.Active).ToList();
                callBackData = listDepartment.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData FetchDepartmentById(int departmentID)
        {
            ReturnData returnData = new ReturnData();

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Department department = db.Departments.AsNoTracking().FirstOrDefault(u => u.ID == departmentID);
                returnData.Data = department;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
                // Static.UserProfile.LogError(ex, "BLL/CashLogic/Fetch");
            }

            return returnData;

        }
    }
}
