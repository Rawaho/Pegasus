using System;
using System.Collections;
using System.Text;

namespace Pegasus.Cryptography
{
    public class Class121
    {
        Class122 class122_0 = new Class122(Enum29.const_3, 0, 1L, 0x204);
        private Hashtable hashtable_0;
        private const int int_0 = 0x102;
        private const int int_1 = 0x204;
        private const string string_0 = "NYT";
        private const string string_1 = "EOS";

        public Class121()
        {
            this.class122_0.method_9(new Class122(Enum29.const_0, 0, 0L, 0x202));
            this.class122_0.method_11(new Class122(Enum29.const_1, 0, 1L, 0x203));
            this.class122_0.method_8().method_13(this.class122_0);
            this.class122_0.method_10().method_13(this.class122_0);
            this.hashtable_0 = new Hashtable(0x204, 0.5f);
            this.hashtable_0["NYT"] = this.class122_0.method_8();
            this.hashtable_0["EOS"] = this.class122_0.method_10();
        }

        public void method_0(byte byte_0)
        {
            Class122 class2 = this.method_2(byte_0);
            if (class2 == null)
            {
                class2 = this.method_6(byte_0, 0);
            }
            do
            {
                Class122 class3 = this.method_7(class2);
                if ((class2 != class3) && (class2.method_12() != class3))
                {
                    this.method_5(class2, class3);
                }
                class2.method_3(class2.method_2() + 1L);
                class2 = class2.method_12();
            }
            while (class2 != null);
        }

        public int[] method_1(Class122 class122_1)
        {
            ArrayList list = new ArrayList();
            while (class122_1.method_12() != null)
            {
                list.Add((class122_1.method_12().method_8() == class122_1) ? 0 : 1);
                class122_1 = class122_1.method_12();
            }
            list.Reverse();
            return (int[])list.ToArray(typeof(int));
        }

        internal string method_10()
        {
            string str = "";
            if (this.class122_0 != null)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "Depth: ", this.method_13(this.class122_0), Environment.NewLine }) + this.method_11(this.class122_0, 0);
            }
            return str;
        }

        private string method_11(Class122 class122_1, int int_2)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { this.method_12(int_2), "Type: ", class122_1.method_6(), Environment.NewLine }));
            builder.Append(string.Concat(new object[] { this.method_12(int_2), "Number: ", class122_1.method_0(), Environment.NewLine }));
            builder.Append(string.Concat(new object[] { this.method_12(int_2), "Count: ", class122_1.method_2(), Environment.NewLine }));
            builder.Append(string.Concat(new object[] { this.method_12(int_2), "Value: ", class122_1.method_4(), "(", char.IsControl((char)class122_1.method_4()) ? '-' : ((char)class122_1.method_4()), ")", Environment.NewLine }));
            if (class122_1.method_8() != null)
            {
                builder.Append(this.method_12(int_2 + 1) + "---- Left ----" + Environment.NewLine);
                builder.Append(this.method_11(class122_1.method_8(), int_2 + 1));
            }
            if (class122_1.method_10() != null)
            {
                builder.Append(this.method_12(int_2 + 1) + "---- Right ----" + Environment.NewLine);
                builder.Append(this.method_11(class122_1.method_10(), int_2 + 1));
            }
            return builder.ToString();
        }

        private string method_12(int int_2)
        {
            string str = "";
            for (int i = 0; i < int_2; i++)
            {
                str = str + "\t\t";
            }
            return str;
        }

        private int method_13(Class122 class122_1)
        {
            if (class122_1 == null)
            {
                return 0;
            }
            int num = this.method_13(class122_1.method_8());
            int num2 = this.method_13(class122_1.method_10());
            return (1 + ((num > num2) ? num : num2));
        }

        private string method_14()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 0x100; i++)
            {
                Class122 class2 = this.method_2((byte)i);
                if (class2 != null)
                {
                    builder.Append(string.Concat(new object[] { "[", char.IsControl((char)class2.method_4()) ? '-' : ((char)class2.method_4()), "]: ", class2.method_2(), Environment.NewLine }));
                }
            }
            return builder.ToString();
        }

        public Class122 method_2(byte byte_0)
        {
            return (Class122)this.hashtable_0[byte_0];
        }

        public Class122 method_3()
        {
            return (Class122)this.hashtable_0["NYT"];
        }

        public Class122 method_4()
        {
            return (Class122)this.hashtable_0["EOS"];
        }

        private void method_5(Class122 class122_1, Class122 class122_2)
        {
            if ((class122_1.method_12() == null) || (class122_2.method_12() == null))
            {
                throw new ApplicationException("Can't swap root nodes.");
            }
            int num = class122_1.method_0();
            class122_1.method_1(class122_2.method_0());
            class122_2.method_1(num);
            Class122 class2 = class122_1.method_12().method_8();
            Class122 class3 = class122_2.method_12().method_8();
            if (class2 == class122_1)
            {
                class122_1.method_12().method_9(class122_2);
            }
            else
            {
                class122_1.method_12().method_11(class122_2);
            }
            if (class3 == class122_2)
            {
                class122_2.method_12().method_9(class122_1);
            }
            else
            {
                class122_2.method_12().method_11(class122_1);
            }
            Class122 class4 = class122_1.method_12();
            class122_1.method_13(class122_2.method_12());
            class122_2.method_13(class4);
        }

        private Class122 method_6(byte byte_0, int int_2)
        {
            Class122 class2 = this.method_3();
            class2.method_7(Enum29.const_3);
            class2.method_9(new Class122(Enum29.const_0, 0, 0L, class2.method_0() - 2));
            class2.method_8().method_13(class2);
            class2.method_11(new Class122(Enum29.const_2, byte_0, (long)int_2, class2.method_0() - 1));
            class2.method_10().method_13(class2);
            this.hashtable_0["NYT"] = class2.method_8();
            this.hashtable_0[byte_0] = class2.method_10();
            return class2.method_10();
        }

        private Class122 method_7(Class122 class122_1)
        {
            Class122 class2 = class122_1;
            if (class122_1.method_12() != null)
            {
                Class122 class3 = class122_1.method_12();
                if ((class3.method_8() == class122_1) && (class3.method_10().method_2() == class122_1.method_2()))
                {
                    class2 = class3.method_10();
                }
                if (class3.method_12() == null)
                {
                    return class2;
                }
                Class122 class4 = class3.method_12();
                if ((class4.method_8() == class3) && (class4.method_10().method_2() == class122_1.method_2()))
                {
                    return class4.method_10();
                }
                if ((class4.method_10() == class3) && (class4.method_8().method_2() == class122_1.method_2()))
                {
                    class2 = class4.method_8();
                }
            }
            return class2;
        }

        private Class122 method_8(Class122 class122_1)
        {
            long num = class122_1.method_2();
            Queue queue = new Queue();
            queue.Enqueue(this.class122_0);
            while (queue.Count > 0)
            {
                Class122 class2 = (Class122)queue.Dequeue();
                if (class2.method_2() == num)
                {
                    return class2;
                }
                if (class2.method_10() != null)
                {
                    queue.Enqueue(class2.method_10());
                    queue.Enqueue(class2.method_8());
                }
            }
            return null;
        }

        public Class122 method_9()
        {
            return this.class122_0;
        }
    }
}
