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
     public class GroupDAL
        {
        public List<Group> GetAllGroupList()
            {
            List<Group> GetAllGroupList = new List<Group>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetAllGroupList", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Group cobj = new Group();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                GetAllGroupList.Add(new Group()
                    {
                    Group_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["GroupID"].ToString()),
                    Group_Code = ds.Tables[0].Rows[i]["group_code"].ToString(),
                    Group_Name = ds.Tables[0].Rows[i]["Group_Name"].ToString(),
                    UGroupName = ds.Tables[0].Rows[i]["UGroupName"].ToString(),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString()),
                    CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()),
                    });
                }
            return GetAllGroupList;
            }
        public void InsertGroupMaster(Group _Group, int Flag)
            {
            DataTable dt = new DataTable();
            var CreatedBy = HttpContext.Current.Session["UserID"];
            SqlParameterCollection pcol = new SqlCommand().Parameters;
            Adapter.AddParam(pcol, "@GrpCode", _Group.Group_Code);
            Adapter.AddParam(pcol, "@Group_Name", _Group.Group_Name);
            Adapter.AddParam(pcol, "@Under_Group", _Group.UGroupName);
            Adapter.AddParam(pcol, "@Active", _Group.Active);
            Adapter.AddParam(pcol, "@CreatedBy", _Group.CreatedBy);
            Adapter.AddParam(pcol, "@Flag", Flag);
            Adapter.ExecutenNonQuery("USPGroupInsertUpdate", CommandType.StoredProcedure, Adapter.param(pcol));
            }
        public List<Group> GroupList()
            {

            List<Group> UnderGroupList = new List<Group>();
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("USPGetGroupListMaster", Adapter.Connection);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Group cobj = new Group();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                UnderGroupList.Add(new Group()
                    {
                    UGroupID = Convert.ToInt32(ds.Tables[0].Rows[i]["UGroupID"].ToString()),
                    UGroupName = ds.Tables[0].Rows[i]["UGroupName"].ToString()

                    });
                }
            return UnderGroupList;
            }
        public List<Group> DublicateGroupListCheck()
            {
            string count = "";
            List<Group> GroupList = null;
            DataTable dt = new DataTable();
            SqlCommand com = new SqlCommand("DublicateGroupListCheck", Adapter.Connection);
            // com.Parameters.Add(new SqlParameter("@Name", _Group.Group_Name));
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            GroupList = new List<Group>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                Group cobj = new Group();
                cobj.Group_Name = ds.Tables[0].Rows[i]["Group_Name"].ToString();
                cobj.Group_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["GroupID"].ToString());
                cobj.Active = Convert.ToBoolean(ds.Tables[0].Rows[i]["Active"].ToString());
                cobj.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString());
                GroupList.Add(cobj);
                }
            return GroupList;
            }
        }
    }
