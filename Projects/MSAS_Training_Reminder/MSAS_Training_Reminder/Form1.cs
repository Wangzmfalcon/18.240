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



namespace MSAS_Training_Reminder
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
        string fromtext = "ENM_Training@airmacau.com.mo";



        public Form1()
        {
           
            try { sendmail(); }
            catch { }

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
                //addto(MyEmailMessage);
                //addcc(MyEmailMessage);
                MyEmailMessage.To.Add(new MailAddress("xeno.zhang@airmacau.com.mo"));
                MyEmailMessage.To.Add(new MailAddress("liufeng@airmacau.com.mo"));
                MyEmailMessage.Bcc.Add(new MailAddress("falcon.wang@airmacau.com.mo"));
                MyEmailMessage.IsBodyHtml = true;
                MyEmailMessage.BodyEncoding = Encoding.GetEncoding(936);
                MyEmailMessage.Subject = "ENM Training Reminder ";

                //获得邮件内容
                MyEmailMessage.Body = getmailtext();//获得发送内容
                //MyEmailMessage.Body= "<font color='red'>邮件测试，呵呵</font>";

                MyEmailMessage.Priority = MailPriority.High;
                try
                {
                    sc.Send(MyEmailMessage);//发送电子邮件
                }
                catch (Exception e)
                {
                    sendadminmail("发送提示邮件失败");
                }
            }
            //MessageBox.Show("已经发送");
        }
        public string getmailtext()
        {
            string mailtext = "Dear Sir/Madam,</br></br>";
            mailtext += "Monitor run on " + DateTime.Now + ",please <a href=\"http://app/MSAS/\">click here</a> to Confirm.</br></br>";

            int count = 1;
            //Training 即将过期

            string sql_exping = "SELECT StaffID,StaffName,Station,Division,Course,Course_Ref,Training_Date,Training_Required_Date    FROM MSAS_Course_I_R_VW where (Alert='OK' or Alert='Need Initial')  and Training_Required_Date between GETDATE()  and dateadd(dd," + T_ex + ",GETDATE()) order by StaffID ";
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, sql_exping))
            {
                while (rdr.Read())
                {
                    if (count == 1)  
                    {
                        mailtext += "</br></br>Training  Warning Time :" + T_ex + " Days:</br>";
                        mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                        mailtext += "<td>StaffID</td>";
                        mailtext += "<td>StaffName</td>";
                        mailtext += "<td>Station</td>";
                        mailtext += "<td>Division</td>";
                        mailtext += "<td>Course</td>";
                        mailtext += "<td>Course Ref</td>";
                        mailtext += "<td>Training Date</td>";
                        mailtext += "<td>Training Required Date</td>";
                        mailtext += "</tr>";

                    }
                    mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";
                    mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                    mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";

                    mailtext += "</tr>";
                    count++;

                }
            }
            if (count != 1)
                mailtext += "</table>";
         

            //Training 过期数据

            string sql_exp = "SELECT StaffID,StaffName,Station,Division,Course,Course_Ref,Training_Date,Training_Required_Date,Training_Type    FROM MSAS_Course_I_R_VW where  Alert<>'OK'  and Training_Required_Date <getdate() order by StaffID ";
             count = 1;
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.Conn, CommandType.Text, sql_exp))
            {
                while (rdr.Read())
                {
                    if (count == 1)
                    {
                        mailtext += "</br></br>Training Expired Records:</br>";
                        mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                        mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                        mailtext += "<td>StaffID</td>";
                        mailtext += "<td>StaffName</td>";
                        mailtext += "<td>Station</td>";
                        mailtext += "<td>Division</td>";
                        mailtext += "<td>Course</td>";
                        mailtext += "<td>Course Ref</td>";
                        mailtext += "<td>Training Date</td>";
                        mailtext += "<td>Training Required Date</td>";
                        mailtext += "</tr>";

                    }


                    //不显示没记录的 recurrent
                    if (Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) == "01-Jan-1900" && Convert.ToString(rdr.GetSqlValue(8)) == "General Recurrent" )
                        continue;

                        
                    mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(0)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(1)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(2)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(3)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(4)) + "</td>";
                    mailtext += "<td>" + Convert.ToString(rdr.GetSqlValue(5)) + "</td>";

                    if (Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) == "01-Jan-1900")
                    {
                        mailtext += "<td>No Records</td>";
                        if (Convert.ToString(rdr.GetSqlValue(8)) == "General Recurrent" || Convert.ToString(rdr.GetSqlValue(8)) == "Position Recurrent")
                        {
                            mailtext += "<td></td>";
                        }
                        else
                        {
                            mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                        }

                    }
                    else
                    {
                        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(6))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";

                        mailtext += "<td>" + Convert.ToDateTime(Convert.ToString(rdr.GetSqlValue(7))).ToString("dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "</td>";
                    }


                    mailtext += "</tr>";
                    count++;

                }

            }

            if (count != 1)
                mailtext += "</table>";
           

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
