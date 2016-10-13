using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;

using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Net;
using System.Web;
using System.Net.Mail;
using SMS.DBUtility;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;

namespace test
{
    class Program
    {

        //ini设置
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    


        //定义数据泛型
        public class Employee
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Prefer { get; set; }
            public string Div { get; set; }
            public string Dept { get; set; }
            public string JobTitle { get; set; }
        }

        //泛型比较
        public class EmployeeComPare : IEqualityComparer<Employee>
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Prefer { get; set; }
            public string Div { get; set; }
            public string Dept { get; set; }
            public string JobTitle { get; set; }

            public bool Equals(Employee x, Employee y)
            {
                return x.ID == y.ID && x.Name == y.Name && x.Prefer == y.Prefer && x.Div == y.Div && x.Dept == y.Dept && x.JobTitle == y.JobTitle;
            }

            public int GetHashCode(Employee obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        //泛型比较只比较ID
        public class EmployeeComParer : IEqualityComparer<Employee>
        {

        

            public bool Equals(Employee x, Employee y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(Employee obj)
            {
                return obj.ToString().GetHashCode();
            }
        }




        static void Main(string[] args)
        {

            StringBuilder totext = new StringBuilder(255);
            StringBuilder cctext = new StringBuilder(255);
            StringBuilder ip = new StringBuilder(255);
            StringBuilder db = new StringBuilder(255);
            StringBuilder user = new StringBuilder(255);
            StringBuilder pwd = new StringBuilder(255);

             string path = @"C:\Users\itdwzm\Documents\Visual Studio 2013\Projects\test\test\bin\Debug\HR_info_Reminder_config.ini";
           


                int i = GetPrivateProfileString("mail", "cc", "", cctext, 255, path);
                int i1 = GetPrivateProfileString("mail", "to", "", totext, 255, path);
                int i2 = GetPrivateProfileString("db", "ip", "", ip, 255, path);
                int i3 = GetPrivateProfileString("db", "database", "", db, 255, path);
                int i4 = GetPrivateProfileString("db", "user", "", user, 255, path);
                int i5 = GetPrivateProfileString("db", "password", "", pwd, 255, path);
                
          


            //数据库连接
            string Conn = "server=" + ip.ToString().Trim() + ";User Id = " + user.ToString().Trim() + ";Password = " + pwd.ToString().Trim() + "; database = " + db.ToString().Trim();
            //读取昨天信息
            //
            List<Employee> oldlist = new List<Employee>();
            string SQL_old = "select * from AM_STAFF3 order by EMPLID";

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(Conn, CommandType.Text, SQL_old))
            {
                while (rdr.Read())
                {
                    Employee olddata = new Employee();
                    olddata.ID = Convert.ToString(rdr.GetSqlValue(0));
                    olddata.Name = Convert.ToString(rdr.GetSqlValue(1));
                    olddata.Prefer = Convert.ToString(rdr.GetSqlValue(2));
                    olddata.Div = Convert.ToString(rdr.GetSqlValue(3));
                    olddata.Dept = Convert.ToString(rdr.GetSqlValue(4));
                    olddata.JobTitle = Convert.ToString(rdr.GetSqlValue(5));
                    oldlist.Add(olddata);

                }

            }



            //获取今天的数据

            List<Employee> newlist = new List<Employee>();
            string SQL_new = "select * from AM_STAFF2 order by EMPLID";

            using (SqlDataReader rdr = SqlHelper.ExecuteReader(Conn, CommandType.Text, SQL_new))
            {
                while (rdr.Read())
                {
                    Employee newdata = new Employee();
                    newdata.ID = Convert.ToString(rdr.GetSqlValue(0));
                    newdata.Name = Convert.ToString(rdr.GetSqlValue(1));
                    newdata.Prefer = Convert.ToString(rdr.GetSqlValue(2));
                    newdata.Div = Convert.ToString(rdr.GetSqlValue(3));
                    newdata.Dept = Convert.ToString(rdr.GetSqlValue(4));
                    newdata.JobTitle = Convert.ToString(rdr.GetSqlValue(5));
                    newlist.Add(newdata);

                }

            }
           //
            Console.WriteLine("old");
            foreach (var item in newlist)
            {
                Console.WriteLine(item.ID);

            }


            Console.ReadLine();
            //离职列表 
          

            List<Employee> leaverlist = oldlist.Except(newlist, new EmployeeComParer()).ToList();
            //新员工列表
            List<Employee> hirelist = newlist.Except(oldlist, new EmployeeComParer()).ToList();
            //共同拥有
            List<Employee> samelist = oldlist.Intersect(newlist, new EmployeeComParer()).ToList();
            //不同的new-hire
            List<Employee> changelist = newlist.Except(hirelist, new EmployeeComPare()).ToList();
            //更新的
            List<Employee> updatelist = changelist.Except(samelist, new EmployeeComPare()).ToList();
            //更新数据未更新前
            List<Employee> nosamelist = samelist.Intersect(updatelist, new EmployeeComParer()).ToList();

        }
    }
}
