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


namespace ITA_Reminder
{
    public partial class Form1 : Form
    {

        //ini设置
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        StringBuilder totext = new StringBuilder(255);
        StringBuilder cctext = new StringBuilder(255);
        StringBuilder ip = new StringBuilder(255);
        StringBuilder db = new StringBuilder(255);
        StringBuilder user = new StringBuilder(255);
        StringBuilder pwd = new StringBuilder(255);

        string fromtext = "IT_Agreement_Reminder@airmacau.com.mo";


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


        //发送邮件
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
            if (!getmailadd())
            {
                //添加收件人
                addto(MyEmailMessage);
                addcc(MyEmailMessage);

                MyEmailMessage.IsBodyHtml = true;
                MyEmailMessage.BodyEncoding = Encoding.GetEncoding(936);
                MyEmailMessage.Subject = "IT agreement Reminder";

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
        }
        
        //邮件主题
        public string getmailtext()
        {

            //数据库连接
            string Conn = "server=" + ip.ToString().Trim() + ";User Id = " + user.ToString().Trim() + ";Password = " + pwd.ToString().Trim() + "; database = " + db.ToString().Trim();
            //一个月将到期list
            List<Agreement> onelist = new List<Agreement>();
            //两个个月将到期list
            List<Agreement> twolist = new List<Agreement>();
            
            //查询语句
            string SQL = "select Ag_id,Description,CONVERT(varchar(100), Ag_Expire_date, 105)  from ITA_Ag_List   where Ag_Expire_date=CONVERT(varchar(100), DATEADD(MM,@month,GETDATE()), 23) ";
            SqlParameter[] parm = new SqlParameter[]{
                new SqlParameter("@month", SqlDbType.Int)
            };


            //一个月查询
            parm[0].Value = 1;
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(Conn, CommandType.Text, SQL, parm))
            {
                while (rdr.Read())
                {
                    Agreement ag1 = new Agreement();
                    ag1.ID = Convert.ToString(rdr.GetSqlValue(0));
                    ag1.Descr = Convert.ToString(rdr.GetSqlValue(1));
                    ag1.Expd = Convert.ToString(rdr.GetSqlValue(2));
                    onelist.Add(ag1);

                }
            }
            
            //两个月查询
                      //一个月查询
            parm[0].Value = 2;
            using (SqlDataReader rdr = SqlHelper.ExecuteReader(Conn, CommandType.Text, SQL, parm))
            {
                while (rdr.Read())
                {
                    Agreement ag2 = new Agreement();
                    ag2.ID = Convert.ToString(rdr.GetSqlValue(0));
                    ag2.Descr = Convert.ToString(rdr.GetSqlValue(1));
                    ag2.Expd = Convert.ToString(rdr.GetSqlValue(2));
                    twolist.Add(ag2);

                }
            }

            //开始写邮件

            string mailtext = "Dear Sir/Madam,</br></br>";
            mailtext += "Monitor facility runs on " + DateTime.Now + "</br></br>";


            if (onelist.Count!=0)
            {
                //表头

                mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                mailtext += " <tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#0F0\"><td colspan=\"3\">Agreement will expire in one month</td></tr>";
                mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                mailtext += "<td>Agreement ID</td>";
                mailtext += "<td>Description</td>";
                mailtext += "<td>Expired Date</td>";
          
                mailtext += "</tr>";
                foreach (var item in onelist)
                {
                    mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                    mailtext += "<td>" + item.ID + "</td>";
                    mailtext += "<td>" + item.Descr + "</td>";
                    mailtext += "<td>" + item.Expd + "</td>";
                    mailtext += "</tr>";
                }

                mailtext += "</table></br></br>";
            }

            if (twolist.Count != 0)
            {
                //表头

                mailtext += "<table border=\"1\" cellspacing=\"0\" cellpadding=\"0\">";
                mailtext += " <tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#0F0\"><td colspan=\"3\">Agreement will expire in two month</td></tr>";
                mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\" style=\"background-color:#CCC\">";
                mailtext += "<td>Agreement ID</td>";
                mailtext += "<td>Description</td>";
                mailtext += "<td>Expired Date</td>";

                mailtext += "</tr>";
                foreach (var item in twolist)
                {
                    mailtext += "<tr align=\"center\" bgcolor=\"#FFFFFF\">";
                    mailtext += "<td>" + item.ID + "</td>";
                    mailtext += "<td>" + item.Descr + "</td>";
                    mailtext += "<td>" + item.Expd + "</td>";
                    mailtext += "</tr>";
                }

                mailtext += "</table></br></br>";
            }

            return mailtext;
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


        //添加to
        public void addto(MailMessage MyEmailMessage)
        {
            string[] tolist = totext.ToString().Split(';');
            int tolistlen = tolist.Length;
            for (int i = 0; i < tolist.Length; i++)
            {
                if (tolist[i] == "")
                    continue;
                else
                    MyEmailMessage.To.Add(new MailAddress(tolist[i]));
            }

        }
        public bool getmailadd()
        {
            bool flag = false; //false是正常
            string path = System.Windows.Forms.Application.StartupPath + "\\ITA_Reminder_config.ini";
            try
            {


                int i = GetPrivateProfileString("mail", "cc", "", cctext, 255, path);
                int i1 = GetPrivateProfileString("mail", "to", "", totext, 255, path);
                int i2 = GetPrivateProfileString("db", "ip", "", ip, 255, path);
                int i3 = GetPrivateProfileString("db", "database", "", db, 255, path);
                int i4 = GetPrivateProfileString("db", "user", "", user, 255, path);
                int i5 = GetPrivateProfileString("db", "password", "", pwd, 255, path);

            }
            catch
            {
                sendadminmail("没有找到config配置文件");
                flag = true;
            }
            return flag;

        }

        //定义数据泛型
        public class Agreement
        {
            public string ID { get; set; }
            public string Descr { get; set; }
            public string Expd { get; set; }
           
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
    }
}
