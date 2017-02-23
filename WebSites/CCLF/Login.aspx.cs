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
        Session["CCLF_Manage_Code"] = null;

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

        string pwd = Request.Form["password"];
        if (pwd == "83966999")
        {
            //进入系统
            Session["CCLF_Manage_Code"] = pwd;
            Server.Transfer("Home.aspx");
         
        }
        else
        {
            ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('Login failed! Please check your UserName and Password.');</script>");

        }

    }



}