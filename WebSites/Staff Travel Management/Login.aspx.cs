using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMS.DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.IO;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["STMUserID"] = null;
        Session["STMUserName"] = null;
    }



    public void Write(string data)
    {

        //获得字节数组
        StreamWriter sw = File.AppendText(Server.MapPath(Request.ApplicationPath) + "/system.log");
        //string  data = "Hello World!";
        //开始写入
        sw.WriteLine(data);

        //清空缓冲区、关闭流
        sw.Flush();
        sw.Close();


    }


    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string uid = Request.Form["username"];
        string pwd = Request.Form["password"];

        string Loginstr = "select a.* from STM_Login a where LoginName ='" + uid + "' and Password = '" + pwd + "'";

        //检查账号密码
        using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, Loginstr))
        {
            if (rdr.Read())
            {
                Session["STMUserID"] = Convert.ToString(rdr.GetSqlValue(0));
                Session["STMUserName"] = Convert.ToString(rdr.GetSqlValue(1));
                //进入系统
                Server.Transfer("Home.aspx");
              
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('You have no right to sign the system, please contact the admin .');</script>");

            }

        }


    }
}