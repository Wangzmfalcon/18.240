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
using SMS.DBUtility;



public partial class PIF_Data_Review : System.Web.UI.Page
{
    public string staffid = "";
    public string username = "";
    public string MarS = "";
    public string MarD = "";

    public string Macauadd = "";
    public string Macauphone = "";
    public string Macauexention = "";
    public string Macauprefer = "";


    public string HomeCountry = "";
    public string Homeadd = "";
    public string Homephone = "";
    public string Homeexention = "";
    public string Homeprefer = "";
    string sqlstr = "Server=192.168.101.114;uid=falcon;pwd=airmacau;database=CSD";
    //ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        staffid = Request.QueryString["staffid"];


        //stafflist
        string SQLSTAFFLIST = "SELECT *  FROM [CSD].[dbo].[HR_PIF_StaffList]   where EMPLID='" + staffid + "' ";
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlstr, CommandType.Text, SQLSTAFFLIST))
        {
            if (rdr.Read())
            {

                username = Convert.ToString(rdr.GetSqlValue(1));
                if (Convert.ToString(rdr.GetSqlValue(2)).ToString() == "S")
                {
                    MarS = "Single";
                }
                else
                {
                    MarS = "Married";
                }
                if (rdr.GetSqlValue(3) == DBNull.Value)
                    MarD = "";
                else
                    MarD = Convert.ToDateTime(rdr.GetSqlValue(3)).ToShortDateString();
            }

        }

        //macau phone
        string SQLmacauphone = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Phone]   where EMPLID='" + staffid + "' and PHONE_TYPE='HOME' ";
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlstr, CommandType.Text, SQLmacauphone))
        {
            if (rdr.Read())
            {
                if (Convert.ToString(rdr.GetSqlValue(2)) == "Null")
                    Macauphone = "";
                else
                    Macauphone = Convert.ToString(rdr.GetSqlValue(2));


                if (Convert.ToString(rdr.GetSqlValue(3)) == "Null")
                    Macauexention = "";
                else
                    Macauexention = Convert.ToString(rdr.GetSqlValue(3));


                if (Convert.ToString(rdr.GetSqlValue(4)) == "Y")
                    prefermacau.Checked = true;


            }

        }


        //home phone
        string SQLhomephone = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Phone]   where EMPLID='" + staffid + "' and PHONE_TYPE='OTR' ";
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlstr, CommandType.Text, SQLhomephone))
        {
            if (rdr.Read())
            {
                if (Convert.ToString(rdr.GetSqlValue(2)) == "Null")
                    Homephone = "";
                else
                    Homephone = Convert.ToString(rdr.GetSqlValue(2));


                if (Convert.ToString(rdr.GetSqlValue(3)) == "Null")
                    Homeexention = "";
                else
                    Homeexention = Convert.ToString(rdr.GetSqlValue(3));

                if (Convert.ToString(rdr.GetSqlValue(4)) == "Y")
                    preferhombase.Checked = true;


            }

        }


        //macau address
        string SQLmacauadd = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Address]  where EMPLID='" + staffid + "' and KEYPROP_ADDRESS_TYPE='HOME' ";
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlstr, CommandType.Text, SQLmacauadd))
        {
            if (rdr.Read())
            {

                Macauadd = Convert.ToString(rdr.GetSqlValue(4)) + " "
                    + Convert.ToString(rdr.GetSqlValue(5)) + " "
                     + Convert.ToString(rdr.GetSqlValue(6)) + " "
                      + Convert.ToString(rdr.GetSqlValue(7));
            }

        }




        string SQLhomeadd = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Address]  where EMPLID='" + staffid + "' and KEYPROP_ADDRESS_TYPE='OTH' ";
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlstr, CommandType.Text, SQLhomeadd))
        {
            if (rdr.Read())
            {

                Homeadd = Convert.ToString(rdr.GetSqlValue(4)) + " "
                    + Convert.ToString(rdr.GetSqlValue(5)) + " "
                     + Convert.ToString(rdr.GetSqlValue(6)) + " "
                      + Convert.ToString(rdr.GetSqlValue(7));

                HomeCountry = gettrans(Convert.ToString(rdr.GetSqlValue(3)), "COUNTRY").ToString();
            }

        }




        //家庭信息
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Dependent]   where EMPLID='" + staffid + "'   and  GC_DEP_EMG_CONTACT is null";


                sqlcmm.CommandText = SQL;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt1 = new DataTable();
                adapter.Fill(dt1);
                this.family.DataSource = dt1;
                this.family.DataBind();

            }
        }


        //EM信息
        using (SqlConnection sqlcnn = new SqlConnection(sqlstr))
        {
            using (SqlCommand sqlcmm = sqlcnn.CreateCommand())
            {
                string SQL1 = "SELECT *  FROM [CSD].[dbo].[HR_PIF_Dependent]   where EMPLID='" + staffid + "'   and  GC_DEP_EMG_CONTACT ='Y'";


                sqlcmm.CommandText = SQL1;
                sqlcnn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcmm);
                DataTable dt2 = new DataTable();
                adapter.Fill(dt2);
                this.Emergency.DataSource = dt2;
                this.Emergency.DataBind();

            }
        }





    }

    public string Emergencystyle(int i)
    {
        string ret_s = "";
        if (i == 1)
            ret_s = "<h3>2.Emergency contact</h3>";
        else
            ret_s = "<h3>Emergency contact" + i + "</h3>";

        ret_s += " <div class=\"form-basic\" name=\"emerform\">                                                                                                                               " +
"                           <div class=\"row\">                                                                                                                              " +
"                               <div class=\" form-group inline-group\">                                                                                                     " +
"                                   <label for=\"family-relationship\">Relationship:</label>                                                                                 " +
"                                       <label>" + gettrans(Convert.ToString(Eval("RELATIONSHIP")), "RELATION").ToString() + "<label>                                                                                                                                               " +
"                               </div>                                                                                                                                       " +
"                               <div class=\" form-group inline-group\">                                                                                                     " +
"                                   <label for=\"familyNameType\"><span>*</span>NameType:</label>                                                                            ";

        if (Eval("COUNTRY_NM_FORMAT").ToString() == "CHN")
            ret_s += "        <label> Chinese  </label>                                      ";
        else
            ret_s += "       <label> English  </label>                                            ";

        ret_s += "                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                           <div class=\"row\">                                                                                                                              " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"\"><span>*</span>Last name:</label>                                                                                         " +
"                                    <label> " + Eval("LAST_NAME") + "  </label>                                               " +
"                               </div>                                                                                                                                       " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"\"><span>*</span>First name:</label>                                                                                        " +
"                                       <label> " + Eval("FIRST_NAME") + "  </label>                                             " +
"                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                           <div class=\"row\">                                                                                                                              " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"\"><span>*</span>Sex:</label>                                                                                               ";

        if (Eval("SEX").ToString() == "M")
            ret_s += "        <label> Male  </label>                                      ";
        else
            ret_s += "       <label> Female  </label>                                            ";


        ret_s += "                               </div>                                                                                                                                       " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"occupation\"><span>*</span>Marital Status:</label>                                                                          ";
        if (Eval("MAR_STATUS").ToString() == "S")
            ret_s += "        <label> Single  </label>                                      ";
        else
            ret_s += "       <label> Married  </label>                                            ";
        ret_s += "                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                           <div class=\"row\">                                                                                                                              " +
"                                                                                                                                                                            " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"\">Country</label>                                                                                                          " +
"                                         <label  > " + gettrans(Convert.ToString(Eval("COUNTRY")), "COUNTRY").ToString() + " </label>                                                                                                                                       " +
"                               </div>                                                                                                                                       " +
"                               <div class=\"form-group inline-group\">                                                                                                      " +
"                                   <label for=\"City\">City</label>                                                                                                         " +
"                               <label  > " + Eval("CITY") + " </label>                                                                        " +
"                                                                                                                                                                            " +
"                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                           <div class=\"row\">                                                                                                                              " +
"                               <div class=\"form-group\">                                                                                                                   " +
"                                   <label for=\"phoneinMacau\"><span>*</span>Phone:</label>                                                                                 " +
"                                     <label  > " + Eval("PHONE") + " </label>                                                              " +
"                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                           <div class=\"row\">                                                                                                                              " +
"                               <div class=\"form-group\">                                                                                                                   " +
"                                   <label for=\"home in Macau\">Address:</label>                                                                                            " +
"                                   <textarea name=\"EmergencyAddress\" id=\"\"   disabled=\"disabled\"  cols=\"55\" rows=\"4\" wrap=\"hard\" style=\"margin-left: 0\">" +  
   Eval("ADDRESS1") + " " + Eval("ADDRESS2") + " " + Eval("ADDRESS3") + " " + Eval("ADDRESS4") + " " + "</textarea>                    " +
"                               </div>                                                                                                                                       " +
"                           </div>                                                                                                                                           " +
"                       </div>                                                                                                                                               " +
"                 </div>                                                                                                                                                      ";



        return ret_s;
    }

    public string familystyle(int i)
    {
        string ret_s = "";
        if (i == 1)
            ret_s = "<h3>2.Family contact</h3>";
        else
            ret_s = "<h3>Family contact" + i + "</h3>";

        ret_s += "                        <div class=\"form-basic\">                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                <div class=\" form-group inline-group\">                                                                                    " +
"                                    <label for=\"family-relationship\"><span>*</span>Relationship:</label>                                                  " +
"                             <label>" + gettrans(Convert.ToString(Eval("RELATIONSHIP")), "FAMILY").ToString() + "<label>                                                                                                           " +
"                                                                                                                                                            " +
"                                </div>                                                                                                                      " +
"                                <div class=\" form-group inline-group\">                                                                                    " +
"                                    <label><span>*</span>NameType:</label>                                                                                  ";
        if (Eval("COUNTRY_NM_FORMAT").ToString() == "CHN")
            ret_s += "        <label> Chinese  </label>                                      ";
        else
            ret_s += "       <label> English  </label>                                            ";

        ret_s += "                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"\"><span>*</span>Last name:</label>                                                                        " +
"                                     <label> " + Eval("LAST_NAME") + "  </label>                        " +
"                                </div>                                                                                                                      " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"\"><span>*</span>First name:</label>                                                                       " +
"                                    <label> " + Eval("FIRST_NAME") + "  </label>                             " +
"                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"\"><span>*</span>Sex:</label>                                                                              ";

        if (Eval("SEX").ToString() == "M")
      ret_s += "        <label> Male  </label>                                      ";
        else
      ret_s += "       <label> Female  </label>                                            ";

  ret_s += "                                </div>                                                                                                                      " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"dateofbirth\"><span>*</span>Date Of Birth:</label>                                                         " +
"                                    <label> " + Convert.ToDateTime(Eval("BIRTHDATE")).ToShortDateString() + "  </label>                  " +
"                                </div>                                                                                                                      " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"occupation\">Occupation:</label>                                                                           " +
"                                    <label> " + Eval("GC_DEP_COMMENT") + "  </label>                                                      " +
"                                </div>                                                                                                                      " +
"                                                                                                                                                            " +
"                                  <div class=\"form-group inline-group\">                                                                                   " +
"                                    <label for=\"occupation\"><span>*</span>Marital Status:</label>                                                         ";
  if (Eval("MAR_STATUS").ToString() == "S")
      ret_s += "        <label> Single  </label>                                      ";
  else
      ret_s += "       <label> Married  </label>                                            ";

  ret_s += "                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                                                                                                                                            " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"\">Country</label>                                                                                         " +
"                             <label  > " + gettrans(Convert.ToString(Eval("COUNTRY")), "COUNTRY").ToString()+ " </label>                                                                                                             " +
"                                </div>                                                                                                                      " +
"                                <div class=\"form-group inline-group\">                                                                                     " +
"                                    <label for=\"City\">City</label>                                                                                        " +
"                                   <label  > " + Eval("CITY") + " </label>                                                          " + 
"                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                <div class=\"form-group\">                                                                                                  " +
"                                    <label for=\"phoneinMacau\">Phone:</label>                                                                              " +
"                                    <label  > " + Eval("PHONE") + " </label>                                              " +
"                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                            <div class=\"row\">                                                                                                             " +
"                                <div class=\"form-group\">                                                                                                  " +
"                                    <label for=\"home in Macau\">Address:</label>                                                                           " +
"                                    <textarea name=\"FamilyAddress\" id=\"\" cols=\"55\"  disabled=\"disabled\" rows=\"4\" wrap=\"hard\" style=\"margin-left: 0\">"+
"                                  " + Eval("ADDRESS1") + " " + Eval("ADDRESS2") + " " + Eval("ADDRESS3") + " " + Eval("ADDRESS4") + " " + "  </textarea>      " +
"                                </div>                                                                                                                      " +
"                            </div>                                                                                                                          " +
"                        </div>                                                                                                                              " +
"                                                                                                                                                            " +
"                    </div>                                                                                                                                  ";
        return ret_s;
    }


    public object gettrans(string fieldval, string fieldbname)
    {



        //检查数据是否已经存在
        string SQL = "select XLATLONGNAME from HR_PIF_TranslationTable WHERE FIELDNAME='" + fieldbname + "'   and FIELDVALUE ='" + fieldval + "'";
        SqlConnection sqlcnn = new SqlConnection(sqlstr);
        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlcnn, CommandType.Text, SQL))
        {

            if (rdr.Read())
            {
                return Convert.ToString(rdr.GetSqlValue(0));
            }
            else
                return DBNull.Value;

        }


    }

}