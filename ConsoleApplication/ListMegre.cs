using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication
{
    class ListMegre
    {
        public static void Test()
        {
            List<Model> list1 = new List<Model>();
            for (int i = 0; i < 10; i++)
            {
                list1.Add(new Model { Id = i, Name = i.ToString() });
            }

            List<Model> list2 = new List<Model>();
            for (int i = 5; i < 15; i++)
            {
                list1.Add(new Model { Id = i, Name = i.ToString() });
            }

            var list = list1.Except(list2, new ModelEqualityComparer());
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + "\t" + item.Name);
            }

            List<Person> list3 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                list3.Add(new Person { Id = i, Name = i.ToString() });
            }

            List<Person> list4 = new List<Person>();
            for (int i = 5; i < 15; i++)
            {
                list4.Add(new Person { Id = i, Name = i.ToString() });
            }

            var list5 = list3.Except(list4);
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + "\t" + item.Name);
            }
        }
    }



    class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    class ModelEqualityComparer : IEqualityComparer<Model>
    {
        public bool Equals(Model x, Model y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Model obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    class Person : IEqualityComparer<Person>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(Person x, Person y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Person obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
