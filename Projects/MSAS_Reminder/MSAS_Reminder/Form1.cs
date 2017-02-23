using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Net;
using System.Web;
using System.Net.Mail;
using SMS.DBUtility;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;



namespace MSAS_Reminder
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        StringBuilder cctext = new StringBuilder(255);
        StringBuilder l_ex = new StringBuilder(255);
        StringBuilder a_ex = new StringBuilder(255);
        StringBuilder T_ex = new StringBuilder(255);
        string fromtext = "MSAS@airmacau.com.mo";



        public Form1()
        {

            try { sendmail(); }
            catch { }
            //sendmail();
            System.Environment.Exit(0);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendmail();
        }


        //发送通知
        public void sendmail()
        {
            string UserName = fromtext;
            string Password = "@@@@@@@";
            string MailServer = "192.168.101.100";
            int smtpPort = 25;
            System.Net.Mail.SmtpClient sc = new SmtpClient(MailServer, smtpPort);
            NetworkCredential nc = new NetworkCredential(UserName, Password);
            sc.Credentials = nc;

            MailMessage MyEmailMessage = new MailMessage();
            MyEmailMessage.From = new MailAddress(UserName);
            //获取收件人
            if (!getmailadd())
            {
                //添加收件人
                addto(MyEmailMessage);
                addcc(MyEmailMessage);
                //MyEmailMessage.To.Add(new MailAddress("xeno.zhang@airmacau.com.mo"));
                MyEmailMessage.Bcc.Add(new MailAddress("falcon.wang@airmacau.com.mo"));
                MyEmailMessage.IsBodyHtml = true;
                MyEmailMessage.BodyEncoding = Encoding.GetEncoding(936);
                MyEmailMessage.Subject = "MSAS_Reminder";

                //获得邮件内容
                MyEmailMessage.Body = getmailtext();//获得发送内容
                //MyEmailMessage.Body= "<font color='red'>邮件测试，呵呵</font>";

                MyEmailMessage.Priority = MailPriority.High;

                //sc.Send(MyEmailMessage);//发送电子邮件

                try
                {
                    sc.Send(MyEmailMessage);//发送电子邮件
                }
                catch (Exception e)
                {
                    sendadminmail(e.Message + " " + e.Data);
                }
            }
            //MessageBox.Show("已经发送");
        }
        public string getmailtext()
        {
            string mailtext = "Dear Sir/Madam,</br></br>";
            mailtext += "Monitor run on " + DateTime.Now + ",please <a href=\"http://app/MSAS/\">click here</a> to Confirm.</br></br>";


            //人员licens过期
            mailtext += "Employee License Warning Time :" + l_ex.ToString().Trim() + " Days</br>";
            string SQL_LC = "SELECT  count(*) "
            + "    FROM MSAS_HRInfo"
            + "    where HRstatus=0"
            + "    AND LicenseExpired <= GETDATE()+" + l_ex.ToString().Trim();
            string lc = "0";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_LC))
            {
                if (rdr.Read())
                {
                    lc = Convert.ToString(rdr.GetSqlValue(0));

                }

            }
            if (Convert.ToInt32(lc) <= 0)//没有过期数据
            {
                mailtext += "There is no expiring license</br></br>";


            }
            else//有过期数据
            {
                mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                mailtext += "<td>StaffID</td>";
                mailtext += "<td>StaffName</td>";
                mailtext += "<td>License</td>";
                mailtext += "<td>Category</td>";
                mailtext += "<td>LicenseExpired</td>";
                mailtext += "</tr>";
                string SQL_L = "SELECT  StaffID,StaffName ,License     ,category      ,LicenseExpired "
              + "    FROM MSAS_HRInfo"
              + "    where HRstatus=0"
              + "    AND LicenseExpired <= GETDATE()+" + l_ex.ToString().Trim()
              + "   ORDER BY LicenseExpired";

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_L))
                {
                    while (rdr.Read())
                    {
                        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
                        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(4))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                        mailtext += "</tr>";

                    }

                }

                mailtext += "</table></br></br>";
            }

            //authorization  过期
            string SQL_ACC = "SELECT  count(*) "
             + "      FROM MSAS_AuthorizationList"
             + "   where Status<>2    AND Vaild=1"
             + "    AND ExpireDate <= GETDATE()";
            string acc = "0";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_ACC))
            {
                if (rdr.Read())
                {
                    acc = Convert.ToString(rdr.GetSqlValue(0));

                }

            }
            //更新状态
            string up_ac = " UPDATE  MSAS_AuthorizationList"
                + "  SET Vaild=0 "
            + " where ExpireDate < GETDATE()";
            using (SqlConnection conn = new SqlConnection(SqlHelper.Conn))
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, up_ac);
            }
            mailtext += "Update " + acc + " expired authorization to inValid</br></br>";

            //authorization 快到期
            mailtext += "Authorization Warning Time :" + a_ex.ToString().Trim() + " Days</br>";

            string SQL_AC = "SELECT  count(*) "
            + "      FROM MSAS_AuthorizationList"
            + "   where Status<>2    AND Vaild=1"
            + "    AND ExpireDate <= GETDATE()+" + a_ex.ToString().Trim();
            string ac = "0";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_AC))
            {
                if (rdr.Read())
                {
                    ac = Convert.ToString(rdr.GetSqlValue(0));

                }

            }
            if (Convert.ToInt32(ac) <= 0)//没有过期数据
            {
                mailtext += "There is no expiring authorization</br></br>";


            }
            else//有过期数据
            {
                mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                mailtext += "<td>StaffID</td>";
                mailtext += "<td>StaffName</td>";
                mailtext += "<td>Subject</td>";
                mailtext += "<td>Rating</td>";
                mailtext += "<td>Level</td>";
                mailtext += "<td>stamp</td>";
                mailtext += "<td>ExpireDate</td>";
                mailtext += "</tr>";
                string SQL_A = "  SELECT A.StaffID,B.StaffName,Project,Range,Level,stamp,ExpireDate"
              + "         FROM MSAS_AuthorizationList A, MSAS_HRInfo B"
              + "      where Status<>2   AND Vaild=1"
              + "        and A.StaffID=B.StaffID  and B.HRstatus=0"
              + "    AND ExpireDate <= GETDATE()+" + a_ex.ToString().Trim()
              + "   ORDER BY ExpireDate";

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_A))
                {
                    while (rdr.Read())
                    {
                        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";
                        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                        mailtext += "</tr>";

                    }

                }
                mailtext += "</table>";
            }





            //authorization  过期30天
            mailtext += "</br></br>Authorization Expired in past 30 Days</br>";
            string SQL_exped = "SELECT  count(*) "
             + "      FROM  MSAS_AuthorizationList A, MSAS_HRInfo B"
             + "   where A.Status<>2  and A.StaffID=B.StaffID  and B.HRstatus=0  "
             + "      AND A.ExpireDate between DATEADD(dd,-30, GETDATE()) and  GETDATE()";
            string exped = "0";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_exped))
            {
                if (rdr.Read())
                {
                    exped = Convert.ToString(rdr.GetSqlValue(0));

                }

            }


            if (Convert.ToInt32(exped) <= 0)//没有过期数据
            {
                mailtext += "There is no expired authorization</br></br>";


            }
            else//有过期数据
            {
                mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                mailtext += "<td>StaffID</td>";
                mailtext += "<td>StaffName</td>";
                mailtext += "<td>Subject</td>";
                mailtext += "<td>Rating</td>";
                mailtext += "<td>Level</td>";
                mailtext += "<td>stamp</td>";
                mailtext += "<td>ExpireDate</td>";
                mailtext += "</tr>";
                string SQL_A = "  SELECT A.StaffID,B.StaffName,Project,Range,Level,stamp,ExpireDate"
              + "         FROM MSAS_AuthorizationList A, MSAS_HRInfo B"
              + "      where Status<>2   "
              + "        and A.StaffID=B.StaffID  and B.HRstatus=0"
              + "    AND A.ExpireDate between DATEADD(dd,-30, GETDATE()) and  GETDATE()"
              + "   ORDER BY ExpireDate";

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_A))
                {
                    while (rdr.Read())
                    {
                        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
                        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";
                        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                        mailtext += "</tr>";

                    }

                }
                mailtext += "</table>";
            }


            //Training 过期数据

            //string sql_exp = "SELECT StaffID,StaffName,Station,Division,Course,Course_Ref,Training_Date,Training_Required_Date,Training_Type    FROM MSAS_Course_I_R_VW where Alert<>'OK' ";
            //int count = 1;
            //using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, sql_exp))
            //{
            //    while (rdr.Read())
            //    {
            //        if (count == 1)
            //        {
            //            mailtext += "</br></br>Training Expired Records:</br>";
            //            mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
            //            mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
            //            mailtext += "<td>StaffID</td>";
            //            mailtext += "<td>StaffName</td>";
            //            mailtext += "<td>Station</td>";
            //            mailtext += "<td>Division</td>";
            //            mailtext += "<td>Course</td>";
            //            mailtext += "<td>Course Ref</td>";
            //            mailtext += "<td>Training Date</td>";
            //            mailtext += "<td>Training Required Date</td>";
            //            mailtext += "</tr>";

            //        }
            //        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";

            //        if (Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) == "01-Jan-1900")
            //        {
            //            mailtext += "<td>No Records</td>";
            //            if (Convert.ToString(rdr.GetSqlValue(6)) == "Recurrent")
            //            {
            //                mailtext += "<td></td>";
            //            }
            //            else
            //            { 
            //              mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
            //            }
                       
            //        }
            //        else
            //        {
            //            mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                        
            //            mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
            //        }
                   

            //        mailtext += "</tr>";
            //        count++;

            //    }

            //}

            //if (count != 1)
            //    mailtext += "</table>";
            //count = 1;
            ////Training 即将过期

            //string sql_exping = "SELECT StaffID,StaffName,Station,Division,Course,Course_Ref,Training_Date,Training_Required_Date    FROM MSAS_Course_I_R_VW where Alert='OK' and Training_Required_Date between GETDATE()  and dateadd(dd," + T_ex + ",GETDATE()) ";
            //using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, sql_exping))
            //{
            //    while (rdr.Read())
            //    {
            //        if (count == 1)
            //        {
            //            mailtext += "</br></br>Training  Warning Time :" + T_ex + " Days:</br>";
            //            mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
            //            mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
            //            mailtext += "<td>StaffID</td>";
            //            mailtext += "<td>StaffName</td>";
            //            mailtext += "<td>Station</td>";
            //            mailtext += "<td>Division</td>";
            //            mailtext += "<td>Course</td>";
            //            mailtext += "<td>Course Ref</td>";
            //            mailtext += "<td>Training Date</td>";
            //            mailtext += "<td>Training Required Date</td>";
            //            mailtext += "</tr>";

            //        }
            //        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
            //        mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";
            //        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
            //        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";

            //        mailtext += "</tr>";
            //        count++;

            //    }
            //}
            //if (count != 1)
            //    mailtext += "</table>";

            return mailtext;
        }


        //初始化接收人地址
        public bool getmailadd()
        {
            bool flag = false; //false是正常
            string path = @"\\192.168.101.46\app\Falcon\MSAS\mail.ini";
            try
            {


                int i = GetPrivateProfileString("mail", "cc", "", cctext, 255, path);
                int i1 = GetPrivateProfileString("mail", "l_ex", "", l_ex, 255, path);
                int i2 = GetPrivateProfileString("mail", "a_ex", "", a_ex, 255, path);
                int i3 = GetPrivateProfileString("mail", "T_ex", "", T_ex, 255, path);
            }
            catch
            {
                sendadminmail("没有找到config配置文件");
                flag = true;
            }
            return flag;

        }

        //发送管理员报警邮件
        public void sendadminmail(string bodytext)
        {
            string UserName = fromtext;
            string Password = "@@@@@@@";
            string MailServer = "192.168.101.100";
            int smtpPort = 25;
            System.Net.Mail.SmtpClient sc = new SmtpClient(MailServer, smtpPort);
            NetworkCredential nc = new NetworkCredential(UserName, Password);
            sc.Credentials = nc;

            MailMessage MyEmailMessage = new MailMessage();
            MyEmailMessage.From = new MailAddress(UserName);
            //addto(MyEmailMessage);
            //addcc(MyEmailMessage); 
            MyEmailMessage.To.Add(new MailAddress("falcon.wang@airmacau.com.mo"));
            //MyEmailMessage.CC.Add(new MailAddress("falcon.wang@airmacau.com.mo"));
            MyEmailMessage.Subject = "MASA错误提示邮件";
            MyEmailMessage.Body = bodytext;
            MyEmailMessage.IsBodyHtml = false;
            MyEmailMessage.Priority = MailPriority.High;
            sc.Send(MyEmailMessage);//发送电子邮件
        }

        //添加To
        public void addto(MailMessage MyEmailMessage)
        {

            string SQL_to = "SELECt  Email  FROM MSAS_Admin  where Admin_Level='100'";

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, SQL_to))
            {

                while (rdr.Read())
                {
                    MyEmailMessage.To.Add(Convert.ToString(rdr.GetSqlValue(0)));
                }
            }



        }
        //添加CC
        public void addcc(MailMessage MyEmailMessage)
        {
            string[] cclist = cctext.ToString().Split(';');
            int cclistlen = cclist.Length;
            for (int i = 0; i < cclist.Length; i++)
            {
                if (cclist[i] == "")
                    continue;
                else
                    MyEmailMessage.CC.Add(new MailAddress(cclist[i]));
            }

        }
    }
}
