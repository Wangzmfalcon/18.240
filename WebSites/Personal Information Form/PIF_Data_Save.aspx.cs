using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
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
using System.Text;



public partial class PIF_Data_Save : System.Web.UI.Page
{


    string sqlstr = "Server=192.168.101.114;uid=falcon;pwd=airmacau;database=CSD";
    protected void Page_Load(object sender, EventArgs e)
    {
        string idflag = "";
        string jsonText = Request.Form["inputdate1"].ToString();
        string[] stringSeparators = new string[] { "||" };
        string[] sArray = jsonText.Split(stringSeparators, StringSplitOptions.None);

        string Errorcode = "";
        string staffid = "";
        int dependent_id = 1;
        bool Erroroc = false;
        //分form数据
        for (int i = 0; i < sArray.Length; i++)
        {


            if (sArray[i] == "")
            {

            }
            else
            {

                //分键值对
                string[] stringKey = new string[] { "#$#" };
                string[] Keypair = sArray[i].Split(stringKey, StringSplitOptions.None);
                //分类用于数据存储
                int typeflag = 0;
                List<Keyp> keylist = new List<Keyp>();

                for (int j = 0; j < Keypair.Length; j++)
                {
                    if (Keypair[j] != "")
                    {
                        //取键值对的 名和值
                        string[] SepareteNameandVal = new string[] { "^^" };
                        string[] NameVal = Keypair[j].Split(SepareteNameandVal, StringSplitOptions.None);
                        Keyp keyNV = new Keyp(NameVal[0].ToString().Trim(), NameVal[1].ToString().Trim());

                        //获得staffid
                        if (NameVal[0].ToString().Trim() == "UserStaffNumber")
                            staffid = NameVal[1].ToString().Trim();
                        keylist.Add(keyNV);
                        if (j == 0)
                        {
                            //判断form类型
                            switch (NameVal[0].ToString().Trim())
                            {

                                case "UserName":
                                    typeflag = 1;//个人信息
                                    break;
                                case "Familyrelationship"://家属
                                    typeflag = 2;
                                    break;
                                case "Emergencyrelationship"://紧急
                                    typeflag = 3;
                                    break;
                                default:
                                    typeflag = 0;
                                    break;

                            }

                        }
                    }
                }







                //根据类型存数据

                switch (typeflag)
                {
                    case 0:




                        break;
                    case 1:

                        //检查数据是否已经存在
                        string SQL = "select 'X' from HR_PIF_StaffList WHERE EMPLID='" + staffid + "'  ";
                        SqlConnection sqlcnn = new SqlConnection(sqlstr);
                        using (SqlDataReader rdr = SqlHelper1.ExecuteReader(sqlcnn, CommandType.Text, SQL))
                        {

                            if (rdr.Read())
                            {
                                idflag = Convert.ToString(rdr.GetSqlValue(0));
                            }
                        }
                        if (idflag == "X" || Erroroc)
                        {
                            Errorcode = "The Staff ID is already exist";//有重复数据


                            break;
                        }


                        //插入人员信息
                        using (SqlConnection sqlcnn1 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn1.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_StaffList values(@emplid,@name,@mar_s,@mar_d,@dl,@st) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", getval(keylist, "UserStaffNumber"));
                                sqlcmm.Parameters.AddWithValue("@name", getval(keylist, "UserName"));

                                if (getval(keylist, "MaritalStatus").ToString() == "Single")
                                    sqlcmm.Parameters.AddWithValue("@mar_s", "S");

                                else
                                    sqlcmm.Parameters.AddWithValue("@mar_s", "M");



                                sqlcmm.Parameters.AddWithValue("@mar_d", getval(keylist, "MaritalDate"));
                                sqlcmm.Parameters.AddWithValue("@dl", 0);
                                sqlcmm.Parameters.AddWithValue("@st", 0);
                                sqlcnn1.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Staff List failure.";
                                    Erroroc = true;
                                }
                                 

                            }
                        }

                        //插入Macau 电话信息
                        using (SqlConnection sqlcnn2 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn2.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Phone values(@emplid,@PHONE_TYPE,@PHONE,@EXTENSION,@PREF_PHONE_FLAG) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                sqlcmm.Parameters.AddWithValue("@PHONE_TYPE", "HOME");
                                sqlcmm.Parameters.AddWithValue("@PHONE", getval(keylist, "PhoneinMacau"));
                                if (getval(keylist, "MacauExtention") == DBNull.Value)
                                    sqlcmm.Parameters.AddWithValue("@EXTENSION", DBNull.Value);
                                else
                                sqlcmm.Parameters.AddWithValue("@EXTENSION", getval(keylist, "MacauExtention").ToString().Substring(0,6));

                                if (getval(keylist, "MAddressprefered").ToString() == "on")
                                    sqlcmm.Parameters.AddWithValue("@PREF_PHONE_FLAG", "Y");

                                else
                                    sqlcmm.Parameters.AddWithValue("@PREF_PHONE_FLAG", "N");


                                sqlcnn2.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Macau Phone failure.";
                                    Erroroc = true;
                                }
                                

                            }
                        }

                        //插入HOME 电话信息
                        using (SqlConnection sqlcnn2 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn2.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Phone values(@emplid,@PHONE_TYPE,@PHONE,@EXTENSION,@PREF_PHONE_FLAG) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                sqlcmm.Parameters.AddWithValue("@PHONE_TYPE", "OTR");
                                sqlcmm.Parameters.AddWithValue("@PHONE", getval(keylist, "PhoneinHomebase"));
                                if (getval(keylist, "HomebaseExtention") == DBNull.Value)
                                    sqlcmm.Parameters.AddWithValue("@EXTENSION", DBNull.Value);
                                else
                                sqlcmm.Parameters.AddWithValue("@EXTENSION", getval(keylist, "HomebaseExtention").ToString().Substring(0, 6));

                                if (getval(keylist, "HAddressprefered").ToString() == "on")
                                    sqlcmm.Parameters.AddWithValue("@PREF_PHONE_FLAG", "Y");

                                else
                                    sqlcmm.Parameters.AddWithValue("@PREF_PHONE_FLAG", "N");


                                sqlcnn2.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Macau Phone failure.";
                                    Erroroc = true;
                                }
                                    

                            }
                        }


                        //导入Macau地址信息
                        using (SqlConnection sqlcnn3 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn3.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Address values(@emplid,@KEYPROP_ADDRESS_TYPE,@KEYPROP_EFFDT,@COUNTRY,@ADDRESS1,@ADDRESS2,@ADDRESS3,@ADDRESS4) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                sqlcmm.Parameters.AddWithValue("@KEYPROP_ADDRESS_TYPE", "HOME");
                                sqlcmm.Parameters.AddWithValue("@KEYPROP_EFFDT", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@COUNTRY", "CHN");
                               
                                string[] result = addressslip(getval(keylist, "AddressinMacau").ToString());
                                sqlcmm.Parameters.AddWithValue("@ADDRESS1", result[0]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS2", result[1]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS3", result[2]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS4", result[3]);




                                sqlcnn3.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Macau Phone failure.";
                                    Erroroc = true;
                                }
                              

                            }
                        }


                        //导入Macau地址信息
                        using (SqlConnection sqlcnn3 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn3.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Address values(@emplid,@KEYPROP_ADDRESS_TYPE,@KEYPROP_EFFDT,@COUNTRY,@ADDRESS1,@ADDRESS2,@ADDRESS3,@ADDRESS4) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                sqlcmm.Parameters.AddWithValue("@KEYPROP_ADDRESS_TYPE", "OTH");
                                sqlcmm.Parameters.AddWithValue("@KEYPROP_EFFDT", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@COUNTRY", gettrans(getval(keylist, "CountryinHomebase").ToString(), "COUNTRY").ToString());


                                string[] result = addressslip(getval(keylist, "AddressinHomebase").ToString());
                                sqlcmm.Parameters.AddWithValue("@ADDRESS1", result[0]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS2", result[1]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS3", result[2]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS4", result[3]);




                                sqlcnn3.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Macau Phone failure.";
                                    Erroroc = true;
                                }
                                    

                            }
                        }


                        break;
                    case 2:
                        if (idflag == "X"||  Erroroc )
                        {

                            break;
                        }
                        //家庭信息
                        using (SqlConnection sqlcnn3 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn3.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Dependent "
                                    + " ([EMPLID],[DEPENDENT_BENEF],[BIRTHDATE],[PHONE],[EFFDT],[COUNTRY_NM_FORMAT],[LAST_NAME],[FIRST_NAME],[NAME_PREFIX],[EFFDT_1],[COUNTRY],"
                                    + " [ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[CITY],[EFFDT_3],[RELATIONSHIP],[MAR_STATUS],[SEX],[GC_DEP_COMMENT],[GC_DEP_PRMCONTACT])"
                                    + " values(@emplid,@DEPENDENT_BENEF,@BIRTHDATE,@PHONE,@EFFDT,@COUNTRY_NM_FORMAT,@LAST_NAME,@FIRST_NAME,@NAME_PREFIX,@EFFDT_1,@COUNTRY"
                                    + "  ,@ADDRESS1,@ADDRESS2,@ADDRESS3,@ADDRESS4,@CITY,@EFFDT_3,@RELATIONSHIP,@MAR_STATUS,@SEX,@GC_DEP_COMMENT,@GC_DEP_PRMCONTACT) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                string did = "";
                                if (dependent_id < 10)
                                {
                                     did = "0" + dependent_id.ToString();
                                }
                                else
                                     did =  dependent_id.ToString();

                                sqlcmm.Parameters.AddWithValue("@DEPENDENT_BENEF", did);
                                sqlcmm.Parameters.AddWithValue("@BIRTHDATE", getval(keylist, "FamilyDateofBirth"));
                                sqlcmm.Parameters.AddWithValue("@PHONE", getval(keylist, "FamilyPhone"));
                                sqlcmm.Parameters.AddWithValue("@EFFDT", DateTime.Now.ToShortDateString());
                                if (getval(keylist, "FamilyNameType").ToString()=="Chinese")
                                    sqlcmm.Parameters.AddWithValue("@COUNTRY_NM_FORMAT", "CHN");
                                else
                                    sqlcmm.Parameters.AddWithValue("@COUNTRY_NM_FORMAT", "ENG");
                                sqlcmm.Parameters.AddWithValue("@LAST_NAME", getval(keylist, "RelationshipLastname"));
                                sqlcmm.Parameters.AddWithValue("@FIRST_NAME", getval(keylist, "RelationshipFirstname"));
                                if (getval(keylist, "FamilySex").ToString() == "Male")
                                {
                                    sqlcmm.Parameters.AddWithValue("@NAME_PREFIX", "Mr");
                                    sqlcmm.Parameters.AddWithValue("@SEX", "M");
                                }
                                else
                                {
                                    sqlcmm.Parameters.AddWithValue("@NAME_PREFIX", "Ms");
                                    sqlcmm.Parameters.AddWithValue("@SEX", "F");
                                }
                                sqlcmm.Parameters.AddWithValue("@EFFDT_1", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@COUNTRY", gettrans(getval(keylist, "FamilyCountry").ToString(), "COUNTRY").ToString());
                                string[] result = addressslip(getval(keylist, "FamilyAddress").ToString());
                                sqlcmm.Parameters.AddWithValue("@ADDRESS1", result[0]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS2", result[1]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS3", result[2]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS4", result[3]);
                                sqlcmm.Parameters.AddWithValue("@CITY", getval(keylist, "FamilyCity").ToString());
                                sqlcmm.Parameters.AddWithValue("@EFFDT_3", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@RELATIONSHIP", gettrans(getval(keylist, "Familyrelationship").ToString(), "FAMILY").ToString());

                                if (getval(keylist, "FMaritalStatus").ToString() == "Single")
                                    sqlcmm.Parameters.AddWithValue("@MAR_STATUS", "S");

                                else
                                    sqlcmm.Parameters.AddWithValue("@MAR_STATUS", "M");



                                sqlcmm.Parameters.AddWithValue("@GC_DEP_COMMENT", getval(keylist, "FamilyOccupation"));
                                if (dependent_id==0)
                                    sqlcmm.Parameters.AddWithValue("@GC_DEP_PRMCONTACT", "Y");
                                else
                                    sqlcmm.Parameters.AddWithValue("@GC_DEP_PRMCONTACT", "N");


                             



                                sqlcnn3.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Family " + dependent_id + " failure.";
                                    Erroroc = true;

                                }
                              

                            }
                        }

                        dependent_id++;

                        break;
                    case 3:
                        if (idflag == "X" || Erroroc)
                        {                            
                            break;
                        }

                        //紧急联系人信息
                        using (SqlConnection sqlcnn3 = new SqlConnection(sqlstr))
                        {
                            using (SqlCommand sqlcmm = sqlcnn3.CreateCommand())
                            {

                                sqlcmm.CommandText = "INSERT INTO  HR_PIF_Dependent "
                                    + " ([EMPLID],[DEPENDENT_BENEF],[PHONE],[EFFDT],[COUNTRY_NM_FORMAT],[LAST_NAME],[FIRST_NAME],[NAME_PREFIX],[EFFDT_1],[COUNTRY],"
                                    + " [ADDRESS1],[ADDRESS2],[ADDRESS3],[ADDRESS4],[CITY],[EFFDT_3],[RELATIONSHIP],[MAR_STATUS],[SEX],[GC_DEP_PRMCONTACT],[GC_DEP_EMG_CONTACT])"
                                    + " values(@emplid,@DEPENDENT_BENEF,@PHONE,@EFFDT,@COUNTRY_NM_FORMAT,@LAST_NAME,@FIRST_NAME,@NAME_PREFIX,@EFFDT_1,@COUNTRY"
                                    + "  ,@ADDRESS1,@ADDRESS2,@ADDRESS3,@ADDRESS4,@CITY,@EFFDT_3,@RELATIONSHIP,@MAR_STATUS,@SEX,@GC_DEP_PRMCONTACT,@GC_DEP_EMG_CONTACT) ";
                                sqlcmm.Parameters.AddWithValue("@emplid", staffid);
                                sqlcmm.Parameters.AddWithValue("@DEPENDENT_BENEF", dependent_id);

                                sqlcmm.Parameters.AddWithValue("@PHONE", getval(keylist, "EmergencyPhone").ToString());
                                sqlcmm.Parameters.AddWithValue("@EFFDT", DateTime.Now);
                                if (getval(keylist, "EmergencyNameType").ToString() == "Chinese")
                                    sqlcmm.Parameters.AddWithValue("@COUNTRY_NM_FORMAT", "CHN");
                                else
                                    sqlcmm.Parameters.AddWithValue("@COUNTRY_NM_FORMAT", "ENG");
                                sqlcmm.Parameters.AddWithValue("@LAST_NAME", getval(keylist, "EmergencyLastName").ToString());
                                sqlcmm.Parameters.AddWithValue("@FIRST_NAME", getval(keylist, "EmergencyFirstName").ToString());
                                if (getval(keylist, "EmergencySex").ToString() == "Male")
                                {
                                    sqlcmm.Parameters.AddWithValue("@NAME_PREFIX", "Mr");
                                    sqlcmm.Parameters.AddWithValue("@SEX", "M");
                                }
                                else
                                {
                                    sqlcmm.Parameters.AddWithValue("@NAME_PREFIX", "Ms");
                                    sqlcmm.Parameters.AddWithValue("@SEX", "F");
                                }
                                sqlcmm.Parameters.AddWithValue("@EFFDT_1", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@COUNTRY", gettrans(getval(keylist, "EmergencyCountry").ToString(), "COUNTRY").ToString());
                                string[] result = addressslip(getval(keylist, "EmergencyAddress").ToString());
                                sqlcmm.Parameters.AddWithValue("@ADDRESS1", result[0]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS2", result[1]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS3", result[2]);
                                sqlcmm.Parameters.AddWithValue("@ADDRESS4", result[3]);
                                sqlcmm.Parameters.AddWithValue("@CITY", getval(keylist, "EmergencyCity").ToString());
                                sqlcmm.Parameters.AddWithValue("@EFFDT_3", DateTime.Now);
                                sqlcmm.Parameters.AddWithValue("@RELATIONSHIP", gettrans(getval(keylist, "Emergencyrelationship").ToString(), "RELATION").ToString());

                                if (getval(keylist, "EMaritalStatus").ToString() == "Single")
                                    sqlcmm.Parameters.AddWithValue("@MAR_STATUS", "S");

                                else
                                    sqlcmm.Parameters.AddWithValue("@MAR_STATUS", "M");

                                sqlcmm.Parameters.AddWithValue("@GC_DEP_PRMCONTACT", "N");
                                sqlcmm.Parameters.AddWithValue("@GC_DEP_EMG_CONTACT", "Y");






                                sqlcnn3.Open();
                                int d = sqlcmm.ExecuteNonQuery();
                                if (d > 0)
                                    Errorcode = "Insert success.";
                                else
                                {
                                    Errorcode = "Insert Emergency" + dependent_id + " failure.";
                                    Erroroc = true;

                                }


                            }
                        }

                        dependent_id++;
                        break;
                    default:
                        if (idflag == "X" || Erroroc)
                        {
                            //  Errorcode = "The Staff ID is already exist";//有重复数据
                            break;
                        }
                        break;


                }



            }




        }


        Response.Write(Errorcode);

    }



    public string[] addressslip(string address)
    {
        string[] result = new string[5];
        result[0] = "";
        result[1] = "";
        result[2] = "";
        result[3] = "";
        result[4] = "";
        int j = 0;



        //长度 分 中英文字符，所以要编码

        string[] aArray = address.Split(' ');
        int alen = 0;
        for (int i = 0; i < aArray.Length; i++)
        {

            if (j > 3)
                break;
            if ((alen + Encoding.Default.GetBytes(aArray[i]).Length + 1) < 55)
            {
                result[j] += aArray[i] + " ";
                alen += Encoding.Default.GetBytes(aArray[i]).Length + 1;

            }
            else
            {
                result[j + 1] += aArray[i] + " ";
                alen = Encoding.Default.GetBytes(aArray[i]).Length + 1;
                j++;

            }



            //if ((alen + aArray[i].Length+1) < 55)
            //{
            //    result[0] += aArray[i] + " ";
            //    alen += aArray[i].Length+1;

            //}
            //else if ((alen + aArray[i].Length+1) < 110 && (alen + aArray[i].Length+1) >= 55)           {
            //    result[1] += aArray[i] + " ";
            //    alen += aArray[i].Length+1;

            //}
            //else if ((alen + aArray[i].Length+1) < 165 && (alen + aArray[i].Length+1) >= 110)
            //{
            //    result[2] += aArray[i] + " ";
            //    alen += aArray[i].Length+1;

            //}
            //else if ((alen + aArray[i].Length+1) < 220 && (alen + aArray[i].Length+1) >= 165)
            //{
            //    result[3] += aArray[i] + " ";
            //    alen += aArray[i].Length+1;

            //}
            //else
            //{ 
            //alen += aArray[i].Length;

            //}



        }

        return result;

    }

    public object gettrans(string fieldval, string fieldbname)
    {



        //检查数据是否已经存在
        string SQL = "select FIELDVALUE from HR_PIF_TranslationTable WHERE FIELDNAME='" + fieldbname + "'   and  XLATLONGNAME='" + fieldval + "'";
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

    public object getval(List<Keyp> kl, string keyname)
    {
        string key = "";

        foreach (Keyp k in kl)
        {
            if (k.Name == keyname)
                key = k.Value.ToString().Trim();

        }

        if (Equals(Convert.ToString(key), ""))
            return DBNull.Value;
        else
            return key;
    }


    #region Keyp
    public class Keyp
    {

        public string Name;
        public string Value;

        public Keyp(string name, string value)
        {

            this.Name = name;
            this.Value = value;
        }
    }
    #endregion Keyp

}