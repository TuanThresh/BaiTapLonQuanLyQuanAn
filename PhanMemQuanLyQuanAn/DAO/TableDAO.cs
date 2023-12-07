using PhanMemQuanLiQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLiQuanCafe.DAO
{
    internal class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instace
        {
            get { if(instance == null) instance = new TableDAO(); return instance;}
            private set { instance = value; }
        }

        public static int TableWidth = 80;
        public static int TableHeight = 80;
        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable1 @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> tabelList = new List<Table> ();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tabelList.Add(table);
            }

            return tabelList;
        }
        public List<Table> GetListTable()
        {
            List<Table> list = new List<Table>();
            string query = "select * from TableFood";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                list.Add(table);
            }

            return list;
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT dbo.TableFood ( name )VALUES  ( N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(string name, int id)
        {
            string query = string.Format("UPDATE dbo.TableFood SET name = N'{0}' WHERE id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int idTable)
        {

            string query = string.Format("Delete dbo.TableFood where id = {0}", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
