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
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace 柒幻_霜降
{
    public partial class Frm_Main : Form
    {
        public Frm_Main()
        {
            InitializeComponent();
        }

        DirectoryInfo root;
        public static int treeview_selected_index;
        bool first_select_file = true;
        public static readonly List<RichTextBox> Rtxs = new List<RichTextBox>(4);
        public static string current_file_name;
        public static bool change_folder_path = false;
        readonly List<int> search_item_index = new List<int>();
        public static readonly int save_tag_length = 5;
        public static List<char> separators = new List<char>();
        public static string file_name_from_add = "";
        // File Property from TreeView
        public static readonly List<string> full_name = new List<string>();
        readonly List<string> directory_name = new List<string>();
        readonly List<DateTime> create_time = new List<DateTime>();
        readonly List<DateTime> last_access_time = new List<DateTime>();
        readonly List<long> file_length = new List<long>();
        // File Property
        public static string[] f_full_name = new string[2];
        readonly DateTime[] f_create_time = new DateTime[2];
        readonly DateTime[] f_last_access_time = new DateTime[2];
        readonly long[] f_file_length = new long[2];
        // Settings
        public static string folder_path = ConfigurationManager.AppSettings["FolderPath"];
        public static string font_name;
        public static int font_size;
        public static string forecolor;
        public static string backcolor;
        public static bool extend_file_content;
        public static bool extend_pair_document;
        public static int decimal_place;
        // Pair Document
        int doc_view_type = 0; // 0 Single, 1 Horizontal, 2 Vertical
        public static int target_tab = 0; // 0 Left or Top, 1 Right or Bottom
        int target_rtx = 0; // 0 Rtx_File, 1 Rtx_Property, 2 Rtx_File_2, 3 Rtx_Property_2
        bool first_show_pair_doc = true;
        public static readonly List<TabControl> Tabcontrols = new List<TabControl>(2);
        public static readonly List<RichTextBox> Rtxs_file = new List<RichTextBox>(2);
        readonly List<RichTextBox> Rtxs_property = new List<RichTextBox>(2);
        // Temporary Document
        public static readonly bool[] temporty_text = new bool[2];
        // Text Change
        readonly bool[] text_changed = new bool[2];
        readonly byte[] changed_times = new byte[2];
        readonly bool[] open_file = new bool[2];
        // Find & Replace
        public static int FAR_type;
        // Menu
        readonly List<Panel> Pn_menu = new List<Panel>();
        readonly List<Panel> Pn_formula_controls = new List<Panel>();
        readonly List<Panel> Pn_data_controls = new List<Panel>();
        readonly List<Panel> Pn_data_generate_controls = new List<Panel>();
        int last_pn_menu_target = -1;
        // Output
        bool error_output = false;
        public static readonly List<RichTextBox> Rtx_output = new List<RichTextBox>(1);
        public static char generate_separator = ' ';
        // Matrix
        bool is_matrix = false;
        // Solstice
        string ans = "";
        readonly public static List<RichTextBox> Rtx_solstice = new List<RichTextBox>(1);
        // Quick generate
        readonly char quick_generate_separator = '-';

        private void AutoResize()
        {
            Tx_Search.Width = Flpn_Search.Width - Lbl_Search.Width - 12;
            Btn_CopyFromOutput.Location = new Point(TabControl_Output.Width - Btn_CopyFromOutput.Width, 0);
            Btn_CopyFromOutput.BringToFront();
        }

        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;
            byte curByte;
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("not expected byte format");
            }
            return true;
        }

        public static System.Text.Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };
            Encoding reVal = Encoding.Default;
            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        private void LoadFileToTreeView()
        {
            TreeView.Nodes.Clear();
            full_name.Clear();
            directory_name.Clear();
            create_time.Clear();
            last_access_time.Clear();
            file_length.Clear();
            if (!Directory.Exists(folder_path))
            {
                CheckFolderPath();
            }
            root = new DirectoryInfo(folder_path);
            foreach (FileInfo f in root.GetFiles())
            {
                if (f.Extension == ".txt")
                {
                    TreeView.Nodes.Add(f.Name.Substring(0, f.Name.IndexOf('.')));
                    full_name.Add(f.FullName);
                    create_time.Add(f.CreationTime);
                    last_access_time.Add(f.LastAccessTime);
                    file_length.Add(f.Length);
                    directory_name.Add(f.DirectoryName);
                }
            }
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            Rtxs.Add(Rtx_File);
            Rtxs.Add(Rtx_Property);
            Rtxs.Add(Rtx_File_2);
            Rtxs.Add(Rtx_Property_2);
            Rtxs_file.Add(Rtx_File);
            Rtxs_file.Add(Rtx_File_2);
            Rtxs_property.Add(Rtx_Property);
            Rtxs_property.Add(Rtx_Property_2);
            Tabcontrols.Add(TabControl_File);
            Tabcontrols.Add(TabControl_File_2);
            Pn_menu.Add(Pn_File);
            Pn_menu.Add(Pn_Formula);
            Pn_menu.Add(Pn_Data);
            Pn_menu.Add(Pn_Solstice);
            //Pn_menu.Add(Pn_Settings);
            ShowPnFromMenu(0);
            // Menu Formula
            Pn_formula_controls.Add(Pn_Formula_Sum_Buttons);
            Pn_formula_controls.Add(Pn_Formula_Statistics_Buttons);
            Pn_formula_controls.Add(Pn_Formula_Math_Buttons);
            Pn_formula_controls.Add(Pn_Formula_Matrix_Buttons);
            // Menu Data
            Pn_data_controls.Add(Pn_Data_Generate_Controls);
            Pn_data_controls.Add(Pn_Data_Sample_Controls);
            Pn_data_controls.Add(Pn_Data_Sort_Controls);
            Pn_data_generate_controls.Add(Pn_Data_Generate_Controls_3);
            Pn_data_generate_controls.Add(Pn_Data_Generate_Controls_4);
            Pn_data_generate_controls.Add(Pn_Data_Generate_Controls_5);
            Pn_data_generate_controls.Add(Pn_Data_Generate_Controls_6);
            Combo_Data_Generate_GenerateType.SelectedIndex = 0;
            Combo_Data_Generate_DataType.SelectedIndex = 0;
            // Output
            Rtx_output.Add(Rtx_Output);
            Rtx_File.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            Rtx_File_2.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            Rtx_Solstice.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            StatusStrip_RAC.Visible = false;
            splitContainer_Rtx_Output.Panel2Collapsed = true;
            // Solstice
            Rtx_solstice.Add(Rtx_Solstice);
            // Add separators
            separators.Add(' ');
            separators.Add(',');
            separators.Add(';');
            separators.Add('\n');
            // Settings
            folder_path = ConfigurationManager.AppSettings["FolderPath"];
            if (folder_path == @"C:\")
            {
                folder_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                XmlDocument doc = new XmlDocument();
                string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                doc.Load(strFileName);
                XmlNodeList nodes = doc.GetElementsByTagName("add");
                for (int i = 1; i < nodes.Count; i++)
                {
                    XmlAttribute att = nodes[i].Attributes["key"];
                    if (att.Value == "FolderPath")
                    {
                        att = nodes[i].Attributes["value"];
                        att.Value = folder_path;
                    }
                }
                doc.Save(strFileName);
                ConfigurationManager.RefreshSection("appSettings");
            }
            font_name = ConfigurationManager.AppSettings["FontName"];
            font_size = Convert.ToInt32(ConfigurationManager.AppSettings["FontSize"]);
            forecolor = ConfigurationManager.AppSettings["ForeColor"];
            backcolor = ConfigurationManager.AppSettings["BackColor"];
            自动换行WToolStripMenuItem.Checked = Convert.ToBoolean(ConfigurationManager.AppSettings["WordWrap"]);
            extend_file_content = Convert.ToBoolean(ConfigurationManager.AppSettings["ExtendContent"]);
            extend_pair_document = Convert.ToBoolean(ConfigurationManager.AppSettings["ExtendPairDocument"]);
            generate_separator = Convert.ToChar(ConfigurationManager.AppSettings["GenerateSeperator"]);
            decimal_place = Convert.ToInt32(ConfigurationManager.AppSettings["DecimalPlace"]);
            foreach (RichTextBox rtx in Rtxs)
            {
                rtx.Font = new Font(font_name, font_size);
                rtx.ForeColor = ColorTranslator.FromHtml(forecolor);
                rtx.BackColor = ColorTranslator.FromHtml(backcolor);
                rtx.WordWrap = 自动换行WToolStripMenuItem.Checked;
            }
            Rtx_Solstice.Font = Rtx_Output.Font = Rtxs[0].Font;
            Rtx_Solstice.ForeColor = Rtx_Output.ForeColor = Rtxs[0].ForeColor;
            Rtx_Solstice.BackColor = Rtx_Output.BackColor = Rtxs[0].BackColor;
            Rtx_Solstice.WordWrap = Rtx_Output.WordWrap = Rtxs[0].WordWrap;
            // Load
            AutoResize();
            LoadFileToTreeView();
            splitContainer_Menu.SplitterDistance = Width / 5;
            UpdateDebug("当前文件夹路径 " + folder_path);
            splitContainer_Rtxs.Panel2Collapsed = true;
            splitContainer_Rtxs.Panel2.Hide();
        }

        public void SaveConfig()
        {
            XmlDocument doc = new XmlDocument();
            string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            doc.Load(strFileName);
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 1; i < nodes.Count; i++)
            {
                XmlAttribute att = nodes[i].Attributes["key"];
                switch (att.Value)
                {
                    case "FolderPath":
                        att = nodes[i].Attributes["value"];
                        att.Value = folder_path;
                        break;
                    case "FontName":
                        att = nodes[i].Attributes["value"];
                        att.Value = font_name;
                        break;
                    case "FontSize":
                        att = nodes[i].Attributes["value"];
                        att.Value = font_size.ToString();
                        break;
                    case "ForeColor":
                        att = nodes[i].Attributes["value"];
                        att.Value = forecolor;
                        break;
                    case "BackColor":
                        att = nodes[i].Attributes["value"];
                        att.Value = backcolor;
                        break;
                    case "WordWrap":
                        att = nodes[i].Attributes["value"];
                        att.Value = 自动换行WToolStripMenuItem.Checked.ToString();
                        break;
                    case "ExtendContent":
                        att = nodes[i].Attributes["value"];
                        att.Value = extend_file_content.ToString();
                        break;
                    case "ExtendPairDocument":
                        att = nodes[i].Attributes["value"];
                        att.Value = extend_pair_document.ToString();
                        break;
                    case "GenerateSeperator":
                        att = nodes[i].Attributes["value"];
                        att.Value = generate_separator.ToString();
                        break;
                    case "DecimalPlace":
                        att = nodes[i].Attributes["value"];
                        att.Value = decimal_place.ToString();
                        break;
                }
            }
            doc.Save(strFileName);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Frm_Main_Resize(object sender, EventArgs e)
        {
            AutoResize();
            LoadFileToTreeView();
        }

        private void UpdateDebug(string text, bool next_line = true)
        {
            if (Rtx_Debug.Text == "")
            {
                Rtx_Debug.AppendText(text);
            }
            else
            {
                Rtx_Debug.AppendText((next_line ? "\n" : " ") + text);
            }
            // Debug
            //Rtx_Debug.AppendText(temporty_text[0].ToString() + " " + temporty_text[1].ToString());
        }

        private void Load_File_Property(byte type = 0, string path = null)
        {
            Rtxs_property[target_tab].Clear();
            switch (type)
            {
                case 0:
                    Rtxs_property[target_tab].AppendText("文件名称 " + TreeView.SelectedNode.Text);
                    Rtxs_property[target_tab].AppendText("\n文件路径 " + full_name[treeview_selected_index]);
                    f_full_name[target_tab] = full_name[treeview_selected_index];
                    Rtxs_property[target_tab].AppendText("\n创建时间 " + create_time[treeview_selected_index]);
                    f_create_time[target_tab] = create_time[treeview_selected_index];
                    Rtxs_property[target_tab].AppendText("\n修改时间 " + last_access_time[treeview_selected_index]);
                    f_last_access_time[target_tab] = last_access_time[treeview_selected_index];
                    Rtxs_property[target_tab].AppendText("\n文件大小 " + file_length[treeview_selected_index]);
                    f_file_length[target_tab] = file_length[treeview_selected_index];
                    break;
                case 1:
                    FileInfo f = new FileInfo(path);
                    Rtxs_property[target_tab].AppendText("文件名称 " + Tabcontrols[target_tab].TabPages[0].Text);
                    Rtxs_property[target_tab].AppendText("\n文件路径 " + f.FullName);
                    f_full_name[target_tab] = f.FullName;
                    Rtxs_property[target_tab].AppendText("\n创建时间 " + f.CreationTime);
                    f_create_time[target_tab] = f.CreationTime;
                    Rtxs_property[target_tab].AppendText("\n修改时间 " + f.LastAccessTime);
                    f_last_access_time[target_tab] = f.LastAccessTime;
                    Rtxs_property[target_tab].AppendText("\n文件大小 " + f.Length);
                    f_file_length[target_tab] = f.Length;
                    break;
                case 2:
                    Tabcontrols[target_tab].TabPages[1].Text = "临时文件 属性";
                    Rtxs_property[target_tab].AppendText("创建时间 " + DateTime.Now.ToString());
                    break;
            }
        }

        private void CheckTabControl(byte type = 0)
        {
            if (type == 0)
            {
                if (first_select_file)
                {
                    TabControl_File.Visible = true;
                    first_select_file = false;
                }
                if (!Tabcontrols[target_tab].Visible)
                {
                    Tabcontrols[target_tab].Visible = true;
                }
                if (!first_select_file)
                {
                    StatusStrip_RAC.Visible = true;
                    保存SToolStripMenuItem.Enabled = true;
                    另存为AToolStripMenuItem.Enabled = true;
                    删除ToolStripMenuItem.Enabled = true;
                    查找FToolStripMenuItem.Enabled = true;
                    替换RToolStripMenuItem.Enabled = true;
                    撤销UToolStripMenuItem.Enabled = true;
                    重做RToolStripMenuItem.Enabled = true;
                    剪切TToolStripMenuItem.Enabled = true;
                    复制CToolStripMenuItem.Enabled = true;
                    粘贴PToolStripMenuItem.Enabled = true;
                    删除DToolStripMenuItem1.Enabled = true;
                    全选AToolStripMenuItem.Enabled = true;
                    复制当前文件全部内容ToolStripMenuItem.Enabled = true;
                    复制当前文件名ToolStripMenuItem.Enabled = true;
                    复制当前文件路径ToolStripMenuItem.Enabled = true;
                    复制当前目录路径ToolStripMenuItem.Enabled = true;
                    转成大写ToolStripMenuItem.Enabled = true;
                    转成小写ToolStripMenuItem.Enabled = true;
                    移除行尾空格ToolStripMenuItem.Enabled = true;
                    移除行首空格ToolStripMenuItem.Enabled = true;
                    移除行首和行尾空格ToolStripMenuItem.Enabled = true;
                    空格转换为换行符ToolStripMenuItem.Enabled = true;
                    换行符转换为空格ToolStripMenuItem.Enabled = true;
                    打开文件ToolStripMenuItem.Enabled = true;
                    文件资源管理器中打开ToolStripMenuItem.Enabled = true;
                    生成数据ToolStripMenuItem.Enabled = true;
                    文件预览窗口WToolStripMenuItem.Enabled = true;
                    文件属性窗口PToolStripMenuItem.Enabled = true;
                    新建水平文档组HToolStripMenuItem.Enabled = true;
                    新建垂直文档组VToolStripMenuItem.Enabled = true;
                    新建单文档SToolStripMenuItem.Enabled = true;
                }
            }
            else if (type == 1)
            {
                StatusStrip_RAC.Visible = false;
                保存SToolStripMenuItem.Enabled = false;
                另存为AToolStripMenuItem.Enabled = false;
                删除ToolStripMenuItem.Enabled = false;
                查找FToolStripMenuItem.Enabled = false;
                替换RToolStripMenuItem.Enabled = false;
                撤销UToolStripMenuItem.Enabled = false;
                重做RToolStripMenuItem.Enabled = false;
                剪切TToolStripMenuItem.Enabled = false;
                复制CToolStripMenuItem.Enabled = false;
                粘贴PToolStripMenuItem.Enabled = false;
                删除DToolStripMenuItem1.Enabled = false;
                全选AToolStripMenuItem.Enabled = false;
                复制当前文件全部内容ToolStripMenuItem.Enabled = false;
                复制当前文件名ToolStripMenuItem.Enabled = false;
                复制当前文件路径ToolStripMenuItem.Enabled = false;
                复制当前目录路径ToolStripMenuItem.Enabled = false;
                转成大写ToolStripMenuItem.Enabled = false;
                转成小写ToolStripMenuItem.Enabled = false;
                移除行尾空格ToolStripMenuItem.Enabled = false;
                移除行首空格ToolStripMenuItem.Enabled = false;
                移除行首和行尾空格ToolStripMenuItem.Enabled = false;
                空格转换为换行符ToolStripMenuItem.Enabled = false;
                换行符转换为空格ToolStripMenuItem.Enabled = false;
                打开文件ToolStripMenuItem.Enabled = false;
                文件资源管理器中打开ToolStripMenuItem.Enabled = false;
                生成数据ToolStripMenuItem.Enabled = false;
                文件预览窗口WToolStripMenuItem.Enabled = false;
                文件属性窗口PToolStripMenuItem.Enabled = false;
                新建水平文档组HToolStripMenuItem.Enabled = false;
                新建垂直文档组VToolStripMenuItem.Enabled = false;
                新建单文档SToolStripMenuItem.Enabled = false;
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                CheckTabControl();
                changed_times[target_tab] = 0;
                treeview_selected_index = TreeView.SelectedNode.Index;
                current_file_name = TreeView.SelectedNode.Text;
                UpdateDebug("正在打开文件 " + full_name[TreeView.SelectedNode.Index]);
                Tabcontrols[target_tab].TabPages[0].Text = current_file_name;
                Tabcontrols[target_tab].TabPages[1].Text = current_file_name + " 属性";
                Encoding f_coding = GetType(full_name[treeview_selected_index]);
                StreamReader sr = new StreamReader(full_name[treeview_selected_index], f_coding);
                Rtxs_file[target_tab].Text = sr.ReadToEnd();
                sr.Close();
                Load_File_Property();
                UpdateDebug("-> 成功打开文件", false);
                Rtxs_file[target_tab].Focus();
                Rtxs_file[target_tab].SelectionStart = 0;
                if (Pn_Welcome.Visible)
                {
                    HideWelcomePanel();
                }
                Rtx_Debug_TextChanged(this, new EventArgs());
            }
            catch
            {
                UpdateDebug("-> 打开文件失败", false);
            }
        }

        private void splitContainer2_Panel1_SizeChanged(object sender, EventArgs e)
        {
            AutoResize();
        }

        private void Frm_Main_Activated(object sender, EventArgs e)
        {
            LoadFileToTreeView();
            TreeView.Focus();
            if (change_folder_path)
            {
                UpdateDebug("当前文件夹路径 " + folder_path);
                change_folder_path = false;
            }
        }

        private void 另存为SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog.Filter = "文本文档|*.txt";
            SaveFileDialog.FileName = temporty_text[target_tab] ? "新建临时文件" : TabControl_File.TabPages[0].Text;
            if (SaveFileDialog.ShowDialog() == DialogResult.OK && SaveFileDialog.FileName.Length > 0)
            {
                FileStream fileStream = new FileStream(SaveFileDialog.FileName, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.Write(Rtxs_file[target_tab].Text);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
                if (temporty_text[target_tab])
                {
                    Tabcontrols[target_tab].TabPages[1].Text = Tabcontrols[target_tab].TabPages[1].Text.Substring(0, Tabcontrols[target_tab].TabPages[1].Text.Length - save_tag_length);
                    text_changed[target_tab] = false;
                    changed_times[target_tab] = 1;
                    // Go to commom document
                    OpenFile(SaveFileDialog.FileName);
                    temporty_text[target_tab] = false;
                    LoadFileToTreeView();
                }
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowFARDialog(int type = 0)
        {
            FAR_type = type;
            Frm_FAR frm_FAR = new Frm_FAR();
            frm_FAR.Show();
        }

        private void 查找FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFARDialog();
        }

        private void 替换RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFARDialog(1);
        }

        private void 撤销UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEditTarget().Undo();
        }

        private void 重做RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEditTarget().Redo();
        }

        private RichTextBox GetEditTarget()
        {
            RichTextBox rtb = new RichTextBox();
            if (Rtx_Solstice.Focused)
            {
                rtb = Rtx_Solstice;
            }
            else if (Rtxs_file[0].Focused)
            {
                rtb = Rtxs_file[0];
            }
            else if (Rtxs_file[1].Focused)
            {
                rtb = Rtxs_file[1];
            }
            return rtb;
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEditTarget().Cut();
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(GetEditTarget().SelectedText, true);
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
            {
                Clipboard.SetDataObject(new RichTextBox
                {
                    Font = new Font(font_name, font_size),
                    ForeColor = ColorTranslator.FromHtml(forecolor),
                    BackColor = ColorTranslator.FromHtml(backcolor),
                    Text = Clipboard.GetText()
                }.Text, true); // Reset text format
                if ((target_rtx == 0 || target_rtx == 2) && !Rtx_Solstice.Focused)
                {
                    Rtxs[target_rtx].Paste();
                }
                if (Rtx_Solstice.Focused)
                {
                    Rtx_Solstice.Paste();
                }
            }
        }

        private void 删除DToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked && !Rtx_Solstice.Focused)
            {
                Rtxs_file[target_tab].SelectedText = "";
            }
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetEditTarget().SelectAll();
        }

        private void 复制当前文件路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(f_full_name[target_tab], true);
        }

        private void 复制当前文件名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Tabcontrols[target_tab].TabPages[0].Text, true);
        }

        private void 复制当前目录路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(f_full_name[target_tab].Substring(0, f_full_name[target_tab].LastIndexOf('\\') + 1));
        }

        private void 复制当前文件全部内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Rtxs[target_rtx].Text, true);
            UpdateDebug("已复制当前文件(" + (f_full_name[target_tab] ?? "临时文件") + ")全部内容到剪贴板");
        }

        private void 调试窗口PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            调试窗口PToolStripMenuItem.Checked = !调试窗口PToolStripMenuItem.Checked;
            if (!调试窗口PToolStripMenuItem.Checked)
            {
                splitContainer_Debug.Panel2Collapsed = true;
                splitContainer_Debug.Panel2.Hide();
            }
            else
            {
                splitContainer_Debug.Panel2Collapsed = false;
                splitContainer_Debug.Panel2.Show();
            }
        }

        private void 选项OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Option frm_Option = new Frm_Option();
            frm_Option.ShowDialog();
        }

        private void 文件预览窗口WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tabcontrols[target_tab].SelectedIndex = 0;
        }

        private void 文件属性窗口PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tabcontrols[target_tab].SelectedIndex = 1;
        }

        private void 关于柒幻霜降AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_About frm_About = new Frm_About();
            frm_About.ShowDialog();
        }

        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            Point ClickPoint = new Point(e.X, e.Y);
            TreeNode CurrentNode = TreeView.GetNodeAt(ClickPoint);
            if (CurrentNode != null)
            {
                TreeView.SelectedNode = CurrentNode;
                treeview_selected_index = TreeView.SelectedNode.Index;
                current_file_name = TreeView.SelectedNode.Text;
                temporty_text[target_tab] = false;
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        TreeView_AfterSelect(this, new TreeViewEventArgs(CurrentNode));
                        break;
                    case MouseButtons.Right:
                        CurrentNode.ContextMenuStrip = ContextMenuStrip_Treeview;
                        break;
                }
            }
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView_AfterSelect(this, new TreeViewEventArgs(TreeView.SelectedNode));
        }

        private void 在文件资源管理器中打开文件夹EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", directory_name[treeview_selected_index]);
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                return;
            }
            DialogResult result = MessageBox.Show("确认删除" + full_name[treeview_selected_index] + "？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string remove_name = full_name[treeview_selected_index];
                if (f_full_name[0] == remove_name && f_full_name[1] == remove_name)
                {
                    first_show_pair_doc = true;
                    ShowPairDoc(0);
                    CheckTabControl(1);
                }
                else
                {
                    新建单文档SToolStripMenuItem_Click(this, new EventArgs());
                    CheckTabControl(1);
                }
                Tabcontrols[target_tab].Visible = false;
                TreeView.Nodes[treeview_selected_index].Remove();
                File.Delete(remove_name);
                //full_name.RemoveAt(treeview_selected_index);
                LoadFileToTreeView();
                Pn_Welcome.Visible = true;
            }
        }

        private void 重命名MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode != null)
            {
                current_file_name = TreeView.SelectedNode.Text;
                Frm_Rename frm_Rename = new Frm_Rename();
                frm_Rename.ShowDialog();
                TreeView.SelectedNode = TreeView.Nodes[treeview_selected_index];
                TreeView_AfterSelect(this, new TreeViewEventArgs(TreeView.SelectedNode));
            }
        }

        private void CheckFolderPath()
        {
            try
            {
                if (!Directory.Exists(folder_path))
                {
                    folder_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    XmlDocument doc = new XmlDocument();
                    string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    doc.Load(strFileName);
                    XmlNodeList nodes = doc.GetElementsByTagName("add");
                    for (int i = 1; i < nodes.Count; i++)
                    {
                        XmlAttribute att = nodes[i].Attributes["key"];
                        if (att.Value == "FolderPath")
                        {
                            att = nodes[i].Attributes["value"];
                            att.Value = folder_path;
                        }
                    }
                    doc.Save(strFileName);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch { }
        }

        private void Frm_Main_Shown(object sender, EventArgs e)
        {
            CheckFolderPath();
            Tx_Search.Focus();
        }

        private void SetListSearch()
        {
            List_Search.Visible = true;
            List_Search.Location = new Point(0, Flpn_Search.Top + Flpn_Search.Height);
            List_Search.Size = new Size(Flpn_Search.Width, (List_Search.Items.Count + 1) * List_Search.ItemHeight);
        }

        private void Rtx_Search_TextChanged(object sender, EventArgs e)
        {
            if (Tx_Search.Text == "")
            {
                List_Search.Visible = false;
                return;
            }
            List_Search.Items.Clear();
            search_item_index.Clear();
            char[] s = Tx_Search.Text.ToCharArray();
            bool[] show_item = new bool[TreeView.Nodes.Count];
            for (int i = 0; i < TreeView.Nodes.Count; i++)
            {
                for (int j = 0; j < s.Length; j++)
                {
                    if (TreeView.Nodes[i].Text.Contains(Tx_Search.Text))
                    {
                        if (!show_item[i])
                        {
                            show_item[i] = true;
                            List_Search.Items.Add(TreeView.Nodes[i].Text);
                            search_item_index.Add(i);
                        }
                        SetListSearch();
                    }
                }
            }
        }

        private void Rtx_Search_Enter(object sender, EventArgs e)
        {
            Rtx_Search_TextChanged(this, new EventArgs());
        }

        private void Rtx_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (!List_Search.Visible)
                return;
            switch (e.KeyValue)
            {
                case 40: // Down Arrow
                    if (List_Search.SelectedIndex != List_Search.Items.Count - 1)
                    {
                        List_Search.SelectedIndex++;
                    }
                    else
                    {
                        List_Search.SelectedIndex = 0;
                    }
                    break;
                case 38: // Up Arrow
                    if (List_Search.SelectedIndex != 0)
                    {
                        List_Search.SelectedIndex--;
                    }
                    else
                    {
                        List_Search.SelectedIndex = List_Search.Items.Count - 1;
                    }
                    break;
                case 13: // Enter
                    SelectTreeNodefromListSearch();
                    break;
            }
        }

        private void SelectTreeNodefromListSearch()
        {
            TreeView.SelectedNode = TreeView.Nodes[search_item_index[List_Search.SelectedIndex]];
            TreeView_AfterSelect(this, new TreeViewEventArgs(TreeView.SelectedNode));
            List_Search.Visible = false;
        }

        private void List_Search_Click(object sender, EventArgs e)
        {
            SelectTreeNodefromListSearch();
        }

        private void List_Search_MouseEnter(object sender, EventArgs e)
        {
            List_Search.Visible = true;
        }

        private void List_Search_MouseLeave(object sender, EventArgs e)
        {
            List_Search.Visible = false;
        }

        private void Rtx_Search_Click(object sender, EventArgs e)
        {
            if (search_item_index.Count != 0)
                List_Search.Visible = true;
        }

        private void 打开OToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog
            {
                Title = "打开文件",
                Filter = "文本文档|*.txt",
                FileName = "", 
            };
            if (opendlg.ShowDialog() == DialogResult.OK && opendlg.FileName.Length > 0)
            {
                HideWelcomePanel();
                try
                {
                    CheckTabControl();
                    OpenFile(opendlg.FileName);
                    temporty_text[target_tab] = false;
                }
                catch { }
            }
            opendlg.Dispose();
        }

        private void TabControl_File_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TabControl_File_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            OpenFile(path);
        }

        private void OpenFile(string path, bool temp = false)
        {
            if (!temp)
            {
                try
                {
                    FileInfo f = new FileInfo(path);
                    Tabcontrols[target_tab].TabPages[0].Text = f.Name.Substring(0, f.Name.IndexOf('.'));
                    Tabcontrols[target_tab].TabPages[1].Text = Tabcontrols[target_tab].TabPages[0].Text + " 属性";
                    open_file[target_tab] = true;
                    Load_File_Property(1, path);
                    UpdateDebug("正在打开文件 " + path);
                    Encoding f_coding = GetType(path);
                    StreamReader sr = new StreamReader(path, f_coding);
                    Rtxs_file[target_tab].Text = sr.ReadToEnd();
                    sr.Close();
                    UpdateDebug("-> 成功打开文件", false);
                    Rtx_File.Focus();
                }
                catch
                {
                    UpdateDebug("-> 打开文件失败", false);
                }
            }
            else
            {
                Tabcontrols[target_tab].TabPages[0].Text = "临时文件";
                UpdateDebug("新建临时文件");
            }
        }

        private void OpenFileFromTreeView(string path)
        {
            try
            {
                FileInfo f = new FileInfo(path);
                Tabcontrols[target_tab].TabPages[0].Text = f.Name.Substring(0, f.Name.IndexOf('.'));
                Tabcontrols[target_tab].TabPages[1].Text = Tabcontrols[target_tab].TabPages[0].Text + " 属性";
                open_file[target_tab] = true;
                Load_File_Property(1, path);
                UpdateDebug("正在打开文件 " + full_name[TreeView.SelectedNode.Index]);
                Encoding f_coding = GetType(path);
                StreamReader sr = new StreamReader(path, f_coding);
                Rtxs_file[target_tab].Text = sr.ReadToEnd();
                sr.Close();
                UpdateDebug("-> 成功打开文件", false);
                Rtx_File.Focus();
            }
            catch
            {
                // Display debug
                //UpdateDebug("-> 打开文件失败", false);
            }
        }

        private void ShowPairDoc(int type)
        {
            doc_view_type = type;
            target_tab = 0;
            TabControl_File_2.Visible = true;
            switch (type)
            {
                case 0:
                    splitContainer_Rtxs.Panel2Collapsed = true;
                    splitContainer_Rtxs.Panel2.Hide();
                    Rtx_File.Focus();
                    break;
                case 1:
                    if (splitContainer_Rtxs.Panel2Collapsed)
                    {
                        temporty_text[1] = temporty_text[0];
                    }
                    splitContainer_Rtxs.Panel2Collapsed = false;
                    splitContainer_Rtxs.Panel2.Show();
                    splitContainer_Rtxs.Orientation = Orientation.Horizontal;
                    splitContainer_Rtxs.SplitterDistance = splitContainer_Rtxs.Height / 2;
                    break;
                case 2:
                    if (splitContainer_Rtxs.Panel2Collapsed)
                    {
                        temporty_text[1] = temporty_text[0];
                    }
                    splitContainer_Rtxs.Panel2Collapsed = false;
                    splitContainer_Rtxs.Panel2.Show();
                    splitContainer_Rtxs.Orientation = Orientation.Vertical;
                    splitContainer_Rtxs.SplitterDistance = splitContainer_Rtxs.Width / 2;
                    break;
            }
            ActiveDoc(0);
            // Debug
            //Rtx_Debug.AppendText(temporty_text[0].ToString() + " " + temporty_text[1].ToString());
        }

        private void 新建垂直文档组VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPairDoc(2);
            if (first_show_pair_doc)
            {
                target_tab = 1;
                if (!temporty_text[target_tab])
                {
                    OpenFileFromTreeView(f_full_name[0]);
                }
                else
                {
                    if (extend_pair_document)
                    {
                        Rtxs_file[1].Text = Rtxs_file[0].Text;
                    }
                }
                target_tab = 0;
                first_show_pair_doc = false;
            }
        }

        private void ActiveDoc(int target)
        {
            if (doc_view_type != 0)
            {
                switch (target)
                {
                    case 0:
                        splitContainer_Rtxs.Panel1.BackColor = Color.FromArgb(153, 180, 209);
                        splitContainer_Rtxs.Panel2.BackColor = Color.FromArgb(240, 240, 240);
                        break;
                    case 1:
                        splitContainer_Rtxs.Panel1.BackColor = Color.FromArgb(240, 240, 240);
                        splitContainer_Rtxs.Panel2.BackColor = Color.FromArgb(153, 180, 209);
                        break;
                }
            }
            else
            {
                splitContainer_Rtxs.Panel1.BackColor = Color.FromArgb(240, 240, 240);
                splitContainer_Rtxs.Panel2.BackColor = Color.FromArgb(240, 240, 240);
            }
        }

        private void ShowRank(RichTextBox rtb)
        {
            int index = rtb.GetFirstCharIndexOfCurrentLine();
            int line = rtb.GetLineFromCharIndex(index) + 1;
            int column = rtb.SelectionStart - index + 1;
            StatusStrip_RAC.Text = string.Format("行：{0}  列：{1}", line.ToString(), column.ToString());
        }

        private void Rtx_File_Click(object sender, EventArgs e)
        {
            target_rtx = 0;
            ActiveDoc(target_tab = 0);
            ShowRank(Rtx_File);
            GetSampleSizeFromSelectedRtx();
        }

        private void Rtx_Property_Click(object sender, EventArgs e)
        {
            target_rtx = 1;
            ActiveDoc(target_tab = 0);
        }

        private void Rtx_File_2_Click(object sender, EventArgs e)
        {
            target_rtx = 2;
            ActiveDoc(target_tab = 1);
            ShowRank(Rtx_File_2);
            GetSampleSizeFromSelectedRtx();
        }

        private void Rtx_Property_2_Click(object sender, EventArgs e)
        {
            target_rtx = 3;
            ActiveDoc(target_tab = 1);
        }

        private void 新建水平文档组HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPairDoc(1);
            if (first_show_pair_doc)
            {
                target_tab = 1;
                if (!temporty_text[target_tab])
                {
                    OpenFileFromTreeView(f_full_name[0]);
                }
                else
                {
                    if (extend_pair_document)
                    {
                        Rtxs_file[1].Text = Rtxs_file[0].Text;
                    }
                }
                target_tab = 0;
                first_show_pair_doc = false;
            }
        }

        private void TabControl_File_2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TabControl_File_2_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            OpenFileFromTreeView(path);
        }

        private void 新建单文档SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPairDoc(0);
            ActiveDoc(0);
            Rtxs_file[1].Text = "";
            first_show_pair_doc = true;
        }

        private void GetSampleSizeFromSelectedRtx()
        {
            int count = Rtxs_file[target_tab].Text.Split(separators.ToArray()).Count();
            Tx_Data_Sample_Capacity.Text = count.ToString();
            Nud_Data_Sample_Size.Maximum = count;
        }

        private void Rtx_File_TextChanged(object sender, EventArgs e)
        {
            Rtx_File_Click(this, new EventArgs());
            if (!open_file[target_tab])
            {
                if (changed_times[0] == 2)
                {
                    return;
                }
                if (changed_times[0] != 1)
                {
                    changed_times[0]++;
                    return;
                }
                if (changed_times[0] == 1)
                {
                    text_changed[0] = true;
                    TabControl_File.TabPages[1].Text += "（未保存）";
                    changed_times[0]++;
                }
            }
            if (open_file[target_tab])
            {
                open_file[target_tab] = false;
            }
        }

        private void Rtx_File_2_TextChanged(object sender, EventArgs e)
        {
            Rtx_File_2_Click(this, new EventArgs());
            if (!open_file[target_tab])
            {
                if (changed_times[1] == 2)
                {
                    return;
                }
                if (changed_times[1] != 1)
                {
                    changed_times[1]++;
                    return;
                }
                if (changed_times[1] == 1)
                {
                    text_changed[1] = true;
                    TabControl_File_2.TabPages[1].Text += "（未保存）";
                    changed_times[1]++;
                }
            }
            if (open_file[target_tab])
            {
                open_file[target_tab] = false;
            }
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text_changed[target_tab] || (!text_changed[target_tab] && temporty_text[target_tab]))
            {
                if (!temporty_text[target_tab])
                {
                    FileStream fileStream = new FileStream(f_full_name[target_tab], FileMode.Create);
                    StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                    streamWriter.Write(Rtxs_file[target_tab].Text);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fileStream.Close();
                    Tabcontrols[target_tab].TabPages[1].Text = Tabcontrols[target_tab].TabPages[1].Text.Substring(0, Tabcontrols[target_tab].TabPages[1].Text.Length - save_tag_length);
                    text_changed[target_tab] = false;
                    changed_times[target_tab] = 1;
                }
                else
                {
                    另存为SToolStripMenuItem_Click(this, new EventArgs());
                }
            }
        }

        private void 自动换行WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自动换行WToolStripMenuItem.Checked = !自动换行WToolStripMenuItem.Checked;
            foreach (RichTextBox rtx in Rtxs)
            {
                rtx.WordWrap = 自动换行WToolStripMenuItem.Checked;
            }
            Rtx_Solstice.WordWrap = Rtx_Output.WordWrap = 自动换行WToolStripMenuItem.Checked;
            SaveConfig();
        }

        private void splitContainer2_Panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void splitContainer2_Panel2_DragDrop(object sender, DragEventArgs e)
        {
            TabControl_File.Visible = true;
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            OpenFile(path);
            Pn_Welcome.Visible = false;
            first_select_file = false;
            CheckTabControl();
        }

        private void 转成大写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked)
                Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.ToUpper();
        }

        private void 转成小写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked)
                Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.ToLower();
        }

        private void 移除行尾空格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked)
                Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.TrimEnd();
        }

        private void 移除行首空格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked)
                Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.TrimStart();
        }

        private void 移除行首和行尾空格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!设为只读ToolStripMenuItem.Checked)
                Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.Trim();
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fp = Rtxs[target_rtx].SelectedText;
            if (File.Exists(fp))
            {
                OpenFile(fp);
            }
            else
            {
                MessageBox.Show("文件不存在", "错误");
            }
        }

        private void 文件资源管理器中打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dp = Rtxs[target_rtx].SelectedText;
            if (Directory.Exists(dp))
            {
                System.Diagnostics.Process.Start("explorer.exe", dp);
            }
            else
            {
                MessageBox.Show("文件夹不存在", "错误");
            }
        }

        private void 设为只读ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            设为只读ToolStripMenuItem.Checked = !设为只读ToolStripMenuItem.Checked;
            Rtxs_file[0].ReadOnly = 设为只读ToolStripMenuItem.Checked;
            Rtxs_file[1].ReadOnly = 设为只读ToolStripMenuItem.Checked;
        }

        private void 复制CToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StringCollection copy_file_path = new StringCollection
            {
                full_name[treeview_selected_index]
            };
            Clipboard.SetFileDropList(copy_file_path);
        }

        private void 复制到文件夹DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string copy_file_name = full_name[treeview_selected_index].Substring(0, full_name[treeview_selected_index].LastIndexOf('.')) + " - 副本.txt";
            FileStream fileStream = new FileStream(copy_file_name, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.Write(Rtxs_file[target_tab].Text);
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
            LoadFileToTreeView();
        }

        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (TreeView.SelectedNode != null)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (e.KeyValue == 68)
                    {
                        复制到文件夹DToolStripMenuItem_Click(this, new EventArgs());
                    }
                }
            }
        }

        private void 清空调试窗口内容DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rtx_Debug.Text = "";
        }

        private void 添加DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Add frm_add = new Frm_Add();
            frm_add.ShowDialog();
            if (file_name_from_add != "")
            {
                OpenFile(file_name_from_add);
                HideWelcomePanel();
            }
            Tabcontrols.ForEach(p => p.Visible = true);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            删除DToolStripMenuItem_Click(this, new EventArgs());
        }

        private void 工具栏TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            工具栏TToolStripMenuItem.Checked = !工具栏TToolStripMenuItem.Checked;
            toolStrip.Visible = 工具栏TToolStripMenuItem.Checked;
        }

        private void Ts_Add_Click(object sender, EventArgs e)
        {
            if (添加DToolStripMenuItem.Enabled)
            {
                添加DToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Open_Click(object sender, EventArgs e)
        {
            if (打开OToolStripMenuItem1.Enabled)
            {
                打开OToolStripMenuItem1_Click(this, new EventArgs());
            }
        }

        private void Ts_Save_Click(object sender, EventArgs e)
        {
            if (保存SToolStripMenuItem.Enabled)
            {
                保存SToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Cut_Click(object sender, EventArgs e)
        {
            if (剪切TToolStripMenuItem.Enabled)
            {
                剪切TToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Copy_Click(object sender, EventArgs e)
        {
            if (复制CToolStripMenuItem.Enabled)
            {
                复制CToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Paste_Click(object sender, EventArgs e)
        {
            if (粘贴PToolStripMenuItem.Enabled)
            {
                粘贴PToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Delete_Click(object sender, EventArgs e)
        {
            if (删除DToolStripMenuItem1.Enabled)
            {
                删除DToolStripMenuItem1_Click(this, new EventArgs());
            }
        }

        private void Ts_Search_Click(object sender, EventArgs e)
        {
            if (查找FToolStripMenuItem.Enabled)
            {
                查找FToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Replace_Click(object sender, EventArgs e)
        {
            if (替换RToolStripMenuItem.Enabled)
            {
                替换RToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Undo_Click(object sender, EventArgs e)
        {
            if (撤销UToolStripMenuItem.Enabled)
            {
                撤销UToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void Ts_Redo_Click(object sender, EventArgs e)
        {
            if (重做RToolStripMenuItem.Enabled)
            {
                重做RToolStripMenuItem_Click(this, new EventArgs());
            }
        }

        private void ShowPnFromMenu(int target)
        {
            if (target == last_pn_menu_target) return;
            Pn_menu.ForEach(p => p.Visible = false);
            Pn_menu[target].Visible = true;
            last_pn_menu_target = target;
            AutoResize();
        }

        private void Btn_Menu1_File_Click(object sender, EventArgs e)
        {
            ShowPnFromMenu(0);
        }

        private void Btn_Menu1_Formula_Click(object sender, EventArgs e)
        {
            ShowPnFromMenu(1);
        }

        private void Btn_Menu1_Data_Click(object sender, EventArgs e)
        {
            ShowPnFromMenu(2);
        }

        private void Btn_Menu1_Solstice_Click(object sender, EventArgs e)
        {
            ShowPnFromMenu(3);
            Rtx_Solstice.Focus();
        }

        private void ShowFormulaFromMenu(int target)
        {
            Pn_formula_controls[target].Visible = !Pn_formula_controls[target].Visible;
        }

        private void Btn_Formula_Sum_Click(object sender, EventArgs e)
        {
            ShowFormulaFromMenu(0);
        }

        private void Btn_Formula_Statistics_Click(object sender, EventArgs e)
        {
            ShowFormulaFromMenu(1);
        }

        private void Btn_Formula_Math_Click(object sender, EventArgs e)
        {
            ShowFormulaFromMenu(2);
        }

        private void Btn_Formula_Matrix_Click(object sender, EventArgs e)
        {
            ShowFormulaFromMenu(3);
        }

        private void 输出窗口OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            输出窗口OToolStripMenuItem.Checked = !输出窗口OToolStripMenuItem.Checked;
            splitContainer_Rtx_Output.Panel2Collapsed = !输出窗口OToolStripMenuItem.Checked;
        }

        private void ResetOutputRtx()
        {
            splitContainer_Rtx_Output.Panel2.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void ErrorOutput()
        {
            error_output = true;
            splitContainer_Rtx_Output.Panel2.BackColor = Color.FromArgb(214, 116, 116);
        }

        private List<double> GetListFromOutput()
        {
            string[] strs = Rtx_Output.Text.Split(separators.ToArray());
            double[] values;
            try
            {
                values = Array.ConvertAll<string, double>(strs, s => double.Parse(s));
            }
            catch
            {
                values = new double[1] { 0 };
            }
            return values.ToList();
        }

        private List<double> GetListFromSelectedRtx(int target = -1)
        {
            ResetOutputRtx();
            target = target == -1 ? target_tab : target;
            string[] strs = Rtxs_file[target].Text.Split(separators.ToArray());
            double[] values;
            List<double> res = new List<double>
            {
                0
            };
            try
            {
                values = Array.ConvertAll<string, double>(strs, s => double.Parse(s));
                res = values.ToList();
            }
            catch
            {
                ErrorOutput();
            }
            return res;
        }

        private List<string> GetStringListFromSelectRtx(int target = -1)
        {
            ResetOutputRtx();
            target = target == -1 ? target_tab : target;
            string[] strs = Rtxs_file[target].Text.Split(separators.ToArray());
            return strs.ToList();
        }

        private void ReportError(string error_str = "输入错误")
        {
            VisualResult();
            splitContainer_Rtx_Output.Panel2.BackColor = Color.FromArgb(214, 116, 116);
            TabControl_Output.TabPages[0].Text = "输出窗口";
            Rtx_Output.Text = error_str;
            UpdateDebug("输出错误 -> " + error_str);
            error_output = false;
        }

        private void VisualResult()
        {
            if (splitContainer_Rtx_Output.Panel2Collapsed)
            {
                splitContainer_Rtx_Output.Panel2Collapsed = false;
                splitContainer_Rtx_Output.SplitterDistance = splitContainer_Rtx_Output.Height / 2;
                输出窗口OToolStripMenuItem.Checked = !splitContainer_Rtx_Output.Panel2Collapsed;
            }
        }

        private string GetDecimalPlace()
        {
            return "G" + decimal_place.ToString();
        }

        private void DisplaySingleResult(string str, double value)
        {
            if (!error_output)
            {
                ResetOutputRtx();
                TabControl_Output.TabPages[0].Text = "输出 " + str;
                Rtx_Output.Text = value.ToString(GetDecimalPlace());
                UpdateDebug("输出 " + str + " -> " + value.ToString(GetDecimalPlace()));
            }
            else
            {
                ReportError();
            }
            VisualResult();
        }

        private void DisplayMultipleResults(string str, List<double> list)
        {
            if (!error_output)
            {
                ResetOutputRtx();
                TabControl_Output.TabPages[0].Text = "输出 " + str;
                Rtx_Output.Text = "";
                list.ForEach(p => Rtx_Output.AppendText(p.ToString(GetDecimalPlace()) + " "));
                Rtx_Output.Text = Rtx_Output.Text.TrimEnd();
                UpdateDebug("输出 " + str);
            }
            else
            {
                ReportError();
            }
            VisualResult();
        }

        private void DisplayMultipleStringResults(string str, List<string> list)
        {
            if (!error_output)
            {
                ResetOutputRtx();
                TabControl_Output.TabPages[0].Text = "输出 " + str;
                Rtx_Output.Text = "";
                list.ForEach(p => Rtx_Output.AppendText(p + " "));
                Rtx_Output.Text = Rtx_Output.Text.TrimEnd();
                UpdateDebug("输出 " + str);
            }
            else
            {
                ReportError();
            }
            VisualResult();
        }

        private void DisplayMultipleResults2(string str, string value)
        {
            if (!error_output)
            {
                ResetOutputRtx();
                TabControl_Output.TabPages[0].Text = "输出 " + str;
                Rtx_Output.Text = value.ToString();
                //UpdateDebug("输出 " + str + " -> " + value);
                UpdateDebug("输出 " + str);
            }
            else
            {
                ReportError();
            }
            VisualResult();
        }

        private void Btn_Formula_MaxValue_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("最大值", GetListFromSelectedRtx().Max());
        }

        private void Btn_Formula_MinValue_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("最小值", GetListFromSelectedRtx().Min());
        }

        private void Btn_Formula_Sum_Sum_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("求和", GetListFromSelectedRtx().Sum());
        }

        private void Btn_Formula_Statistics_Range_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("极差", GetListFromSelectedRtx().Max() - GetListFromSelectedRtx().Min());
        }

        private void Btn_Formula_Statistics_Average_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("平均值", GetListFromSelectedRtx().Average());
        }

        private void Btn_Formula_Statistics_GeometricMean_Click(object sender, EventArgs e)
        {
            double res = 1;
            List<double> arr = GetListFromSelectedRtx();
            arr.ForEach(p => res *= p);
            res = Math.Pow(res, 1.0 / arr.Count);
            DisplaySingleResult("几何平均值", res);
        }

        private void Btn_Formula_Statistics_HarmonicMean_Click(object sender, EventArgs e)
        {
            double res = 0;
            List<double> arr = GetListFromSelectedRtx();
            if (!arr.Contains(0))
            {
                arr.ForEach(p => res += 1.0 / p);
                res = arr.Count / res;
            }
            DisplaySingleResult("调和平均值", res);
        }

        private void Btn_Formula_Statistics_Median_Click(object sender, EventArgs e)
        {
            double res;
            List<double> arr = GetListFromSelectedRtx();
            arr.Sort();
            int n = arr.Count;
            if (n % 2 == 0)
            {
                res = (arr[n / 2 - 1] + arr[n / 2]) / 2;
            }
            else
            {
                res = arr[n / 2];
            }
            DisplaySingleResult("中位值", res);
        }

        private void Btn_Formula_Statistics_Mode_Click(object sender, EventArgs e)
        {
            List<double> arr = GetListFromSelectedRtx();
            var mostPresentTimes = arr.Distinct().Max(i => arr.Count(j => j == i));
            var mostPresent = arr.Distinct().Where(i => arr.Count(j => j == i) == mostPresentTimes);
            var mpx = mostPresent.Select(i => i.ToString()).Aggregate((accu, next) => $"{accu} {next}");
            string[] mpx_x = mpx.ToString().Split(' ');
            DisplayMultipleResults2("众数（出现次数：" + arr.Count(p => p == Convert.ToDouble(mpx_x[0])) + "）", mpx);
        }

        private double Variance(int type = 0)
        {
            List<double> arr = GetListFromSelectedRtx();
            int n = arr.Count;
            double avg = arr.Average(), t = 0, res = 0;
            if (n > 0)
            {
                arr.ForEach(p => t += Math.Pow(p - avg, 2));
                switch (type)
                {
                    case 0:
                        res = t / n;
                        break;
                    case 1:
                        res = t / (n - 1);
                        break;
                }
            }
            return res;
        }

        private void Btn_Formula_Statistics_PSDV_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("总体标准差的方差", Variance());
        }

        private void Btn_Formula_Statistics_SSDV_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("样本标准差的方差", Variance(1));
        }

        private void Btn_Formula_Statistics_PSD_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("总体标准差", Math.Sqrt(Variance()));
        }

        private void Btn_Formula_Statistics_SSD_Click(object sender, EventArgs e)
        {
            DisplaySingleResult("样本标准差", Math.Sqrt(Variance(1)));
        }

        private void Btn_Formula_Statistics_CoefficientofVariation_Click(object sender, EventArgs e)
        {
            List<double> arr = GetListFromSelectedRtx();
            DisplaySingleResult("离散系数", Math.Sqrt(Variance()) / arr.Average());
        }

        private void Btn_Formula_Math_AbsoluteValue_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("绝对值", GetListFromSelectedRtx().Select(p => p = Math.Abs(p)).ToList());
        }

        private void Btn_Formula_Math_OppositeNumber_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("相反数", GetListFromSelectedRtx().Select(p => p = -p).ToList());
        }

        private void Btn_Formula_Math_Ceil_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("向上取整", GetListFromSelectedRtx().Select(p => p = Math.Ceiling(p)).ToList());
        }

        private void Btn_Formula_Math_Floor_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("向下取整", GetListFromSelectedRtx().Select(p => p = Math.Floor(p)).ToList());
        }

        private void Btn_Formula_Math_Round_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("四舍五入", GetListFromSelectedRtx().Select(p => p = Math.Round(p)).ToList());
        }

        public int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return GCD(b, a % b);
        }

        public int GCDN(int[] arr, int n)
        {
            if (n == 1)
                return arr[0];
            else
                return GCD(arr[n - 1], GCDN(arr, n - 1));
        }

        private void Btn_Formula_Math_GCD_Click(object sender, EventArgs e)
        {
            try
            {
                int[] arr = Array.ConvertAll<double, int>(GetListFromSelectedRtx().ToArray(), p => Convert.ToInt32(p));
                DisplaySingleResult("最大公约数", GCDN(arr, arr.Length));
            }
            catch
            {
                ReportError("无法计算");
            }
        }

        private int LCMN(int[] arr, int n)
        {
            int res = arr[0];
            for (int a = 1, b; a < n; a++)
            {
                b = GCD(res, arr[a]);
                res *= arr[a];
                res /= b;
            }
            return res;
        }

        private void Btn_Formula_Math_LCM_Click(object sender, EventArgs e)
        {
            try
            {
                int[] arr = Array.ConvertAll<double, int>(GetListFromSelectedRtx().ToArray(), p => Convert.ToInt32(p));
                DisplaySingleResult("最大公约数", LCMN(arr, arr.Length));
            }
            catch
            {
                ReportError("无法计算");
            }

        }

        private void Btn_Formula_Math_Sine_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("正弦值", GetListFromSelectedRtx().Select(p => p = Math.Sin(Math.PI * p / 180)).ToList());
        }

        private void Btn_Formula_Math_Cosine_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("余弦值", GetListFromSelectedRtx().Select(p => p = Math.Cos(Math.PI * p / 180)).ToList());
        }

        private void Btn_Formula_Math_Tangent_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("正切值", GetListFromSelectedRtx().Select(p => p = Math.Tan(Math.PI * p / 180)).ToList());
        }

        public double[,] GetMatrixFromTargetRtx(RichTextBox rtx)
        {
            string txt = rtx.Text;
            is_matrix = true;
            int row_count = Regex.Matches(txt, "\n").Count + 1;
            string[] rows = txt.Split(Environment.NewLine.ToCharArray());
            int[] element_count_in_rows = new int[rows.Count()];
            for (int i = 0; i < rows.Count(); i++)
            {
                string[] element_in_row = rows[i].Split(' ');
                element_count_in_rows[i] += element_in_row.Count();
            }
            for (int i = 1; i < element_count_in_rows.Count(); i++)
            {
                if (element_count_in_rows[i] != element_count_in_rows[i - 1])
                {
                    is_matrix = false;
                    break;
                }
            }
            int column_count = element_count_in_rows[0];
            string[] elements_row_string = txt.Split('\n');
            string elements_string = "";
            foreach (string str in elements_row_string)
            {
                elements_string += str;
                elements_string += ' ';
            }
            string[] element_s = elements_string.Split(' ');
            double[] elements = new double[element_s.Length - 1];
            try
            {
                for (int i = 0; i < element_s.Length - 1; i++)
                {
                    elements[i] = Convert.ToDouble(element_s[i]);
                }
            }
            catch { }
            double[,] matrix = new double[row_count, column_count];
            int p = 0, q = 0;
            try
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    matrix[p, q] = elements[i];
                    if ((i + 1) % column_count == 0)
                    {
                        p += 1;
                        q = -1;
                    }
                    q += 1;
                }
            }
            catch
            {
                ErrorOutput();
            }
            return matrix;
        }

        private void DisplayMatrixResult(string str, double[,] matrix, bool error = false, string error_str = "输入错误")
        {
            if (!error_output && is_matrix && !error)
            {
                ResetOutputRtx();
                TabControl_Output.TabPages[0].Text = "输出 " + str;
                Rtx_Output.Text = "";
                int col = matrix.GetLength(1), p = 0, q = 0;
                for (int i = 0; i < matrix.Length; i++)
                {
                    Rtx_Output.AppendText(matrix[p, q++].ToString(GetDecimalPlace()) + " ");
                    if ((i + 1) % col == 0)
                    {
                        Rtx_Output.Text = Rtx_Output.Text.TrimEnd();
                        Rtx_Output.AppendText("\n");
                        p++;
                        q = 0;
                    }
                }
                Rtx_Output.Text = Rtx_Output.Text.Remove(Rtx_Output.Text.Length - 1, 1);
                UpdateDebug("输出 " + str);
            }
            else
            {
                ReportError(error_str);
            }
            VisualResult();
        }

        private void Btn_Formula_Matrix_Sum_Click(object sender, EventArgs e)
        {
            double[,] matrix1 = GetMatrixFromTargetRtx(Rtxs_file[0]);
            double[,] matrix2 = GetMatrixFromTargetRtx(Rtxs_file[1]);
            int row1 = matrix1.GetLength(0), col1 = matrix1.GetLength(1);
            int row2 = matrix2.GetLength(0), col2 = matrix2.GetLength(1);
            if (!(row1 == row2 && col1 == col2) || matrix1 == null || matrix2 == null)
            {
                ErrorOutput();
                DisplayMatrixResult("矩阵加法", null, true, "两矩阵无法相加");
                return;
            }
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col1; j++)
                {
                    matrix1[i, j] += matrix2[i, j];
                }
            }
            DisplayMatrixResult("矩阵加法", matrix1);
        }

        private void Btn_Formula_Matrix_Multiply1_Click(object sender, EventArgs e)
        {
            double[,] matrix1 = GetMatrixFromTargetRtx(Rtxs_file[0]);
            double[,] matrix2 = GetMatrixFromTargetRtx(Rtxs_file[1]);
            int row1 = matrix1.GetLength(0), col1 = matrix1.GetLength(1);
            int row2 = matrix2.GetLength(0), col2 = matrix2.GetLength(1);
            if (col1 == row2)
            {
                if (row1 == 1 && col2 == 1)
                {
                    double value = 0;
                    for (int i = 0; i < matrix1.GetLength(1); i++)
                    {
                        value += matrix1[0, i] * matrix2[i, 0];
                    }
                    double[,] matrix42 = new double[1, 1];
                    matrix42[0, 0] = value;
                    DisplayMatrixResult("矩阵乘法（矩阵乘矩阵）", matrix42);
                    return;
                }
                else if (col1 == 1 && row2 == 1)
                {
                    double[,] matrix41 = new double[row1, col2];
                    for (int i = 0; i < row1; i++)
                    {
                        for (int j = 0; j < col2; j++)
                        {
                            matrix41[i, j] = matrix1[i, 0] * matrix2[0, j];
                        }
                    }
                    DisplayMatrixResult("矩阵乘法（矩阵乘矩阵）", matrix41);
                    return;
                }
                double[,] matrix4 = new double[row1, col2];
                for (int i = 0; i < row1; i++)
                {
                    for (int j = 0; j < col2; j++)
                    {
                        for (int p = 0; p < col1; p++)
                        {
                            matrix4[i, j] += matrix1[i, p] * matrix2[p, j];
                        }
                    }
                }
                DisplayMatrixResult("矩阵乘法（矩阵乘矩阵）", matrix4);
                return;
            }
            else
            {
                ErrorOutput();
                DisplayMatrixResult("矩阵乘法（矩阵乘矩阵）", null, true, "两矩阵无法相乘");
            }
        }

        private void Btn_Formula_Matrix_Multiply2_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[1]);
            if (!Rtxs_file[0].Visible)
            {
                ErrorOutput();
                DisplayMatrixResult("矩阵乘法（数乘矩阵）", null, true, "数和矩阵无法相乘");
                return;
            }
            double k = Rtxs_file[0].Text != "" ? Convert.ToDouble(Rtxs_file[0].Text.Split(separators.ToArray())[0]) : 0;
            int row1 = matrix.GetLength(0), col1 = matrix.GetLength(1);
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col1; j++)
                {
                    matrix[i, j] *= k;
                }
            }
            DisplayMatrixResult("矩阵乘法（数乘矩阵）", matrix);
        }

        private void Btn_Formula_Matrix_Trace_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
            int row = matrix.GetLength(0), col = matrix.GetLength(1);
            double trace = 0;
            for (int i = 0; i < Math.Min(row, col); i++)
            {
                trace += matrix[i, i];
            }
            DisplaySingleResult("矩阵的迹", trace);
        }

        private void Btn_Formula_Matrix_Transpose_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
            int row = matrix.GetLength(0), column = matrix.GetLength(1);
            double[,] res_matrix = new double[column, row];
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    res_matrix[i, j] = matrix[j, i];
                }
            }
            DisplayMatrixResult("矩阵的转置", res_matrix);
        }

        private void Btn_Formula_Matrix_Square_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
                int row = matrix.GetLength(0), col = matrix.GetLength(1);
                double[,] res = new double[row, col];
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        for (int p = 0; p < col; p++)
                        {
                            res[i, j] += matrix[i, p] * matrix[p, j];
                        }
                    }
                }
                DisplayMatrixResult("矩阵的平方", res);
            }
            catch
            {
                ReportError("无法计算");
            }
        }

        private int GetMatrixRank(int i, int j, double[,] matrix)
        {
            int n = Math.Max(i, j), m = Math.Min(i, j), now = 0;
            double[,] res = new double[n, m];
            double stp;
            for (int a = 0; a < n; a++)
            {
                for (int b = 0; b < m; b++)
                {
                    res[a, b] = (i > j ? matrix[a, b] : matrix[b, a]);
                }
            }
            for (int a = 0; a < m; a++)
            {
                if (Math.Abs(res[now, a]) < 1e-5)
                {
                    for (int b = now + 1; b < n; b++)
                    {
                        if (Math.Abs(res[b, a]) > 1e-5)
                        {
                            for (int c = a; c < m; c++)
                            {
                                stp = res[a, c];
                                res[a, c] = res[b, c];
                                res[b, c] = stp;
                            }
                            break;
                        }
                    }
                }
                if (Math.Abs(res[now, a]) > 1e-5)
                {
                    for (int b = now + 1; b < n; b++)
                    {
                        if (Math.Abs(res[b, a]) > 1e-5)
                        {
                            stp = res[b, a] / res[now, a];
                            for (int c = a; c < m; c++)
                            {
                                res[b, c] = res[b, c] - res[now, c] * stp;
                            }
                        }
                    }
                    now++;
                }
            }
            return now;
        }


        private void Btn_Formula_Matrix_Rank_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
            int row = matrix.GetLength(0), col = matrix.GetLength(1);
            DisplaySingleResult("矩阵的秩", GetMatrixRank(row, col, matrix));
        }

        private void Btn_Formula_Matrix_Inverse_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
            int n = matrix.GetLength(0), col = matrix.GetLength(1);
            if (n != col)
            {
                ErrorOutput();
                DisplayMatrixResult("逆矩阵", null, true, "矩阵的行数和列数不相等");
                return;
            }
            double[,] now = new double[n, n];
            double[,] res = new double[n, n];
            double stp;
            for (int a = 0; a < n; a++)
            {
                for (int b = 0; b < n; b++)
                {
                    now[a, b] = matrix[a, b];
                    res[a, b] = (a == b ? 1 : 0);
                }
            }
            for (int a = 0; a < n; a++)
            {
                if (Math.Abs(now[a, a]) < 1e-5)
                {
                    for (int b = a + 1; b < n;)//b++)
                    {
                        if (Math.Abs(now[b, a]) > 1e-5)
                        {
                            for (int c = a; c < n; c++)
                            {
                                stp = now[a, c];
                                now[a, c] = now[b, c];
                                now[b, c] = stp;
                            }
                            for (int c = 0; c < n; c++)
                            {
                                stp = res[a, c];
                                res[a, c] = res[b, c];
                                res[b, c] = stp;
                            }
                        }
                        break;
                    }
                    DisplayMultipleResults2("逆矩阵", "逆矩阵不存在");
                    return;
                }
                for (int b = a + 1; b < n; b++)
                {
                    if (Math.Abs(now[b, a]) > 1e-5)
                    {
                        stp = now[b, a] / now[a, a];
                        for (int c = a; c < n; c++) now[b, c] = now[b, c] - now[a, c] * stp;
                        for (int c = 0; c < n; c++) res[b, c] = res[b, c] - res[a, c] * stp;
                    }
                }
            }
            for (int a = n - 1; a > -1; a--)
            {
                for (int b = 0; b < n; b++)
                {
                    res[a, b] = res[a, b] / now[a, a];
                }
                now[a, a] = 1;
                for (int b = a - 1; b > -1; b--)
                {
                    for (int c = 0; c < n; c++)
                    {
                        res[b, c] = res[b, c] - res[a, c] * now[b, a];
                    }
                    now[b, a] = 0;
                }
            }
            DisplayMatrixResult("逆矩阵", res);
        }

        private double[,] UptriMatrix(int n, double[,] matrix)
        {
            int now = 0;
            double[,] res = new double[n + 1, n + 1];
            double stp = new double();
            for (int a = 0; a < n; a++)
            {
                for (int b = 0; b < n; b++)
                {
                    res[a, b] = matrix[a, b];
                }
            }
            res[n, n] = 1;
            for (int a = 0; a < n; a++)
            {
                if (Math.Abs(res[now, a]) < 1e-5)
                {
                    for (int b = now + 1; b < n; b++)
                    {
                        if (Math.Abs(res[b, a]) > 1e-5)
                        {
                            for (int c = a; c < n; c++)
                            {
                                stp = res[a, c];
                                res[a, c] = res[b, c];
                                res[b, c] = stp;
                            }
                            res[n, n] *= -1;
                            break;
                        }
                    }
                }
                if (Math.Abs(res[now, a]) > 1e-5)
                {
                    for (int b = now + 1; b < n; b++)
                    {
                        if (Math.Abs(res[b, a]) > 1e-5)
                        {
                            stp = res[b, a] / res[now, a];
                            for (int c = a; c < n; c++)
                            {
                                res[b, c] = res[b, c] - res[now, c] * stp;
                            }
                        }
                    }
                    now++;
                }
            }
            for (int a = 0; a < n; a++)
            {
                res[n, n] = res[n, n] * res[a, a];
            }
            return res;
        }

        private void Btn_Formula_Matrix_Value_Click(object sender, EventArgs e)
        {
            double[,] matrix = GetMatrixFromTargetRtx(Rtxs_file[target_tab]);
            int row = matrix.GetLength(0), col = matrix.GetLength(1);
            if (row != col)
            {
                ErrorOutput();
                DisplayMatrixResult("行列式的值", null, true, "矩阵的行数和列数不相等");
                return;
            }
            matrix = UptriMatrix(row, matrix);
            double res = 1;
            for (int i = 0; i < row; i++)
            {
                res *= matrix[i, i];
            }
            DisplaySingleResult("行列式的值", res);
        }

        private void 清空输出窗口内容DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rtx_Output.Text = "";
        }

        private void 复制输出窗口内容CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Rtx_Output.Text, true);
        }

        private void 复制输出窗口内容到选中窗口TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Tabcontrols[0].Visible)
            {
                CreateTemporaryDocument();
                Pn_Welcome.Visible = false;
            }
            Rtxs_file[target_tab].Text = Rtx_Output.Text;
        }

        private void 输出窗口内容另存为SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog.Filter = "文本文档|*.txt";
            SaveFileDialog.FileName = "输出窗口";
            if (SaveFileDialog.ShowDialog() == DialogResult.OK && SaveFileDialog.FileName.Length > 0)
            {
                FileStream fileStream = new FileStream(SaveFileDialog.FileName, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.Write(Rtx_Output.Text);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }
        }

        private void Combo_Data_Generate_GenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Combo_Data_Generate_DataType.Items.Clear();
            switch (Combo_Data_Generate_GenerateType.SelectedIndex)
            {
                case 0:
                    Combo_Data_Generate_DataType.Items.Add("数字");
                    Combo_Data_Generate_DataType.Items.Add("字母");
                    break;
                case 1:
                    Combo_Data_Generate_DataType.Items.Add("字符串");
                    Combo_Data_Generate_DataType.Items.Add("数列");
                    Combo_Data_Generate_DataType.Items.Add("矩阵");
                    break;
            }
            Combo_Data_Generate_DataType.SelectedIndex = 0;
        }

        private void ShowDataFromMenu(int target)
        {
            Pn_data_controls[target].Visible = !Pn_data_controls[target].Visible;
        }

        private void ShowDataGenerateFromData(int target)
        {
            Pn_data_generate_controls.ForEach(p => p.Visible = false);
            Pn_data_generate_controls[target].Visible = true;
        }

        private void Combo_Data_Generate_DataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Combo_Data_Generate_DataType.SelectedIndex)
            {
                case 0:
                    switch (Combo_Data_Generate_GenerateType.SelectedIndex)
                    {
                        case 0:
                            ShowDataGenerateFromData(0);
                            Lbl_Data_Generate_3.Text = "最小值";
                            Lbl_Data_Generate_4.Text = "最大值";
                            Tx_Data_Generate_MinValue.Text = "1";
                            Tx_Data_Generate_MaxValue.Text = "100";
                            break;
                        case 1:
                            ShowDataGenerateFromData(1);
                            break;
                    }
                    break;
                case 1:
                    switch (Combo_Data_Generate_GenerateType.SelectedIndex)
                    {
                        case 0:
                            ShowDataGenerateFromData(0);
                            Lbl_Data_Generate_3.Text = "起始字符";
                            Lbl_Data_Generate_4.Text = "终止字符";
                            Tx_Data_Generate_MinValue.Text = "a";
                            Tx_Data_Generate_MaxValue.Text = "z";
                            break;
                        case 1:
                            ShowDataGenerateFromData(2);
                            Combo_Data_Generate_ArrayType.SelectedIndex = 0;
                            break;
                    }
                    break;
                case 2:
                    ShowDataGenerateFromData(3);
                    Combo_Data_Generate_MatrixType.SelectedIndex = 0;
                    break;
            }
        }

        private void Btn_Data_Generate_Click(object sender, EventArgs e)
        {
            ShowDataFromMenu(0);
        }

        private bool IsNumber(string str)
        {
            Regex rx = new Regex("^[0-9]*$");
            return str != "" && (rx.IsMatch(str) || (str[0] == '-' && rx.IsMatch(str.Substring(1))));
        }

        private bool IsChar(string str)
        {
            Regex rx = new Regex("^[a-z]*$|^[A-Z]*$");
            return str != "" && str.Length == 1 && rx.IsMatch(str);
        }

        private void Btn_Data_Generate_Confirm_Click(object sender, EventArgs e)
        {
            string res = "";
            switch (Combo_Data_Generate_GenerateType.SelectedIndex)
            {
                case 0:
                    Random ran = new Random();
                    switch (Combo_Data_Generate_DataType.SelectedIndex)
                    {
                        case 0:
                            if (!IsNumber(Tx_Data_Generate_MinValue.Text))
                            {
                                ReportError("最小值输入错误");
                                return;
                            }
                            if (!IsNumber(Tx_Data_Generate_MaxValue.Text))
                            {
                                ReportError("最大值输入错误");
                                return;
                            }
                            int min_value;
                            int max_value;
                            try
                            {
                                min_value = Convert.ToInt32(Tx_Data_Generate_MinValue.Text);
                                max_value = Convert.ToInt32(Tx_Data_Generate_MaxValue.Text);
                            }
                            catch
                            {
                                ReportError("无法生成");
                                return;
                            }
                            if (min_value > max_value)
                            {
                                ReportError("最小值大于最大值");
                                return;
                            }
                            if (Nud_Data_Generate_RandomRepeat.Text == "")
                            {
                                ReportError("重复次数输入错误");
                                return;
                            }
                            for (int i = 0; i < Nud_Data_Generate_RandomRepeat.Value; i++)
                            {
                                res += ran.Next(min_value, max_value + 1).ToString() + generate_separator;
                            }
                            res = res.Remove(res.Length - 1, 1);
                            DisplayMultipleResults2("随机生成数字", res);
                            break;
                        case 1:
                            if (!IsChar(Tx_Data_Generate_MinValue.Text))
                            {
                                ReportError("起始字符输入错误");
                                return;
                            }
                            if (!IsChar(Tx_Data_Generate_MaxValue.Text))
                            {
                                ReportError("终止字符输入错误");
                                return;
                            }
                            int start_char = Tx_Data_Generate_MinValue.Text.ToCharArray()[0];
                            int end_char = Tx_Data_Generate_MaxValue.Text.ToCharArray()[0];
                            if (start_char > end_char)
                            {
                                ReportError("起始字符大于终止字符");
                                return;
                            }
                            if (Nud_Data_Generate_RandomRepeat.Text == "")
                            {
                                ReportError("重复次数输入错误");
                                return;
                            }
                            for (int i = 0; i < Nud_Data_Generate_RandomRepeat.Value; i++)
                            {
                                res += Convert.ToChar(ran.Next(start_char, end_char + 1));
                                res += Convert.ToChar(generate_separator);
                            }
                            res = res.Remove(res.Length - 1, 1);
                            DisplayMultipleResults2("随机生成字符", res);
                            break;
                    }
                    break;
                case 1:
                    switch (Combo_Data_Generate_DataType.SelectedIndex)
                    {
                        case 0:
                            if (Nud_Data_Generate_StringRepeat.Text == "")
                            {
                                ReportError("重复次数输入错误");
                                return;
                            }
                            for (int i = 0; i < Nud_Data_Generate_StringRepeat.Value; i++)
                            {
                                res += Tx_Data_Generate_String.Text + generate_separator;
                            }
                            res = res.Remove(res.Length - 1, 1);
                            DisplayMultipleResults2("生成指定字符串", res);
                            break;
                        case 1:
                            switch (Combo_Data_Generate_ArrayType.SelectedIndex)
                            {
                                case 0:
                                    if (!IsNumber(Tx_Data_Generate_InitValue.Text))
                                    {
                                        ReportError("初值输入错误");
                                        return;
                                    }
                                    if (!IsNumber(Tx_Data_Generate_Step.Text))
                                    {
                                        ReportError("步长输入错误");
                                        return;
                                    }
                                    if (Nud_Data_Generate_ArrayRepeat.Text == "")
                                    {
                                        ReportError("重复次数输入错误");
                                        return;
                                    }
                                    double init_val1 = Convert.ToDouble(Tx_Data_Generate_InitValue.Text);
                                    double step1 = Convert.ToDouble(Tx_Data_Generate_Step.Text);
                                    init_val1 -= step1;
                                    for (int i = 0; i < Nud_Data_Generate_ArrayRepeat.Value; i++)
                                    {
                                        res += (init_val1 += step1).ToString() + generate_separator;
                                    }
                                    res = res.Remove(res.Length - 1, 1);
                                    DisplayMultipleResults2("生成等差数列", res);
                                    break;
                                case 1:
                                    if (!IsNumber(Tx_Data_Generate_InitValue.Text))
                                    {
                                        ReportError("初值输入错误");
                                        return;
                                    }
                                    if (!IsNumber(Tx_Data_Generate_Step.Text))
                                    {
                                        ReportError("步长输入错误");
                                        return;
                                    }
                                    if (Convert.ToDouble(Tx_Data_Generate_Step.Text) == 0)
                                    {
                                        ReportError("步长不能为0");
                                        return;
                                    }
                                    if (Nud_Data_Generate_ArrayRepeat.Text == "")
                                    {
                                        ReportError("重复次数输入错误");
                                        return;
                                    }
                                    double init_val2 = Convert.ToDouble(Tx_Data_Generate_InitValue.Text);
                                    double step2 = Convert.ToDouble(Tx_Data_Generate_Step.Text);
                                    for (int i = 0; i < Nud_Data_Generate_ArrayRepeat.Value; i++)
                                    {
                                        if (i == 0)
                                        {
                                            res += init_val2.ToString() + generate_separator;
                                        }
                                        else
                                        {
                                            res += (init_val2 *= step2).ToString() + generate_separator;
                                        }
                                    }
                                    res = res.Remove(res.Length - 1, 1);
                                    DisplayMultipleResults2("生成等比数列", res);
                                    break;
                            }
                            break;
                        case 2:
                            switch (Combo_Data_Generate_MatrixType.SelectedIndex)
                            {
                                case 0:
                                    if (!IsNumber(Tx_Data_Generate_MatrixRow.Text))
                                    {
                                        ReportError("行数输入错误");
                                        return;
                                    }
                                    if (Check_Data_Generate_MatrixSize.Checked && !IsNumber(Tx_Data_Generate_MatrixCol.Text))
                                    {
                                        ReportError("列数输入错误");
                                        return;
                                    }
                                    if (!IsNumber(Tx_Data_Generate_MatrixVal.Text))
                                    {
                                        ReportError("值输入错误");
                                        return;
                                    }
                                    int row1;
                                    try
                                    {
                                        row1 = Convert.ToInt32(Tx_Data_Generate_MatrixRow.Text);
                                    }
                                    catch
                                    {
                                        ReportError("无法生成");
                                        return;
                                    }
                                    int col1;
                                    double val1 = Convert.ToDouble(Tx_Data_Generate_MatrixVal.Text);
                                    if (Check_Data_Generate_MatrixSize.Checked)
                                    {
                                        col1 = row1;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            col1 = Convert.ToInt32(Tx_Data_Generate_MatrixCol.Text);
                                        }
                                        catch
                                        {
                                            ReportError("无法生成");
                                            return;
                                        }
                                    }
                                    double[,] matrix1 = new double[row1, col1];
                                    for (int i = 0; i < row1; i++)
                                    {
                                        for (int j = 0; j < col1; j++)
                                        {
                                            matrix1[i, j] = val1;
                                        }
                                    }
                                    is_matrix = true;
                                    DisplayMatrixResult("生成矩阵", matrix1);
                                    break;
                                case 1:
                                    if (!IsNumber(Tx_Data_Generate_MatrixRow.Text))
                                    {
                                        ReportError("行数输入错误");
                                        return;
                                    }
                                    int row2 = Convert.ToInt32(Tx_Data_Generate_MatrixRow.Text);
                                    double[,] matrix2 = new double[row2, row2];
                                    for (int i = 0; i < row2; i++)
                                    {
                                        for (int j = 0; j < row2; j++)
                                        {
                                            matrix2[i, j] = i == j ? 1 : 0;
                                        }
                                    }
                                    is_matrix = true;
                                    DisplayMatrixResult("生成单位矩阵", matrix2);
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void Check_Data_Generate_MatrixSize_CheckedChanged(object sender, EventArgs e)
        {
            Pn_Data_Generate_Controls_6_4.Visible = !Check_Data_Generate_MatrixSize.Checked;
        }

        private void Combo_Data_Generate_MatrixType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Combo_Data_Generate_MatrixType.SelectedIndex)
            {
                case 0:
                    Pn_Data_Generate_Controls_6_5.Visible = true;
                    Check_Data_Generate_MatrixSize.Enabled = true;
                    break;
                case 1:
                    Pn_Data_Generate_Controls_6_5.Visible = false;
                    Check_Data_Generate_MatrixSize.Checked = true;
                    Check_Data_Generate_MatrixSize.Enabled = false;
                    break;
            }
        }

        //private List<double> RandomSampling(double[] nsp, int n, int m, bool repeat)
        //{
        //    Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
        //    List<double> res = new List<double>();
        //    int[] id = new int[n];
        //    if (repeat)
        //    {
        //        for (int a = 0; a < m; a++)
        //        {
        //            res.Add(nsp[rand.Next(n)]);
        //        }
        //    }
        //    else
        //    {
        //        for (int a = 0; a < n; a++) id[a] = a;
        //        for (int a = 0, b; a < m; a++)
        //        {
        //            b = rand.Next(n);
        //            res.Add(nsp[id[b]]);
        //            id[b] = id[--n];
        //        }
        //    }
        //    return res;
        //}

        private List<string> RandomSampling(List<string> nsp, int n, int m, bool repeat)
        {
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            List<string> res = new List<string>();
            int[] id = new int[n];
            if (repeat)
            {
                for (int a = 0; a < m; a++)
                {
                    res.Add(nsp[rand.Next(n)]);
                }
            }
            else
            {
                for (int a = 0; a < n; a++) id[a] = a;
                for (int a = 0, b; a < m; a++)
                {
                    b = rand.Next(n);
                    res.Add(nsp[id[b]]);
                    id[b] = id[--n];
                }
            }
            return res;
        }

        private void Btn_Data_Sample_Confirm_Click(object sender, EventArgs e)
        {
            List<string> arr = GetStringListFromSelectRtx();
            if (Nud_Data_Sample_Size.Text == "")
            {
                ReportError("抽取数量输入错误");
                return;
            }
            try
            {
                List<string> res = RandomSampling(arr, Convert.ToInt32(Tx_Data_Sample_Capacity.Text), Convert.ToInt32(Nud_Data_Sample_Size.Value), Check_Data_Sample_SampleRepeat.Checked);
                DisplayMultipleStringResults("随机抽样", res);
            }
            catch
            {
                ReportError();
            }
        }

        private void Btn_Data_Sample_Click(object sender, EventArgs e)
        {
            ShowDataFromMenu(1);
        }

        private void Btn_Data_Sort_Click(object sender, EventArgs e)
        {
            ShowDataFromMenu(2);
        }

        private void Btn_Data_Sort_Aescend_Click(object sender, EventArgs e)
        {
            List<double> arr = GetListFromSelectedRtx();
            arr.Sort();
            DisplayMultipleResults("升序排序", arr);
        }

        private void Btn_Data_Sort_Descend_Click(object sender, EventArgs e)
        {
            List<double> arr = GetListFromSelectedRtx();
            arr.Sort();
            arr.Reverse();
            DisplayMultipleResults("升序排序", arr);
        }

        private void Btn_Data_Sort_Reverse_Click(object sender, EventArgs e)
        {
            List<double> arr = GetListFromSelectedRtx();
            arr.Reverse();
            DisplayMultipleResults("逆序", arr);
        }

        private void Btn_Data_Sort_Chaos_Click(object sender, EventArgs e)
        {
            List<string> arr = GetStringListFromSelectRtx();
            arr = RandomSampling(arr, arr.Count, arr.Count, false);
            DisplayMultipleStringResults("乱序", arr);
        }

        private void Btn_Data_Sort_Distinct_Click(object sender, EventArgs e)
        {
            DisplayMultipleResults("删除重复", GetListFromSelectedRtx().Distinct().ToList());
        }

        private void Pn_Welcome_Resize(object sender, EventArgs e)
        {
            Pn_Welcome_Controls.Location = new Point(Pn_Welcome.Width / 2 - Pn_Welcome_Controls.Width / 2, Pn_Welcome.Height / 2 - Pn_Welcome_Controls.Height / 2);
        }

        private void CreateTemporaryDocument()
        {
            temporty_text[target_tab] = true;
            OpenFile("", true);
            Load_File_Property(2, "");
            HideWelcomePanel();
            TabControl_File.Visible = true;
            CheckTabControl();
            if (!extend_file_content)
            {
                Rtxs_file[target_tab].Text = "";
            }
            f_full_name[target_tab] = null;
        }

        private void HideWelcomePanel()
        {
            Pn_Welcome.Visible = false;
        }

        private void Btn_Welcome_New_Click(object sender, EventArgs e)
        {
            CreateTemporaryDocument();
        }

        private void 新建临时窗口NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTemporaryDocument();
        }

        private void Btn_Welcome_Add_Click(object sender, EventArgs e)
        {
            添加DToolStripMenuItem_Click(this, new EventArgs());
        }

        private void Btn_Welcome_Open_Click(object sender, EventArgs e)
        {
            打开OToolStripMenuItem1_Click(this, new EventArgs());
        }

        private void Ts_New_Click(object sender, EventArgs e)
        {
            CreateTemporaryDocument();
        }

        private void 编辑EToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (!Pn_Welcome.Visible)
            {
                复制当前文件路径ToolStripMenuItem.Enabled = !temporty_text[target_tab];
                复制当前文件名ToolStripMenuItem.Enabled = !temporty_text[target_tab];
                复制当前目录路径ToolStripMenuItem.Enabled = !temporty_text[target_tab];
            }
        }

        private void ReportSolsticeError(string str = "输入错误")
        {
            ReportError(str);
        }

        private void UpdateAnsByVal(double value)
        {
            ans = value.ToString(GetDecimalPlace());
        }

        private void UpdateAnsByList(List<double> list)
        {
            ans = "";
            list.ForEach(p => ans += p.ToString(GetDecimalPlace()) + " ");
            ans = ans.TrimEnd();
        }

        private void UpdateAnsByString(string str)
        {
            ans = str;
        }

        private void DisplaySolsticeResult(int target_result_rtx = 2)
        {
            if (!error_output)
            {
                switch (target_result_rtx)
                {
                    case 0: // to doc1
                        if (!Tabcontrols[0].Visible)
                        {
                            CreateTemporaryDocument();
                        }
                        Rtxs_file[0].Text = ans;
                        break;
                    case 1: // to doc2
                        if (!Tabcontrols[1].Visible)
                        {
                            新建垂直文档组VToolStripMenuItem_Click(this, new EventArgs());
                        }
                        else if (!Tabcontrols[0].Visible && !Tabcontrols[1].Visible)
                        {
                            CreateTemporaryDocument();
                            新建垂直文档组VToolStripMenuItem_Click(this, new EventArgs());
                        }
                        Rtxs_file[1].Text = ans;
                        break;
                    case 2: // to output
                        ResetOutputRtx();
                        TabControl_Output.TabPages[0].Text = "输出 Solstice";
                        Rtx_Output.Text = ans;
                        VisualResult();
                        break;
                }
                UpdateDebug("Solstice -> " + ans);
            }
            else
            {
                ReportError();
            }
        }

        public double Factorial(int num)
        {
            switch (num)
            {
                case 0:
                case 1:
                    return 1;
                default:
                    return num * Factorial(num - 1);
            }
        }

        private void StartDebugging(string str)
        {
            // string processing
            if (str == "")
            {
                ReportSolsticeError("数据输入不完整");
                return;
            }
            List<string> words = str.Split('\n').ToList();
            words.RemoveAll(p => p == "");
            if (words.Count < 2)
            {
                ReportSolsticeError("数据输入不完整");
                return;
            }
            for (int i = 0; i < words.Count; i++)
            {
                words[i] = words[i].TrimStart();
                words[i] = words[i].TrimEnd();
                words[i] = words[i].ToLower();
            }
            words[1] = words[1].Replace("pi", Math.PI.ToString()).Replace("ee", Math.E.ToString()).Replace("ans", ans);
            // set prefix and suffix
            List<string> prefix = new List<string>()
            {
                "sum", "prod", "exp", "pow", "perm", "comb", "mod", "sin", "cos", "tan", "fac", "abs", "sqrt", "ceil",
                "floor", "round", "max", "min", "gcd", "lcm", "sort", "select", "rnd", "count", "split"
            };
            List<string> prefix2 = new List<string>()
            {
                "from f1", "from f2", "from f", "from output"
            };
            List<string> suffix3 = new List<string>()
            {
                ">", "<", ">=", "=<", "=", "<>", "*", "by ", "into "
            };
            List<string> suffix2 = new List<string>()
            {
                "reverse", "no repeat", "distinct"
            };
            List<string> suffix = new List<string>()
            {
                "to f1", "to f2", "to f", "to output", "to clipboard"
            };
            // check prefix2 or data
            int prefix2_index = prefix2.IndexOf(words[1]); // -1 represent no prefix2 but data
            bool is_data = prefix2_index == -1; // true represent prefix2, false represent data
            // processing: data -> prefix -> suffixs
            // data processing
            List<double> data = new List<double>();
            if (is_data)
            {
                // get list from data
                string[] strs = words[1].Split(' ');
                try
                {
                    // data prefix processing
                    double[] values = Array.ConvertAll<string, double>(strs, s => double.Parse(s));
                    data = values.ToList();
                }
                catch
                {
                    ReportSolsticeError();
                    return;
                }
            }
            else
            {
                // get list from textbox
                switch (prefix2_index)
                {
                    case 0: // from f1
                        data = GetListFromSelectedRtx(0);
                        break;
                    case 1: // from f2
                        data = GetListFromSelectedRtx(1);
                        break;
                    case 2: // from f
                        data = GetListFromSelectedRtx();
                        break;
                    case 3: // from output
                        data = GetListFromOutput();
                        break;
                }
            }
            // prefix processing
            bool s_sort = false;
            bool active_sort = false;
            bool s_select = false;
            bool s_count = false;
            bool s_split = false;
            bool result_to_clipboard = false;
            List<double> pending_data = new List<double>();
            if (prefix.Contains(words[0]))
            {
                switch (words[0])
                {
                    case "sum":
                        UpdateAnsByVal(data.Sum());
                        break;
                    case "prod":
                        double product = 1;
                        data.ForEach(p => product *= p);
                        UpdateAnsByVal(product);
                        break;
                    case "pow":
                        if (data.Count > 1)
                        {
                            UpdateAnsByVal(Math.Pow(data[0], data[1]));
                        }
                        else
                        {
                            ReportSolsticeError("参数输入不完整");
                            return;
                        }
                        break;
                    case "exp":
                        UpdateAnsByList(data.Select(p => p = Math.Pow(Math.E, p)).ToList());
                        break;
                    case "perm":
                        if (data.Count > 1)
                        {
                            int d1 = Convert.ToInt32(data[0]);
                            int d2 = Convert.ToInt32(data[1]);
                            if (d1 >= d2)
                            {
                                UpdateAnsByVal(Factorial(d1) / Factorial(d1 - d2));
                            }
                            else
                            {
                                ReportSolsticeError();
                                return;
                            }
                        }
                        else
                        {
                            ReportSolsticeError("参数输入不完整");
                            return;
                        }
                        break;
                    case "comb":
                        if (data.Count > 1)
                        {
                            int d1 = Convert.ToInt32(data[0]);
                            int d2 = Convert.ToInt32(data[1]);
                            if (d1 >= d2)
                            {
                                UpdateAnsByVal(Factorial(d1) / (Factorial(d2) * Factorial(d1 - d2)));
                            }
                            else
                            {
                                ReportSolsticeError();
                                return;
                            }
                        }
                        else
                        {
                            ReportSolsticeError("参数输入不完整");
                            return;
                        }
                        break;
                    case "sin":
                        UpdateAnsByList(data.Select(p => p = Math.Sin(Math.PI * p / 180)).ToList());
                        break;
                    case "cos":
                        UpdateAnsByList(data.Select(p => p = Math.Cos(Math.PI * p / 180)).ToList());
                        break;
                    case "tan":
                        UpdateAnsByList(data.Select(p => p = Math.Tan(Math.PI * p / 180)).ToList());
                        break;
                    case "fac":
                        UpdateAnsByList(data.Select(p => Factorial(Convert.ToInt32(p))).ToList());
                        break;
                    case "abs":
                        UpdateAnsByList(data.Select(p => p = Math.Abs(p)).ToList());
                        break;
                    case "sqrt":
                        UpdateAnsByList(data.Select(p => p = Math.Sqrt(p)).ToList());
                        break;
                    case "ceil":
                        UpdateAnsByList(data.Select(p => p = Math.Ceiling(p)).ToList());
                        break;
                    case "floor":
                        UpdateAnsByList(data.Select(p => p = Math.Floor(p)).ToList());
                        break;
                    case "round":
                        UpdateAnsByList(data.Select(p => p = Math.Round(p)).ToList());
                        break;
                    case "mod":
                        if (data.Count > 1)
                        {
                            UpdateAnsByVal(data[0] % data[1]);
                        }
                        else
                        {
                            ReportSolsticeError("参数输入不完整");
                            return;
                        }
                        break;
                    case "max":
                        UpdateAnsByVal(data.Max());
                        break;
                    case "min":
                        UpdateAnsByVal(data.Min());
                        break;
                    case "gcd":
                        int[] gcd_data = data.ConvertAll(x => Convert.ToInt32(x)).ToArray();
                        UpdateAnsByVal(GCDN(gcd_data, data.Count));
                        break;
                    case "lcm":
                        int[] lcm_data = data.ConvertAll(x => Convert.ToInt32(x)).ToArray();
                        UpdateAnsByVal(LCMN(lcm_data, data.Count));
                        break;
                    case "sort":
                        s_sort = true;
                        data.Sort();
                        pending_data = data;
                        break;
                    case "select":
                        s_select = true;
                        pending_data = data;
                        break;
                    case "rnd":
                        if (data.Count > 2)
                        {
                            Random rnd = new Random();
                            int d1 = Convert.ToInt32(data[0]);
                            int d2 = Convert.ToInt32(data[1]);
                            int d3 = Convert.ToInt32(data[2]);
                            if (d1 < 0 || d2 > d3)
                            {
                                ReportSolsticeError();
                                return;
                            }
                            List<double> res = new List<double>();
                            for (int i = 0; i < d1; i++)
                            {
                                res.Add(rnd.Next(d2, d3 + 1));
                            }
                            UpdateAnsByList(res);
                        }
                        else
                        {
                            ReportSolsticeError("参数输入不完整");
                            return;
                        }
                        break;
                    case "count":
                        s_count = true;
                        pending_data = data;
                        break;
                    case "split":
                        s_split = true;
                        pending_data = data;
                        break;
                }
            }
            else
            {
                ReportSolsticeError("前缀输入有误");
                return;
            }
            // suffix processing
            words.RemoveAt(0);
            words.RemoveAt(0);// remove data and prefix, start processing suffixs
            // bug fix: list value to output
            if (words.Count == 0)
            {
                words.Add("to output");
            }
            int target_display_rtx = 2;
            List<string> suffixs = words;
            words.ForEach(word =>
            {
                if (s_select)
                {
                    if (word.Length > 2 && suffix3.Contains(word[0].ToString() + word[1].ToString()))
                    {
                        if (!IsNumber(word.Substring(2)))
                        {
                            error_output = true;
                            return;
                        }
                        double compare_value = Convert.ToDouble(word.Substring(2));
                        switch (word[0].ToString() + word[1].ToString())
                        {
                            case "*":
                                UpdateAnsByList(pending_data);
                                break;
                            case ">=":
                            //case "=>":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q >= compare_value).ToList());
                                break;
                            case "=<":
                            //case "<=":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q <= compare_value).ToList());
                                break;
                            case "<>":
                            //case "!=":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q != compare_value).ToList());
                                break;
                            default:
                                ReportSolsticeError();
                                return;
                        }
                    }
                    else if (word.Length > 1 && suffix3.Contains(word[0].ToString()))
                    {
                        if (!IsNumber(word.Substring(1)))
                        {
                            error_output = true;
                            return;
                        }
                        double compare_value = Convert.ToDouble(word.Substring(1));
                        switch (word[0].ToString())
                        {
                            case ">":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q > compare_value).ToList());
                                break;
                            case "<":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q < compare_value).ToList());
                                break;
                            case "=":
                                UpdateAnsByList(pending_data = pending_data.Where(q => q == compare_value).ToList());
                                break;
                            default:
                                ReportSolsticeError();
                                return;
                        }
                    }
                    else
                    {
                        UpdateAnsByList(pending_data);
                    }
                }
                if (s_count)
                {
                    if (word.Length > 2 && suffix3.Contains(word[0].ToString() + word[1].ToString()))
                    {
                        if (!IsNumber(word.Substring(2)))
                        {
                            error_output = true;
                            return;
                        }
                        double compare_value = Convert.ToDouble(word.Substring(2));
                        switch (word[0].ToString() + word[1].ToString())
                        {
                            case "*":
                                UpdateAnsByVal(pending_data.Count);
                                break;
                            case ">=":
                                //case "=>":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q >= compare_value).ToList()).Count);
                                break;
                            case "=<":
                                //case "<=":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q <= compare_value).ToList()).Count);
                                break;
                            case "<>":
                                //case "!=":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q != compare_value).ToList()).Count);
                                break;
                            default:
                                ReportSolsticeError();
                                return;
                        }
                    }
                    else if (word.Length > 1 && suffix3.Contains(word[0].ToString()))
                    {
                        if (!IsNumber(word.Substring(1)))
                        {
                            error_output = true;
                            return;
                        }
                        double compare_value = Convert.ToDouble(word.Substring(1));
                        switch (word[0].ToString())
                        {
                            case ">":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q > compare_value).ToList()).Count);
                                break;
                            case "<":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q < compare_value).ToList()).Count);
                                break;
                            case "=":
                                UpdateAnsByVal((pending_data = pending_data.Where(q => q == compare_value).ToList()).Count);
                                break;
                            default:
                                ReportSolsticeError();
                                return;
                        }
                    }
                    else
                    {
                        UpdateAnsByVal(pending_data.Count);
                    }
                }
                if (s_split)
                {
                    if (word.Length > 3 && word.Substring(0, 3) == "by ")
                    {
                        if (!IsNumber(word.Substring(3)))
                        {
                            error_output = true;
                            return;
                        }
                        int split_length = Convert.ToInt32(word.Substring(3));
                        int data_length = pending_data.Count;
                        StringBuilder sb = new StringBuilder();
                        int check = 0;
                        foreach (double x in pending_data)
                        {
                            sb.Append(x);
                            if (--data_length > 0)
                            {
                                if (++check != split_length)
                                {
                                    sb.Append(generate_separator);
                                }
                                else
                                {
                                    sb.Append("\n");
                                    check = 0;
                                }
                            }
                        }
                        UpdateAnsByString(sb.ToString());
                    }
                    if (word.Length > 5 && word.Substring(0, 5) == "into ")
                    {
                        if (!IsNumber(word.Substring(5)))
                        {
                            error_output = true;
                            return;
                        }
                        int groups = Convert.ToInt32(word.Substring(5));
                        if (groups < 1)
                        {
                            error_output = true;
                            return;
                        }
                        int data_length = pending_data.Count;
                        StringBuilder sb = new StringBuilder();
                        int group_length = data_length / groups;
                        if (groups > data_length)
                        {
                            error_output = true;
                            return;
                        }
                        if (data_length % groups != 0)
                        {
                            group_length++;
                        }
                        int check = 0;
                        foreach (double x in pending_data)
                        {
                            sb.Append(x);
                            if (--data_length > 0)
                            {
                                if (++check != group_length)
                                {
                                    sb.Append(generate_separator);
                                }
                                else
                                {
                                    sb.Append("\n");
                                    check = 0;
                                }
                            }
                        }
                        UpdateAnsByString(sb.ToString());
                    }
                }
                if (suffix2.Contains(word))
                {
                    switch (word)
                    {
                        case "reverse":
                            if (s_sort)
                            {
                                pending_data.Reverse();
                                active_sort = true;
                                UpdateAnsByList(pending_data);
                            }
                            break;
                        case "no repeat":
                        case "distinct":
                            if (s_select || s_count)
                            {
                                List<double> temp_date = new List<double>();
                                pending_data.ForEach(q =>
                                {
                                    if (!temp_date.Contains(q))
                                    {
                                        temp_date.Add(q);
                                    }
                                });
                                UpdateAnsByList(pending_data = temp_date);
                            }
                            break;
                    }
                }
                // sort ascend
                if (s_sort && !active_sort)
                {
                    UpdateAnsByList(pending_data);
                }
                if (suffix.Contains(word))
                {
                    switch (word)
                    {
                        case "to f1":
                            target_display_rtx = 0;
                            break;
                        case "to f2":
                            target_display_rtx = 1;
                            break;
                        case "to f":
                            target_display_rtx = target_tab;
                            break;
                        case "to output":
                            target_display_rtx = 2;
                            break;
                        case "to clipboard":
                            result_to_clipboard = true;
                            break;
                    }
                }
            });
            if (!result_to_clipboard)
            {
                DisplaySolsticeResult(target_display_rtx);
            }
            else
            {
                UpdateDebug("Solstice -> " + ans);
                Clipboard.SetDataObject(ans, true);
            }
        }

        private void 开始调试SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sentence = Rtx_Solstice.SelectedText == "" ? Rtx_Solstice.Text : Rtx_Solstice.SelectedText;
            string[] commands = sentence.Split('\\');
            foreach (string c in commands)
            {
                StartDebugging(c);
            }
        }

        private void 关闭调试窗口LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            调试窗口PToolStripMenuItem_Click(this, new EventArgs());
        }

        private void 关闭输出窗口LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            输出窗口OToolStripMenuItem_Click(this, new EventArgs());
        }

        private void 文件管理器FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Btn_Menu1_File_Click(this, new EventArgs());
        }

        private void 公式LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Btn_Menu1_Formula_Click(this, new EventArgs());
        }

        private void 数据DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Btn_Menu1_Data_Click(this, new EventArgs());
        }

        private void 代码CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Btn_Menu1_Solstice_Click(this, new EventArgs());
        }

        private void 了解如何使用SolsticeHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.dream7c.com/Solstice.html");
        }

        private void 空格转换为换行符ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.Replace(' ', '\n');
        }

        private void 换行符转换为空格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rtxs_file[target_tab].SelectedText = Rtxs_file[target_tab].SelectedText.Replace('\n', ' ');
        }

        private void Rtx_Debug_TextChanged(object sender, EventArgs e)
        {
            Rtx_Debug.ScrollToCaret();
        }

        private void Btn_CopyFromOutput_Click(object sender, EventArgs e)
        {
            复制输出窗口内容到选中窗口TToolStripMenuItem_Click(this, new EventArgs());
        }

        private void TsS_Run_Click(object sender, EventArgs e)
        {
            开始调试SToolStripMenuItem_Click(this, new EventArgs());
        }

        private void TsS_Clear_Click(object sender, EventArgs e)
        {
            Rtx_Solstice.Text = "";
        }

        private void TsS_Help_Click(object sender, EventArgs e)
        {
            UpdateDebug("Solstice -> v 1.2.6");
            System.Diagnostics.Process.Start("http://www.dream7c.com/Solstice.html");
        }

        private void SetPairStatus(int target)
        {
            ToolStripMenuItem[] items = { 新建水平文档组HToolStripMenuItem, 新建垂直文档组VToolStripMenuItem };
            foreach (ToolStripMenuItem item in items)
            {
                if (!item.Enabled)
                {
                    return;
                }
            }
            if (target == doc_view_type)
            {
                新建单文档SToolStripMenuItem_Click(this, new EventArgs());
            }
            else
            {
                ShowPairDoc(target);
            }
        }

        private void Ts_HPair_Click(object sender, EventArgs e)
        {
            SetPairStatus(1);
        }

        private void Ts_VPair_Click(object sender, EventArgs e)
        {
            SetPairStatus(2);
        }

        private void GenerateDataToSelectedRtx()
        {
            string select_text = Rtxs_file[target_tab].SelectedText;
            if (select_text != "" && select_text.Contains(quick_generate_separator))
            {
                string[] str = select_text.Split(quick_generate_separator);
                if (str.Contains(""))
                {
                    return;
                }
                List<double> paras = new List<double>();
                foreach (string s in str)
                {
                    if (!IsNumber(s))
                    {
                        return;
                    }
                    paras.Add(Convert.ToDouble(s));
                }
                string result = "";
                switch (paras.Count)
                {
                    case 2:
                        if (paras[0] < paras[1])
                        {
                            do
                            {
                                result += paras[0].ToString(GetDecimalPlace()) + generate_separator;
                            } while (paras[0]++ < paras[1]);
                        }
                        else
                        {
                            do
                            {
                                result += paras[0].ToString(GetDecimalPlace()) + generate_separator;
                            } while (paras[0]-- > paras[1]);
                        }
                        break;
                    case 3:
                        if (paras[1] == 0)
                        {
                            return;
                        }
                        if (paras[0] <= paras[2])
                        {
                            while (paras[0] < paras[2])
                            {
                                result += paras[0].ToString(GetDecimalPlace()) + generate_separator;
                                paras[0] += paras[1];
                            }
                        }
                        else
                        {
                            while (paras[0] >= paras[2])
                            {
                                result += paras[0].ToString(GetDecimalPlace()) + generate_separator;
                                paras[0] -= paras[1];
                            }
                        }
                        break;
                    default:
                        return;
                }
                result = result.TrimEnd();
                Rtxs_file[target_tab].SelectedText = result;
            }
        }

        private void Rtx_File_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyValue == 13)
            {
                GenerateDataToSelectedRtx();
                SendKeys.Send("%");
            }
        }

        private void Rtx_File_2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyValue == 13)
            {
                GenerateDataToSelectedRtx();
                SendKeys.Send("%");
            }
        }

        private void 生成数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataToSelectedRtx();
        }

        private void 添加文件DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            添加DToolStripMenuItem_Click(this, new EventArgs());
        }

        private void 打开文件资源管理器EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", directory_name[treeview_selected_index]);
        }

        private void 更改文件夹路径CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            选项OToolStripMenuItem_Click(this, new EventArgs());
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFileToTreeView();
        }
    }
}
