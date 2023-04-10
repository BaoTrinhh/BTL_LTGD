using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BTL_Dictionary.frmMain;

namespace BTL_Dictionary
{
    public partial class FormEdit : Form
    {
        private object txtFilePath;

        public FormEdit()
        {
            InitializeComponent();
        }

        //LẤY THÔNG TIN CỦA TỪ 
        private void btGetInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKey.Text != "")
                {
                    string mean = TranslationService.Translate(txtKey.Text);
                    txtMean.Text = mean;
                }
               else
                {
                    MessageBox.Show("Vui long nhap _tu khoa_ can kiem tra thong tin!!!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception) { }

        }

        //THÊM TỪ VỰNG
        private void btAdd_Click(object sender, EventArgs e)
        {
            try
            {
               if(txtKey.Text != "" && txtMean.Text != "")
               {
                    frmMain.TranslationService.CheckWord(txtKey.Text, txtMean.Text);                   
               }
               else
               {
                    MessageBox.Show("Vui long nhap _tu khoa & nghia cua tu_can them vao!!!","Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);

               }

            }
            catch
            {
                MessageBox.Show("Thêm từ mới thất bại: " + txtKey.Text);
            }
        }

        //THAY ĐỔI_SỬA NGHĨA CỦA TỪ TRONG FILE
        private void btChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKey.Text != "" && txtMean.Text != "")
                {
                    frmMain.TranslationService.RemoveTranslationbyWord(txtKey.Text, txtMean.Text);
                }
                else
                {
                    MessageBox.Show("Vui long nhan _GetInfo_ de kiem tra thong tin co ton tai khong???","Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                frmMain.TranslationService.AddTranslationByWord(txtKey.Text, txtMean.Text, true);
                MessageBox.Show("Thay đổi dữ liệu thành công", "Thông báo", MessageBoxButtons.OK );
            }
            catch
            {
                MessageBox.Show("Thêm từ mới thất bại: " + txtKey.Text);
            }

        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtKey.Text != "")
                {
                    TranslationService.RemoveTranslationbyWord(txtKey.Text, txtMean.Text);
                    MessageBox.Show("Xóa từ " + "< " + txtKey.Text + " >" +" thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception)
            { };
        }
        private void btClear_Click(object sender, EventArgs e)
        {
            txtKey.Clear();
            txtMean.Clear();
        }
    }
}
