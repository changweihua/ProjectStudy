using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication
{
    public class FileNameEx : IComparable
    {
        public string FileName { get; set; }


        public int CompareTo(object obj)
        {
            FileNameEx fne = (obj as FileNameEx);

            if (fne.FileName == this.FileName)
            {
                return 0;
            }

            //if()

            //if(result < 1e-3)
            //{
            //    return 0;//相等
            //}
            //if((obj as Circle).Radius < this.Radius)
            //{
            //    return 1;
            //}
            return -1;

        }

        public override bool Equals(object obj)
        {
            if (this.CompareTo(obj) == 0)
            {
                return true;
            }
            return false;

        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = (int)((new Random()).Next(1000) * 1000);

            return hashCode;

        }

        public static bool operator ==(FileNameEx lhs, FileNameEx rhs)
        {


            return lhs.Equals(rhs);

        }

        public static bool operator !=(FileNameEx lhs, FileNameEx rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator <=(FileNameEx lhs, FileNameEx rhs)
        {
            if ((lhs.CompareTo(rhs) < 0) || (lhs.CompareTo(rhs) == 0))
            {
                return true;
            }
            return false;
        }

        public static bool operator >=(FileNameEx lhs, FileNameEx rhs)
        {
            if ((lhs.CompareTo(rhs) > 0) || (lhs.CompareTo(rhs) == 0))
            {
                return true;
            }
            return false;
        }


    }
}
