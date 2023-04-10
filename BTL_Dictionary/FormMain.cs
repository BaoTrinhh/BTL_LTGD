using Microsoft.Office.Interop.Word;
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
using Excel = Microsoft.Office.Interop.Word;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace BTL_Dictionary
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            TranslationService.InitDictionary();
        }
       
        //Chức năng tìm kiếm
        private void btFind_Click(object sender, EventArgs e)
        {
     
            try
            {
                if(txtSearch.Text != "")
                {
                    // bảo trinh vuawfw update dòng này
                    string mean = TranslationService.Translate(txtSearch.Text);
                    txtMean.Text = mean;
                    //History
                    ListViewItem item = new ListViewItem(txtSearch.Text);
                    item.SubItems.Add(txtMean.Text);
                    lvResult.Items.Add(item);
                }
            }
            catch (Exception)  {  }

            if (txtSearch.Text == "")
                MessageBox.Show("Vui lòng nhập từ cần tra cứu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //Chức năng xóa từ trên ô nhập 
        private void btClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtMean.Clear();
        }
        //Chức năng xóa từ trong danh sách lịch sử
        private void btDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvResult.SelectedItems)
                lvResult.Items.Remove(item);
        }
        //Chức năng clear tất cả từ trong lịch sử
        private void btClearAll_Click(object sender, EventArgs e)
        {
            lvResult.Items.Clear();
        }

        // Mở form con EditWords
        private void ImportToolAdd_Click(object sender, EventArgs e)
        {
            FormEdit AddForm = new FormEdit();
            AddForm.Show();
        }
        //listbox1 gợi ý 1 số từ tiếng anh để tra cứu
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listBox1.SelectionMode = SelectionMode.MultiExtended;
                int index = listBox1.SelectedIndex;
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (i == index)
                    {
                        string value = listBox1.Items[i].ToString();
                        txtSearch.Text = value;
                        txtMean.Text = "";
                    }
                }
            }
            catch (Exception) { }
        }
        //listbox2 gợi ý 1 số từ tiếng Việt để tra cứu
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listBox1.SelectionMode = SelectionMode.MultiExtended;
                int index = listBox2.SelectedIndex;
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    if (i == index)
                    {
                        string value = listBox2.Items[i].ToString();
                        txtSearch.Text = value;
                        txtMean.Text = "";
                    }
                }
            }
            catch (Exception) { }
        }
        //Đọc file txt
        private void texttxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.Title = "Import Text File";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    
                    string fileName = openFileDialog1.FileName;
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string lines = "";
                        string line;
                        string[] words = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != null)
                            {
                                
                                words = line.Split(':');
                                if( words.Length == 2)
                                {
                                    lines += line + "\n";
                                    frmMain.TranslationService.AddTranslationByWord(words[0], words[1], false);
                                }
                               
                            }

                        } 
                        MessageBox.Show("Import file " + fileName + " thành công!!!");
                        frmMain.TranslationService.SaveWordsInFile(lines);
                    }
                    

                }
            }
            catch(Exception)
            {
                MessageBox.Show("Import file Dictionary.txt thất bại!!!");
            };
            

        }

        //Đọc file excel 
        private void ExcelxlsxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XLSX files (*.xlsx)|*.xlsx";
            openFileDialog1.Title = "Import File XLSX";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    {
                    string fileName = openFileDialog1.FileName;
                    using (ExcelPackage package = new ExcelPackage(fileName))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;
                        string lines = "";
                        for (int i = 2; i <= rowCount; i++)
                        {
                            if (colCount == 2)
                            {
                                lines += worksheet.Cells[i, 1].Value.ToString() +":"+ worksheet.Cells[i, 2].Value.ToString() + "\n";
                            }
                            frmMain.TranslationService.AddTranslationByWord(worksheet.Cells[i, 1].Value.ToString(), worksheet.Cells[i, 2].Value.ToString(), false);
                        }
                        frmMain.TranslationService.SaveWordsInFile(lines);//lưu tất cả các "từ khóa và value" vào hàm SaveWordInfile để ghi hết xuống file mặc định.
                        MessageBox.Show("Import file Dictionary.xlsx thành công!!!");
                    }    
                        
                }    
                
                }
        }

        public partial class TranslationService
        {
            private static readonly Dictionary<string, string> Translations = new Dictionary<string, string>
            {};

            //kiểm tra từ khóa tồn tại k và trả ra kq
            public static string Translate(string text)
            {
                string word = text.ToLower();          
                if (Translations.ContainsKey(word))
                {     
                    return Translations[word];
                }
                else if(Translations.ContainsValue(word))
                {
                    return Translations[word];
                }    
                return "chưa có thông tin !!!";
            }

            //Ghi-Thêm từ mới vào file mặc định
            public static bool AddTranslationByWord(string key, string value, bool is_save_file)
            {
                try
                {
                    Translations[key.ToLower()] = value.ToLower();
                    //Translations[value.ToLower()] = key.ToLower();
                    string new_word = "";
                   
                    if(is_save_file == true)
                    {
                        using (StreamWriter writer = new StreamWriter("Dictionary.txt", true))
                        {
                            new_word = key.ToLower() + ":" + value.ToLower();
                            writer.Write(new_word);
                            writer.WriteLine();
                        }
                    }
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }

            }
            //kiểm tra sự tồn tại của từ khóa trước khi Thêm vào Từ Điển
            public static void CheckWord(string key, string value)
            {
                if (Translations.ContainsKey(key))
                {
                    MessageBox.Show("Từ khóa < " + key + " > đã tồn tại!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Translations.ContainsValue(value))
                {
                    MessageBox.Show("Nghĩa của từ < " + key + " > đã tồn tại!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TranslationService.AddTranslationByWord(key, value, true);
                    MessageBox.Show("Thêm từ" + "< " + key + " >" + " thành công");
                }
            }

            //mặc định đọc file
            public static void InitDictionary()
            {
                using (StreamReader sr = new StreamReader("Dictionary.txt"))
                {
                    string line;
                    string[] words = null;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != null)
                            {
                                words = line.Split(':');
                                frmMain.TranslationService.AddTranslationByWord(words[0], words[1], false);
                                frmMain.TranslationService.AddTranslationByWord(words[1], words[0], false);
                            }

                        }
                    }
                    catch (Exception)
                    { };
                }
            }
           

            //Lưu và thêm dữ liệu từ file import xuống file mặc định
            public static bool SaveWordsInFile(string words)
            {
                try
                { 
                    using (StreamWriter writer = File.AppendText("Dictionary.txt"))
                    {
                        writer.Write(words); 
                    }
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }

            }

            

            //chức năng Xóa từ trong file
            public static bool RemoveTranslationbyWord(string key, string text)
            {
                try
                {
                    string newDictSaveFile = "";
                    bool result = Translations.Remove(key.ToLower());
                    if (result == true)
                    {
                        foreach (string keyDict in Translations.Keys)
                        {
                            newDictSaveFile += keyDict + ":" + Translations[keyDict] + "\n";
                        }
                        OverWriteDict(newDictSaveFile);//lưu dữ liệu mới ---> ghi đè dữ liệu
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            //khi xóa 1 từ trong file ==> ghi đè dữ liệu mới hiện có vào file
            public static bool OverWriteDict(string words)
            {
                try
                {
                    File.WriteAllText("Dictionary.txt", words);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }

        }
    }
}
