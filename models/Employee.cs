using System.ComponentModel.DataAnnotations.Schema;

namespace department.models
{
    public class Employee
    {

        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        [ForeignKey("department")]

        public int departmentId { get; set; }

        public virtual Department department { get; set; }
    }
}
