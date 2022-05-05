using IrecordDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using IRecordweb.Models;
using System.Web;
using System.Net.Http;
using System.Web.SessionState;

namespace DAL
    {
    public class MenuData
        {     
        public static IList<Menu> GetMenus(string UserId)
            {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IrecordwebConnection"].ToString()))
                {
                con.Open();
                List<Menu> menuList = new List<Menu>();
               
                SqlParameterCollection pcol = new SqlCommand().Parameters;
                Adapter.AddParam(pcol, "@UserId", UserId);
                SqlDataReader sdr;
                sdr= Adapter.ExecuteReader("uspUserMenuTrans", CommandType.StoredProcedure, Adapter.param(pcol));

                while (sdr.Read())
                    {
                    Menu menu = new Menu();
                  
                    menu.MenuID = Convert.ToInt32(sdr["MenuID"].ToString());
                    menu.Name = sdr["Name"].ToString();
                    menu.url = sdr["url"].ToString();
                    menu.ParentMenuID = sdr["ParentMenuID"] != DBNull.Value? Convert.ToInt32(sdr["ParentMenuID"]):(int?) null;
                    menuList.Add(menu);
                        }
                    
                return menuList;
                }

            }
        }
    }