using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;
using Word = Microsoft.Office.Interop.Word;

namespace nauchka
{
    public partial class Result : Form
    {
        string n, f;
        public Result(string file, string num)
        {
            InitializeComponent();
            n = num;
            f = file;
        }
        DataTable lecturerData;
        private void CreateLecturerData()
        {
            lecturerData = new DataTable();
            lecturerData.Columns.Add("number", typeof(Int32));
            lecturerData.Columns.Add("topic", typeof(String));
            lecturerData.Columns.Add("type", typeof(String));
            lecturerData.Columns.Add("date", typeof(String));
            lecturerData.Columns.Add("time", typeof(String));
            lecturerData.Columns.Add("hoursAmount", typeof(Int32));
            lecturerData.Columns.Add("signature", typeof(String));

        }

        private void Result_Load(object sender, EventArgs e)
        {
            label1.Text = n;
            CreateLecturerData();

            dataGridView1.DataSource = lecturerData;
            dataGridView1.Columns["number"].HeaderText = "№";
            dataGridView1.Columns["number"].Width = 100;
            dataGridView1.Columns["topic"].HeaderText = "Тема";
            dataGridView1.Columns["topic"].Width = 250;
            dataGridView1.Columns["type"].HeaderText = "Тип";
            dataGridView1.Columns["type"].Width = 200;
            dataGridView1.Columns["date"].HeaderText = "Дата";
            dataGridView1.Columns["date"].Width = 150;
            dataGridView1.Columns["time"].HeaderText = "Время";
            dataGridView1.Columns["time"].Width = 150;
            dataGridView1.Columns["hoursAmount"].HeaderText = "Количество часов";
            dataGridView1.Columns["hoursAmount"].Width = 150;
            dataGridView1.Columns["signature"].HeaderText = "Роспись";
            dataGridView1.Columns["signature"].Width = 150;

            string path = "http://timetable.sbmt.by/shedule/lecturer/" + f;

            XDocument doc = XDocument.Load(path);
            int i = 1;
            var elemList =
                from el in doc.Descendants("lesson")
                where ((string)el.Element("group")).IndexOf(n) > -1
                select el;

            foreach (var elem in elemList)
            {

                DataRow tempRow = lecturerData.NewRow();
                tempRow["type"] = elem.Element("type").Value;
                tempRow["date"] = elem.Element("date").Value;
                tempRow["time"] = elem.Element("time").Value;
                tempRow["number"] = i;
                tempRow["hoursAmount"] = 2;
                i++;
                lecturerData.Rows.Add(tempRow);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            Word.Document wd = new Word.Document();
            wd.Activate();
            Object start = Type.Missing;
            Object end = Type.Missing;
            Word.Range rng = wd.Range(ref start, ref end);
            Object defaultTableBehavior = Type.Missing;
            Object autoFitBehavior = Type.Missing;
            object missing = System.Type.Missing;
            Word.Table tbl = wd.Tables.Add(rng, 1, 7, ref missing, ref missing);
            SetHeadings(tbl.Cell(1, 1), "Title 1");
            SetHeadings(tbl.Cell(1, 2), "Title 2");
            SetHeadings(tbl.Cell(1, 3), "Title 3");
            SetHeadings(tbl.Cell(1, 4), "Title 4");
            SetHeadings(tbl.Cell(1, 5), "Title 5");
            SetHeadings(tbl.Cell(1, 6), "Title 6");
            SetHeadings(tbl.Cell(1, 7), "Title 7");
            for (int i = 0; i < lecturerData.Rows.Count; i++)
            {
                Word.Row newRow = wd.Tables[1].Rows.Add(ref missing);
                newRow.Range.Font.Bold = 0;
                newRow.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                newRow.Cells[1].Range.Text = lecturerData.Rows[i][0].ToString();
                newRow.Cells[2].Range.Text = lecturerData.Rows[i][1].ToString();
                newRow.Cells[3].Range.Text = lecturerData.Rows[i][2].ToString();
                newRow.Cells[4].Range.Text = lecturerData.Rows[i][3].ToString(); ;
                newRow.Cells[5].Range.Text = lecturerData.Rows[i][4].ToString();
                newRow.Cells[6].Range.Text = lecturerData.Rows[i][5].ToString();
                newRow.Cells[7].Range.Text = lecturerData.Rows[i][6].ToString();
            }
        }
        static void SetHeadings(Word.Cell tblCell, string text)
        {
            tblCell.Range.Text = text;
            tblCell.Range.Font.Bold = 1;
            tblCell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        }
    }
}

