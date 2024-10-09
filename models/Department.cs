namespace department.models
{
    public class Department
    {
        public int id { get; set; }

        public string name { get; set; }
        public string Manger { get; set; }

        

        public virtual List<Employee> employees { get; set; }
    }
}
