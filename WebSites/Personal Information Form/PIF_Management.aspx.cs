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
using System.Collections.Generic;  
using System.IO;
using System.Globalization;//日期格式化



public partial class PIF_Management : System.Web.UI.Page
{

    string sqlstr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
        reflash();
    }



    public void reflash()
    {
        string staff = TextBox1.Text;
        if (staff == "")
        {
            staff = "A.EMPLID";
        
        }
        else
            staff = "'"+staff+"'";


        string onlytest = Request.Form["only"];
         string only="";
        if(onlytest=="on")
             only=" and A.DOWNLOAD_FLAG = 0";


        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "select A.EMPLID,A.NAME,A.DOWNLOAD_FLAG from HR_PIF_StaffList A where 1=1 AND A.STATUS=0 and A.EMPLID=" + staff +only;


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();

            }
        }

    }


    public string trstyle(int i)
    {
        string ret_s = "";
                           ret_s+= "  <tr>                                ";

                           if (Convert.ToBoolean(Eval("DOWNLOAD_FLAG").ToString()) == true)
                               ret_s += "  <td><input id=\"checkbox" + Eval("EMPLID") + "\" type=\"checkbox\" name=\"items\"  disabled=\"true\" /></td>    ";
                           else
                               ret_s += "  <td><input id=\"checkbox" + Eval("EMPLID") + "\"  type=\"checkbox\" name=\"items1\" /></td>    ";
                           ret_s += " <td><a href=\"PIF_Data_Review.aspx?staffid=" + Eval("EMPLID") + "\" target=\"_blank\">" + Eval("EMPLID") + "</td>       ";
                           ret_s += " <td>" + Eval("NAME") + "</td>                  ";
                           ret_s += "  <td><a <a class=\"btn\"     onclick=\"Delete('checkbox" + Eval("EMPLID") + "')\" >Delete</a></td>        ";
                           if (Convert.ToBoolean( Eval("DOWNLOAD_FLAG").ToString())==false)

                               ret_s += " <td><a  class=\"btndis\" disabled=\"true\">Release</a></td>        ";
                                //ret_s += " <td></td>        ";
                           else
                               ret_s += " <td><a class=\"btn\" onclick=\"release('checkbox" + Eval("EMPLID") + "')\" >Release</a></td>        ";
                     
                           ret_s+= " </tr>                                ";

        return ret_s;
    }
}