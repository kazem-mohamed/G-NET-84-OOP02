#region Question 1
/*
a) Difference between a class and a struct:

   - A class is a reference type. Its object is stored on the heap, and variables
     hold references to that object. Copying a class variable copies the
     reference, so both variables point to the same object.

   - A struct is a value type. Its value is stored directly in the variable or
     inside the containing object. Copying a struct variable copies the whole
     value, so each variable has an independent copy.

   - Classes support inheritance, while structs cannot inherit from another
     struct or class.

b) Classes are more suitable than structs for large applications because large
   systems usually need inheritance, polymorphism, shared object identity, and
   controlled relationships between objects. Classes also avoid expensive copies
   when objects contain many fields, because only references are passed around.
*/
#endregion

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Smart Delivery Management System");
    }
}
