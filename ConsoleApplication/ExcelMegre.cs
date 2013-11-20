using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace ConsoleApplication
{
    class ExcelMegreTest
    {
        public static void Test()
        {
            //string connString07 = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=test.xlsx;Extended Properties='Excel 12.0;HDR=Yes'";
            //string connString03 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=test.xls;Extended Properties='Excel 8.0;HDR=Yes'";

            string connString1 = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=test1.xls;Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
            string connString2 = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=test2.xls;Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";

            DataTable dt1 = GetDataTable(connString1);
            DataTable dt2 = GetDataTable(connString2);

            var l1 = DataTable2List<QQ>(dt1);
            var l2 = DataTable2List<QQ>(dt2);

            var l = l1.Concat(l2).Distinct(new QQEqualityComparer());

            foreach (var item in l1)
            {
                Console.WriteLine(item.Number + "," + item.Name);
            }

            Console.WriteLine("\n----------------------------");

            foreach (var item in l2)
            {
                Console.WriteLine(item.Number + "," + item.Name);
            }

            Console.WriteLine("\n----------------------------");

            foreach (var item in l)
            {
                Console.WriteLine(item.Number + "," + item.Name);
            }
            
        }

        static DataTable GetDataTable(string connString)
        {
            DataTable table = null;

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                OleDbDataAdapter adapter = null;
                DataSet ds = new DataSet();
                adapter = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                adapter.Fill(ds, "table1");
                table = ds.Tables[0];
            }

            return table;

        }

        //DataTable 转为 List 
        static List<T> DataTable2List<T>(DataTable table) where T : class,new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (table.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                //创建TResult的实例
                T ob = new T();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }

            return oblist;
        }


        //List 转为 DataTable
        static DataTable List2DataTable<T>(IEnumerable<T> list) where T : class
        {
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    class QQ
    {
        public string Number { get; set; }
        public string Name { get; set; }

    }

    class QQEqualityComparer : IEqualityComparer<QQ>
    {
        public bool Equals(QQ x, QQ y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Number == y.Number;
        }

        public int GetHashCode(QQ obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    //class QQ:IEqualityComparer<QQ>
    //{
    //    public string Number { get; set; }
    //    public string Name { get; set; }

    //    public bool Equals(QQ x, QQ y)    //比较x和y对象是否相同，按照地址比较
    //    {
    //        return x.Number == y.Number;
    //    }

    //    public int GetHashCode(QQ obj)
    //    {
    //        return obj.ToString().GetHashCode();
    //    }
    //}

}
