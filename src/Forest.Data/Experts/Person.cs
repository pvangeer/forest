namespace Forest.Data.Experts
{
    public class Person : NotifyPropertyChangedObject
    {
        public string Name { get; set; }
        
        public string Email { get; set; }

        public string Telephone { get; set; }
    }
}