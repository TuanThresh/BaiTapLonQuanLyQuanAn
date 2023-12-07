using PhanMemQuanLiQuanCafe.DAO;
using PhanMemQuanLiQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemQuanLiQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region methods
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCatogery.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);

            LoadListFood();
            AddFoodBinding();
            LoadCatogeryIntoCombobox(cbxCatogeryFood);

            LoadListCategory();
            AddCategoryBinding();
            

            LoadListTable();
            AddTableBinding();


            LoadAccount();
            AddAccountBinding();
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            cbxAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime chechIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(chechIn, checkOut);
        }

        void AddFoodBinding()
        {
            txtNameFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmPriceFood.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBinding()
        {
            txtNameCatogery.DataBindings.Add(new Binding("Text", dtgvCatogery.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDCatogery.DataBindings.Add(new Binding("Text", dtgvCatogery.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            txtNameTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtIDTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            cbxStatusTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }

        void LoadCatogeryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void LoadStatusIntoCombobox(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instace.LoadTableList();
            cb.DisplayMember = "Name";
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instace.GetListTable();
        }

        #endregion

        #region events
        ///Doanh Thu//////////////////////////////////////////
        private void btnBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
        }


        ///Thuc An//////////////////////////////////////////
        private void txtIDFood_TextChanged(object sender, EventArgs e)
        {
            if (dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbxCatogeryFood.SelectedItem = cateogory;

                int index = -1;
                int i = 0;
                foreach (Category item in cbxCatogeryFood.Items)
                {
                    if (item.ID == cateogory.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbxCatogeryFood.SelectedIndex = index;
            }
            else   
                MessageBox.Show("Món ăn không tồn tại", "Thông báo");
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int categoryID = (cbxCatogeryFood.SelectedItem as Category).ID;
            float price = (float)nmPriceFood.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDFood.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int categoryID = (cbxCatogeryFood.SelectedItem as Category).ID;
            float price = (float)nmPriceFood.Value;
            int id = Convert.ToInt32(txtIDFood.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }


        ///Danh Muc//////////////////////////////////////////
        private void btnViewCatogery_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnAddCatogery_Click(object sender, EventArgs e)
        {
            string name = txtNameCatogery.Text;

            if (CategoryDAO.Instance.InsertCategory(name)) 
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadListCategory();
                /*if (insertFood != null)
                    insertFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục");
            }
        }

        private void btnDeleteCatogery_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDCatogery.Text);

            if (CategoryDAO.Instance.DeleteCategory(id)) 
            {
                MessageBox.Show("Xóa danh mục thành công");
                LoadListCategory();
                /*if (deleteFood != null)
                    deleteFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục");
            }
        }

        private void btnEditCatogery_Click(object sender, EventArgs e)
        {
            string name = txtNameCatogery.Text;
            int id = Convert.ToInt32(txtIDCatogery.Text);

            if (CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadListCategory();
                /*if (updateFood != null)
                    updateFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục");
            }
        }

        private void txtIDCatogery_TextChanged(object sender, EventArgs e)
        {
            if (dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbxCatogeryFood.SelectedItem = cateogory;

                int index = -1;
                int i = 0;
                foreach (Category item in cbxCatogeryFood.Items)
                {
                    if (item.ID == cateogory.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbxCatogeryFood.SelectedIndex = index;
            }
        }


        ///Ban An//////////////////////////////////////////
        private void btnViewTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txtNameTable.Text;

            if (TableDAO.Instace.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();
               /* if (insertFood != null)
                    insertFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDTable.Text);

            if (TableDAO.Instace.DeleteTable (id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadListTable();
                /*if (deleteFood != null)
                    deleteFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txtNameTable.Text;
            int id = Convert.ToInt32(txtIDTable.Text);

            if (TableDAO.Instace.UpdateTable(name, id))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadListTable();
                /*if (updateFood != null)
                    updateFood(this, new EventArgs());*/
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = FoodDAO.Instance.SearchFoodByName(txtSearchFood.Text);
        }
        

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

       

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)cbxAccountType.Value;

            AddAccount(userName, displayName, type);
        }
    

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)cbxAccountType.Value;

            EditAccount(userName, displayName, type);
        }
        #endregion

        private void btnReset_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txtPageBill.Text = "1";
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);

            if (page > 1)
                page--;

            txtPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpFromDate.Value, dtpToDate.Value);

            if (page < sumRecord)
                page++;

            txtPageBill.Text = page.ToString();
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpFromDate.Value, dtpToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txtPageBill.Text = lastPage.ToString();
        }

        private void txtPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpFromDate.Value, dtpToDate.Value, Convert.ToInt32(txtPageBill.Text));
        }
    }
}
