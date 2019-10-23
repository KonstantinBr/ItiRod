using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CSV
{
    [DataContract]
    public class OutputFormatForJson
    {
        [DataMember] public List<Student> Students { get; set; }
        [DataMember] public List<Subj> Subjects { get; set; }
        [DataMember] public double AverageMark { get; set; }
    }

    [DataContract]
    public class Student
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public double AverageMark { get; set; }
        [DataMember] public List<SudentsSubject> SudentsSubjects { get; set; }

    }

    [DataContract]
    public class SudentsSubject
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int Mark { get; set; }
    }

    [DataContract]
    public class Subj
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public double AverageMark { get; set; }
        [DataMember] public List<SubjSudent> SubjSudents { get; set; }

    }

    [DataContract]
    public class SubjSudent
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int Mark { get; set; }
    }
}
