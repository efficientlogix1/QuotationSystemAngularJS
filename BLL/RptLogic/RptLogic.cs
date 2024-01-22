using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RptLogic
    {
        public TriotechDbEntities db = new TriotechDbEntities();

        public CallBackData FetchBuyerBusinessReport(Paging paging, int buyerID)
        {
            CallBackData callBackData = new CallBackData();

            try
            {
                BuyerRequest buyerRequest = paging.SearchJson.Deserialize<BuyerRequest>();
                if (buyerID == 0)
                {
                    buyerID = buyerRequest.BuyerID;
                }
                List<FetchBuyerBusinessRecord_Result> listBuyerBusiness = db.FetchBuyerBusinessRecord(paging.DisplayLength, paging.DisplayStart, paging.SortColumn, paging.SortOrder, buyerRequest.StatusID, buyerID, buyerRequest.Search).ToList();
                if (listBuyerBusiness.Count>0)
                {
                    listBuyerBusiness.ForEach(x=> 
                    {
                        x.StrBuyerPrice = "$" + x.BuyerPrice;
                        
                    });
                    
                   
                }
                callBackData = listBuyerBusiness.ToDataTable(paging);
            }
            catch (Exception ex)
            {
                callBackData.msg.Success = false;
                callBackData.msg.MessageDetail = Message.ErrorMessage;
            }

            return callBackData;

        }
        public ReturnData PreBindBuyerReport()
        {
            ReturnData returnData = new ReturnData();
            try
            {

                var lstReturn = new
                {
                    lstBuyers = (new Generic()).FetchUserByRoleName("Buyer"),
                    lstStatus = (new Generic()).FetchStatus()
                };
                returnData.Data = lstReturn;
            }
            catch (Exception ex)
            {
                returnData.msg.Success = false;
                returnData.msg.MessageDetail = Message.ErrorMessage;
            }

            return returnData;
        }

    }
}
