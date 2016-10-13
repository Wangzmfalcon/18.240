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
//using System.Xml.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Globalization;//日期格式化

public partial class PIF_Date_Input : System.Web.UI.Page
{

    string sqlstr = "Server=192.168.101.114;uid=falcon;pwd=airmacau;database=CSD";
        //ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        //国籍
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "SELECT XLATLONGNAME  FROM [CSD].[dbo].[HR_PIF_TranslationTable]   where [FIELDNAME]='COUNTRY' ";
               

                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                this.countoption.DataSource = dt;
                this.countoption.DataBind();
                this.countoption1.DataSource = dt;
                this.countoption1.DataBind();
                this.countoption2.DataSource = dt;
                this.countoption2.DataBind();
            }
        }


        //家庭关系
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "SELECT XLATLONGNAME  FROM [CSD].[dbo].[HR_PIF_TranslationTable]   where [FIELDNAME]='FAMILY' ";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt1 = new DataTable();
                adapter.Fill(dt1);
                this.familyop.DataSource = dt1;
                this.familyop.DataBind();

            }
        }


        //联系人关系
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "SELECT XLATLONGNAME  FROM [CSD].[dbo].[HR_PIF_TranslationTable]   where [FIELDNAME]='RELATION' ";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt1 = new DataTable();
                adapter.Fill(dt1);
                this.relationop.DataSource = dt1;
                this.relationop.DataBind();

            }
        }
        //address
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "SELECT XLATLONGNAME  FROM [CSD].[dbo].[HR_PIF_TranslationTable]   where [FIELDNAME]='ADDRESS' ";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt2 = new DataTable();
                adapter.Fill(dt2);
                //this.addressopt.DataSource = dt2;
                //this.addressopt.DataBind();
                //this.addressopt1.DataSource = dt2;
                //this.addressopt1.DataBind();

            }
        }

    }


    public string countoptionstyle(int i)
    {
        string ret_s = "";

        ret_s = "<option value=\"" + Eval("XLATLONGNAME") + "\">" + Eval("XLATLONGNAME") + "</option>";
        return ret_s;
    }


    public string addressoptstyle(int i)
    {
        string ret_s = "";

        ret_s = "<option value=\"" + Eval("XLATLONGNAME") + "\">" + Eval("XLATLONGNAME") + "</option>";
        return ret_s;
    }



    public string familyoptionstyle(int i)
    {
        string ret_s = "";

        ret_s = "<option value=\"" + Eval("XLATLONGNAME") + "\">" + Eval("XLATLONGNAME") + "</option>";
        return ret_s;
    }


    public string relationoptionstyle(int i)
    {
        string ret_s = "";

        ret_s = "<option value=\"" + Eval("XLATLONGNAME") + "\">" + Eval("XLATLONGNAME") + "</option>";
        return ret_s;
    }

}