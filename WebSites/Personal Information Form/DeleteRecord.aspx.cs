using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;
using System.Drawing;
using System.IO;
using System.Globalization;//日期格式化


public partial class DeleteRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string StaffID = Request.QueryString["StaffID"];

        StaffID = StaffID.Substring(8, StaffID.Length-8);
        string sqlstr = "Server=192.168.101.114;uid=falcon;pwd=airmacau;database=CSD";//ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "update HR_PIF_StaffList SET STATUS=1 WHERE EMPLID='" + StaffID + "' ";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                int d = sqlcmm.ExecuteNonQuery();
                if (d > 0)   
                    Response.Write("Y");   
                 
                  //  ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Y');</script>");
                else
                    Response.Write("N");
                  //  ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('N');</script>");


            }
        }
    }
}