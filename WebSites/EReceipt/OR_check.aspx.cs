using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMS.DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class OR_check : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    
        string OR_num = Request.Form["OR"].ToString();
        string Station = Request.Form["Station"].ToString();
        string reply="";
        string offs = "";
        string SQL_balance = "select A.Balance,A.offs,A.Total_amt from ERS_Receipt A where A.Station='" + Station + "'and A.Num='" + OR_num + "'  and void=0 ";
        using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_balance))
        {
            if (rdr.Read())
            {
                reply = Convert.ToString(rdr.GetSqlValue(2));
                offs = Convert.ToString(rdr.GetSqlValue(1));
            }
            else
                reply = "can not found";
        }

        if (offs=="True" || offs=="1")
            reply = "have been used";
		//reply=offs;
        Response.Write(reply);
    }
}