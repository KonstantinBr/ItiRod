using CsvHelper;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Root.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace CSV
{
    public class Subject
    {
        public string Student { get; set; }
        public string SubjectName { get; set; }
        public string Mark { get; set; }
        public override string ToString()
        {
            return $"[Student:{Student}][Subject:{SubjectName}] [Mark:{Mark}]";
        }
    }

    class CSVTransformer
    {
        const string PATH = @"csv\marks.csv";
        IEnumerable<Subject> UploadFile(string path)
        {
            using (StreamReader streamReader = new StreamReader(path))
            {
                using (CsvReader csvReader = new CsvReader(streamReader))
                {
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.Delimiter = ";";
                    List<Subject> subject = csvReader.GetRecords<Subject>().ToList();
                    return subject;
                }
            }
        }

        public void ConsoleOutput(string path = PATH)
        {
            var subjects = UploadFile(path);
            foreach (var subj in subjects)
            {
                Console.WriteLine(subj);
            }
        }

        //public void XLSXOutput(string path = PATH)
        //{
        //    var subjects = UploadFile(path);
        //    var row = 0;
        //    FileStream stream = new FileStream("marks.xls", FileMode.OpenOrCreate);
        //    ExcelWriter writer = new ExcelWriter(stream);
        //    writer.BeginWrite();
        //    foreach (var subject in subjects)
        //    {
        //        writer.WriteCell(row, 0, subject.Student);
        //        writer.WriteCell(row, 1, subject.SubjectName);
        //        writer.WriteCell(row++, 2, subject.Mark);
        //    }
        //    writer.EndWrite();
        //    stream.Close();
        //}

        public void XLSXOutputGroupByStudent(string path = PATH)
        {
            var subjects = UploadFile(path);
            var row = 0;
            var col = 0;
            FileStream stream = new FileStream("marks.xls", FileMode.OpenOrCreate);
            ExcelWriter writer = new ExcelWriter(stream);
            writer.BeginWrite();
            var subjList = subjects.Select(a=>a.SubjectName).Distinct().ToList();

            foreach (var subjectName in subjList)
            {
                writer.WriteCell(row, ++col, subjectName);
            }
            writer.WriteCell(row, ++col, "Average");
            var maxRowForAverageMark = col;
            col = 0;

            var studentList = subjects.GroupBy(a => a.Student).ToList();
            foreach (var student in studentList)
            {
                writer.WriteCell(++row, col, student.Key);
                for(int i=0; i< subjList.Count; i++)
                {
                    ++col;
                    if (student.Any(a => a.SubjectName == subjList[i]))
                        writer.WriteCell(row, col, student.FirstOrDefault(a => a.SubjectName == subjList[i]).Mark);
                }
                writer.WriteCell(row, maxRowForAverageMark, student.Average(a=> int.Parse(a.Mark)));
                col = 0;
            }

            col = 0;
            row++;
            writer.WriteCell(row, col, "Average");

            for (int i = 0; i < subjList.Count; i++)
                if (subjects.Any(a => a.SubjectName == subjList[i]))
                    writer.WriteCell(row, ++col, subjects.Where(a => a.SubjectName == subjList[i]).Average(a => int.Parse(a.Mark)));

            writer.WriteCell(row, ++col, subjects.Average(a =>int.Parse(a.Mark)));

            writer.EndWrite();
            stream.Close();
        }

        public void PDFOutput(string path = PATH)
        {
            var subjects = UploadFile(path);

            Report report = new Report(new PdfFormatter());
            FontDef fd = new FontDef(report, "Helvetica");
            FontProp fp = new FontPropMM(fd, 5);
            Page page = new Page(report);
            int row = 40;
            foreach(var subj in subjects)
            {
                page.AddCenteredMM(row += 10, new RepString(fp, subj.ToString()));
            }
            report.Save("Marks.pdf");
        }

        public void JSONOutput(string path = PATH)
        {
            var subjects = UploadFile(path);
            OutputFormatForJson Json = new OutputFormatForJson();
            Json.AverageMark = subjects.Average(a => int.Parse(a.Mark));

            #region addStudents
            Json.Students = new List<Student>();
            foreach(var studentFromSubjects in subjects.GroupBy(a => a.Student).ToList())
            {
                Student newStudent = new Student();
                newStudent.Name = studentFromSubjects.Key;
                newStudent.AverageMark = studentFromSubjects.Average(a => int.Parse(a.Mark));

                newStudent.SudentsSubjects = new List<SudentsSubject>();
                foreach(var studentSubj in studentFromSubjects)
                {
                    newStudent.SudentsSubjects.Add(new SudentsSubject { Mark = int.Parse(studentSubj.Mark), Name = studentSubj.SubjectName });
                }

                Json.Students.Add(newStudent);
            }
            #endregion

            #region addSubjects
            Json.Subjects = new List<Subj>();
            foreach (var subjectsFromStudents in subjects.GroupBy(a => a.SubjectName).ToList())
            {
                Subj newSubj = new Subj();
                newSubj.Name = subjectsFromStudents.Key;
                newSubj.AverageMark = subjectsFromStudents.Average(a => int.Parse(a.Mark));

                newSubj.SubjSudents = new List<SubjSudent>();
                foreach (var student in subjectsFromStudents)
                {
                    newSubj.SubjSudents.Add(new SubjSudent { Mark = int.Parse(student.Mark), Name = student.Student });
                }

                Json.Subjects.Add(newSubj);
            }
            #endregion

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(OutputFormatForJson));

            using (FileStream fs = new FileStream("Marks.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, Json);
            }
        }

    }
}
