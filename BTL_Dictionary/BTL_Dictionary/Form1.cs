using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTL_Dictionary
{
    public partial class frmDic : Form
    {
        public frmDic()
        {
            InitializeComponent();
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            string filePath = "D:\\UI\\dictionary.xlsx"; // đường dẫn file excel
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet ws = wb.Sheets[1];

            // Lấy dữ liệu từ sheet và hiển thị lên ListView
            int row = 2; // bắt đầu từ dòng 2 để bỏ qua tiêu đề
            while (ws.Cells[row, 1].Value != null)
            {
                if (ws.Cells[row, 1].Value.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    ListViewItem item = new ListViewItem(ws.Cells[row, 1].Value.ToString());
                    item.SubItems.Add(ws.Cells[row, 2].Value.ToString());
                    lvResult.Items.Add(item);
                }
                row++;
            }

            // Đóng file excel
            wb.Close();
            excelApp.Quit();
        }

        private void btDel_Click(object sender, EventArgs e)
        {

        }

        private void btAdd_Click(object sender, EventArgs e)
        {

        }

        private void btChange_Click(object sender, EventArgs e)
        {

        }
    }
}
