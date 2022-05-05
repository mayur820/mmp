using BAL;
using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAL
    {
    public class SubscriberDAL
        {
        //To Insert Subscriber Master data
        public Subscriber InsertSubscriberMaster(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var RoleId = HttpContext.Current.Session["RoleId"];
            //   var Case = 1;
            HttpContext.Current.Session["SubscriptionLimits"] = _Subscriber.MemberSubscription;
            HttpContext.Current.Session["SubscriberName"] = _Subscriber.SubscriberName;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", _Subscriber.Case);
            Adapter.AddParam(pcol, "@SubscriberName", _Subscriber.SubscriberName);
            Adapter.AddParam(pcol, "@EmailID", _Subscriber.EmailID);
            Adapter.AddParam(pcol, "@MobileNo", _Subscriber.MobileNo);
            Adapter.AddParam(pcol, "@MobileOTP", _Subscriber.MobileOTP);
            Adapter.AddParam(pcol, "@EmailOTP", _Subscriber.EmailOTP);
            Adapter.AddParam(pcol, "@RoleId", _Subscriber.RoleId);
            Adapter.AddParam(pcol, "@MemberSubscription", _Subscriber.MemberSubscription);
            Adapter.AddParam(pcol, "@SubscriberID", _Subscriber.SubscriberID);
            Adapter.AddParam(pcol, "@CrPassword", _Subscriber.CreatePassword);
            //  Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            //Adapter.ExecutenNonQuery("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt != null)
                {
                if (dt.Rows.Count > 0)
                    {
                    obj.Code = dt.Rows[0]["Code"].ToString();
                    obj.Message = dt.Rows[0]["Message"].ToString();
                    var data = HttpContext.Current.Session["MemberCount"];
                    HttpContext.Current.Session["MemberCount"] = data;
                    if (obj.Code == "200")
                        {
                        obj.SubscriberID = (Int32)dt.Rows[0]["SubscriberID"];
                        obj.MemberSubscription = (Int32)dt.Rows[0]["MemberSubscription"];
                        obj.EmailID = dt.Rows[0]["EmailID"].ToString();
                        obj.EmailOTP = dt.Rows[0]["EmailOTP"].ToString();
                       // obj.OTPID = Convert.ToInt32(dt.Rows[0]["OTPID"].ToString());
                        HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                        HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                        HttpContext.Current.Session["EmailID"] = dt.Rows[0]["EmailID"];
                        HttpContext.Current.Session["EmailOTP"] = dt.Rows[0]["EmailOTP"];
                       // HttpContext.Current.Session["OTPID"] = dt.Rows[0]["OTPID"];
                        }

                    }
                }
            return obj;
            }

        public void InsertOTPDT(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var Subscriberid = HttpContext.Current.Session["SubscriberID"];
            //    var Case = 1;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", _Subscriber.Case);
            Adapter.AddParam(pcol, "@SubscriberID", Subscriberid);
            Adapter.AddParam(pcol, "@EmailOTP", _Subscriber.EmailOTP);
            Adapter.AddParam(pcol, "@MobileOTP", _Subscriber.MobileOTP);
          //  Adapter.AddParam(pcol, "@OTPID", _Subscriber.OTPID);
            dt = Adapter.ExecuteDataTable("USPOTPInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt != null)
                {
                if (dt.Rows.Count > 0)
                    {
                    // obj.Code = dt.Rows[0]["Code"].ToString();
                    obj.Message = dt.Rows[0]["Message"].ToString();
                    HttpContext.Current.Session["Message"] = obj.Message;

                    }
                }
            }

        // To Search Menu's 
        public List<Subscriber> VerifyDate()
            {
            Subscriber _sb = new Subscriber();
            List<Subscriber> Sblist = new List<Subscriber>();
            // var Case = 3;
            var Subscriberid = HttpContext.Current.Session["SubscriberID"];
            DataTable dt = new DataTable();
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            //  Adapter.AddParam(pcol, "@Case", Case);
            Adapter.AddParam(pcol, "@SubscriberID", Subscriberid);
            dt = Adapter.ExecuteDataTable("USPGetVerifyDate", CommandType.StoredProcedure, Adapter.param(pcol));
            Sblist = new List<Subscriber>();
            if (dt.Rows.Count > 0)
                {
                _sb.OTPVerifiedDate = Convert.ToDateTime(dt.Rows[0]["OTPVerifiedDate"].ToString());
                HttpContext.Current.Session["OTPVerifiedDate"] = _sb.OTPVerifiedDate;

                }
            return Sblist;
            }

        //To Insert Subscriber Master data
        public Subscriber RegisterSubscriberMaster(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 1;
            HttpContext.Current.Session["SubscriptionLimits"] = _Subscriber.MemberSubscription;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            Adapter.AddParam(pcol, "@SubscriberName", _Subscriber.SubscriberName);
            Adapter.AddParam(pcol, "@EmailID", _Subscriber.EmailID);
            Adapter.AddParam(pcol, "@MobileNo", _Subscriber.MobileNo);
            Adapter.AddParam(pcol, "@RoleId", _Subscriber.RoleId);
            Adapter.ExecutenNonQuery("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                obj.Code = dt.Rows[0]["Code"].ToString();
                obj.Message = dt.Rows[0]["Message"].ToString();
                if (obj.Code == "200")
                    {
                    obj.SubscriberID = (Int32)dt.Rows[0]["SubscriberID"];
                    obj.MemberSubscription = (Int32)dt.Rows[0]["MemberSubscription"];
                    HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                    //   HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                    }

                }

            return obj;
            }

        //To Insert Subscriber Master data
        public Subscriber OTPSubscriberMaster(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 2;
            HttpContext.Current.Session["SubscriptionLimits"] = _Subscriber.MemberSubscription;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            //Adapter.AddParam(pcol, "@SubscriberName", _Subscriber.SubscriberName);
            //Adapter.AddParam(pcol, "@EmailID", _Subscriber.EmailID);
            //Adapter.AddParam(pcol, "@MobileNo", _Subscriber.MobileNo);
            Adapter.AddParam(pcol, "@MobileOTP", _Subscriber.MobileOTP);
            Adapter.AddParam(pcol, "@EmailOTP", _Subscriber.EmailOTP);
            //Adapter.AddParam(pcol, "@RoleId", _Subscriber.RoleId);
            ////Adapter.AddParam(pcol, "@MemberSubscription", _Subscriber.MemberSubscription);
            //Adapter.AddParam(pcol, "@Active", _Subscriber.Active);
            //Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            Adapter.ExecutenNonQuery("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                obj.Code = dt.Rows[0]["Code"].ToString();
                obj.Message = dt.Rows[0]["Message"].ToString();
                if (obj.Code == "200")
                    {
                    //  obj.SubscriberID = (Int32)dt.Rows[0]["SubscriberID"];
                    //    obj.MemberSubscription = (Int32)dt.Rows[0]["MemberSubscription"];
                    //    HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                    //   HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                    }

                }

            return obj;
            }

        //To Insert Subscriber Master data
        public Subscriber SubmitSubscriberMaster(Subscriber _Subscriber)
            {
            Subscriber obj = new Subscriber();
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            var Case = 3;
            HttpContext.Current.Session["SubscriptionLimits"] = _Subscriber.MemberSubscription;
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@Case", Case);
            //Adapter.AddParam(pcol, "@SubscriberName", _Subscriber.SubscriberName);
            //Adapter.AddParam(pcol, "@EmailID", _Subscriber.EmailID);
            //Adapter.AddParam(pcol, "@MobileNo", _Subscriber.MobileNo);
            ////Adapter.AddParam(pcol, "@MobileOTP", _Subscriber.MobileOTP);
            ////Adapter.AddParam(pcol, "@EmailOTP", _Subscriber.EmailOTP);
            //Adapter.AddParam(pcol, "@RoleId", _Subscriber.RoleId);
            Adapter.AddParam(pcol, "@MemberSubscription", _Subscriber.MemberSubscription);
            Adapter.AddParam(pcol, "@Active", _Subscriber.Active);
            Adapter.AddParam(pcol, "@CreatedBy", CreatedBy);
            //   Adapter.ExecutenNonQuery("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            dt = Adapter.ExecuteDataTable("USPSubscriberInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            if (dt.Rows.Count > 0)
                {
                obj.Code = dt.Rows[0]["Code"].ToString();
                obj.Message = dt.Rows[0]["Message"].ToString();
                if (obj.Code == "200")
                    {
                    obj.SubscriberID = (Int32)dt.Rows[0]["SubscriberID"];
                    obj.MemberSubscription = (Int32)dt.Rows[0]["MemberSubscription"];
                    HttpContext.Current.Session["SubscriberID"] = dt.Rows[0]["SubscriberID"];
                    HttpContext.Current.Session["MemberSubscription"] = dt.Rows[0]["MemberSubscription"];
                    }

                }

            return obj;
            }
        // To Bind Role Master DropdownList
        public List<Role> BindRole()
            {
            List<Role> RoleMasterList = null;

            DataTable dt = new DataTable();

            //SqlParameterCollection pcol = new SqlCommand().Parameters;

            //Adapter.ExecuteScalar("USPGetMTypeMaster", CommandType.StoredProcedure, Adapter.param(pcol));

            SqlCommand com = new SqlCommand("USPGetRoleMaster", Adapter.Connection);
            //  com.Parameters.AddWithValue("@ParentTypeId", 1);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            RoleMasterList = new List<Role>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Role role = new Role();
                role.RoleId = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"].ToString());
                role.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                RoleMasterList.Add(role);
                }

            return RoleMasterList;

            }

        //To Display List of Member Master
        public List<Subscriber> SelectallSubscriber()
            {
            var CreatedBy = HttpContext.Current.Session["UserID"];

            List<Subscriber> Subscriberlist = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllSubscriber", Adapter.Connection);

            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();

            da.Fill(ds);
            Subscriberlist = new List<Subscriber>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Subscriber cobj = new Subscriber();
                cobj.SubscriberID = Convert.ToInt32(ds.Tables[0].Rows[i]["SubscriberID"].ToString());
                cobj.SubscriberName = ds.Tables[0].Rows[i]["SubscriberName"].ToString();
                cobj.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                cobj.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                cobj.MobileOTP = ds.Tables[0].Rows[i]["MobileOTP"].ToString();
                cobj.EmailOTP = ds.Tables[0].Rows[i]["EmailOTP"].ToString();
                // cobj.MemberSubscription = Convert.ToInt32(ds.Tables[0].Rows[i]["MemberSubscription"].ToString());                         
                Subscriberlist.Add(cobj);
                }

            return Subscriberlist;
            }

        }
    }
