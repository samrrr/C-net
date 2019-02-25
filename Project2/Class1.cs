using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;


namespace Project2
{

    public class Tree
    {
        private class Node
        {
            public Node l;
            public Node r;
            public int val;
            public int level;

            public Node(int _val)
            {
                l = null;
                r = null;
                val = _val;
                level = 0;
            }

            private void Lcalc()
            {
                if (l == null)
                {
                    if (r == null)
                    {
                        level = 0;
                    }
                    else
                    {
                        level = r.level + 1;
                    }         
                }
                else
                {
                    if (r == null)
                    {
                        level = l.level + 1;
                    }
                    else
                    {
                        level = Math.Max(r.level,l.level) + 1;
                    }
                }
            }

            static public Node Rotl(Node p)
            {
                // Node t0 = p.l;
                Node t1 = p.r.l;
                //Node t2 = p.r.r;
                Node l0 = p;
                Node l1 = p.r;
                l1.l = l0;
                l0.r = t1;
                l0.Lcalc();
                l1.Lcalc();
                return l1;
            }
            static public Node Rotr(Node p)
            {
                // Node t0 = p.r;
                Node t1 = p.l.r;
                //Node t2 = p.l.l;
                Node l0 = p;
                Node l1 = p.l;
                l1.r = l0;
                l0.l = t1;
                l0.Lcalc();
                l1.Lcalc();
                return l1;
            }
            static public Node Rotlbig(Node p)
            {
                // Node t0 = p.l;
                Node t1 = p.r.l.l;
                Node t2 = p.r.l.r;
                //Node t3 = p.r.r;
                Node l0 = p;
                Node l1 = p.r;
                Node l2 = p.r.l;
                l0.r = t1;
                l1.l = t2;
                l2.l = l0;
                l2.r = l1;
                l0.Lcalc();
                l1.Lcalc();
                l2.Lcalc();
                return l2;
            }
            static public Node Rotrbig(Node p)
            {
                // Node t0 = p.r;
                Node t1 = p.l.r.r;
                Node t2 = p.l.r.l;
                //Node t3 = p.l.l;
                Node l0 = p;
                Node l1 = p.l;
                Node l2 = p.l.r;
                l0.l = t1;
                l1.r = t2;
                l2.r = l0;
                l2.l = l1;
                l0.Lcalc();
                l1.Lcalc();
                l2.Lcalc();
                return l2;
            }
            static public bool Testr(Node n)
            {
                if (n == null)
                    return true;
                if (!Testr(n.l))
                    return false;
                if (!Testr(n.r))
                    return false;

                int old = n.level;
                int l_l=-1, l_r=-1;
                if (n.l != null)
                    l_l = n.l.level;
                if (n.r != null)
                    l_r = n.r.level;
                if (l_l - l_r >= 2)
                    return false;
                if (l_l - l_r <= -2)
                    return false;
                n.Lcalc();
                if(n.level!=old)
                    return false;
                return true;
            }
        }

        private Node tree;

        public bool Test()
        {
            return Node.Testr(tree);
        }
        public string Getstr()
        {
            return Getstrr(tree);
        }
        private string Getstrr(Node n)
        {
            if (n == null)
                return "_";
            return "("+Getstrr(n.l) + "," +n.val+ "," + Getstrr(n.r) + ")";
        }
        public bool Is(int val)
        {
            if (tree == null)
            {
                return false;
            }

            return Isr(tree, val);
        }
        private bool Isr(Node n, int val)//0-err 1-ok 2-rl 3-rr
        {
            if (val > n.val)
            {
                if (n.r != null)
                {
                    return Isr(n.r, val);
                }
                else
                {
                    return false;
                }
            }
            if (val < n.val)
            {
                if (n.l != null)
                {
                    return Isr(n.l, val);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public bool Add(int val)
        {
            if (tree == null)
            {
                tree = new Node(val);
                return true;
            }

            int res = Addr(tree, val);
            if (res == 2)
            {
                if (tree.r.l != null && (tree.l == null || tree.l.level < tree.r.l.level))
                    tree = Node.Rotlbig(tree);
                else
                    tree = Node.Rotl(tree);
                return true;
            }
            if (res == 3)
            {
                if (tree.l.r != null && (tree.r == null || tree.r.level < tree.l.r.level))
                    tree = Node.Rotrbig(tree);
                else
                    tree = Node.Rotr(tree);
                return true;
            }
            if (res == 1)
            {
                return true;
            }
            return false;
        }
        private int Addr(Node n,int val)//0-err 1-ok 2-rl 3-rr
        {
            if (val == n.val)
            {
                return 0;
            }
            if (val > n.val)
            {
                if (n.r != null)
                {
                    int res = Addr(n.r, val);
                    if (res == 0)
                        return 0;
                    if (res == 1)
                    {
                        if (n.l == null)
                            return 2;
                        if (n.r.level - n.l.level > 1)
                            return 2;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 2)
                    {

                        if (n.r.r.l != null && (n.r.l == null || n.r.l.level < n.r.r.l.level))
                            n.r = Node.Rotlbig(n.r);
                        else
                            n.r = Node.Rotl(n.r);
                        return 1;
                    }
                    if (res == 3)
                    {
                        if (n.r.l.r != null && (n.r.r == null || n.r.r.level < n.r.l.r.level))
                            n.r = Node.Rotrbig(n.r);
                        else
                            n.r = Node.Rotr(n.r);
                        return 1;
                    }
                }
                else
                {
                    n.r = new Node(val);
                    n.level = 1;
                    return 1;
                }
            }
            if (val < n.val)
            {
                if (n.l != null)
                {
                    int res = Addr(n.l, val);
                    if (res == 0)
                        return 0;
                    if (res == 1)
                    {
                        if (n.r == null)
                            return 3;
                        if (n.l.level - n.r.level > 1)
                            return 3;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 2)
                    {

                        if (n.l.r.l != null && (n.l.l == null || n.l.l.level < n.l.r.l.level))
                            n.l = Node.Rotlbig(n.l);
                        else
                            n.l = Node.Rotl(n.l);
                        return 1;
                    }
                    if (res == 3)
                    {
                        if (n.l.l.r != null && (n.l.r == null || n.l.r.level < n.l.l.r.level))
                            n.l = Node.Rotrbig(n.l);
                        else
                            n.l = Node.Rotr(n.l);
                        return 1;
                    }
                }
                else
                {
                    n.l = new Node(val);
                    n.level = 1;
                    return 1;
                }
            }
            return -1;
        }
        public bool Del(int val)
        {
            if (tree == null)
            {
                return false;
            }

            int res = Delr(tree, val);

            if (res == 4)
            {
                if (tree.r == null && tree.l != null)
                {
                    tree = tree.l;
                    return true;
                }
                else
                if (tree.r != null && tree.l == null)
                {
                    tree = tree.r;
                    return true;
                }
                else
                if (tree.r == null && tree.l == null)
                {
                    tree = null;
                    return true;
                }
                else
                {
                    Node pmin = tree.r;
                    while (pmin.l != null)
                        pmin = pmin.l;

                    res = Delr(tree, pmin.val);
                    tree.val = pmin.val;
                }
            }
            if (res == 2)
            {
                if (tree.r.l != null && (tree.l == null || tree.l.level < tree.r.l.level))
                    tree = Node.Rotlbig(tree);
                else
                    tree = Node.Rotl(tree);
                return true;
            }
            if (res == 3)
            {
                if (tree.l.r != null && (tree.r == null || tree.r.level < tree.l.r.level))
                    tree = Node.Rotrbig(tree);
                else
                    tree = Node.Rotr(tree);
                return true;
            }
            if (res == 1)
            {
                return true;
            }
            return false;
        }
        private int Delr(Node n, int val)//0-err 1-ok 2-rl 3-rr 4-delthis
        {
            if (val == n.val)
            {
                return 4;
            }
            if (val > n.val)
            {
                if (n.r != null)
                {
                    int res = Delr(n.r, val);
                    if (res == 4)
                    {
                        if (n.r.r == null && n.r.l != null)
                        {
                            n.r = n.r.l;
                            n.level = Math.Max(n.l.level, n.r.level) + 1;
                            if (n.l.level - n.r.level > 1)
                                return 3;
                            return 1;
                        }
                        else
                        if (n.r.r != null && n.r.l == null)
                        {
                            n.r = n.r.r;
                            n.level = Math.Max(n.l.level, n.r.level) + 1;
                            if (n.l.level - n.r.level > 1)
                                return 3;
                            return 1;
                        }
                        else
                        if (n.r.r == null && n.r.l == null)
                        {
                            n.r = null;
                            if (n.l != null && n.l.level > 0)
                                return 3;
                            if (n.l == null)
                                n.level = 0;
                            return 1;
                        }
                        else
                        {
                            Node pmin=n.r.r;
                            while (pmin.l != null)
                                pmin = pmin.l;

                            res = Delr(n.r, pmin.val);
                            n.r.val = pmin.val;
                        }
                    }
                    if (res == 0)
                        return 0;
                    if (res == 1)
                    {
                        if (n.l.level - n.r.level > 1)
                            return 3;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 2)
                    {

                        if (n.r.r.l != null && (n.r.l == null || n.r.l.level < n.r.r.l.level))
                            n.r = Node.Rotlbig(n.r);
                        else
                            n.r = Node.Rotl(n.r);
                        if (n.l.level - n.r.level > 1)
                            return 3;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 3)
                    {
                        if (n.r.l.r != null && (n.r.r == null || n.r.r.level < n.r.l.r.level))
                            n.r = Node.Rotrbig(n.r);
                        else
                            n.r = Node.Rotr(n.r);
                        if (n.l.level - n.r.level > 1)
                            return 3;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            if (val < n.val)
            {
                if (n.l != null)
                {
                    int res = Delr(n.l, val);
                    if (res == 4)
                    {
                        if (n.l.r == null && n.l.l != null)
                        {
                            n.l = n.l.l;
                            n.level = Math.Max(n.r.level, n.l.level) + 1;
                            if (n.r.level - n.l.level > 1)
                                return 2;
                            return 1;
                        }
                        else
                        if (n.l.r != null && n.l.l == null)
                        {
                            n.l = n.l.r;
                            n.level = Math.Max(n.r.level, n.l.level) + 1;
                            if (n.r.level - n.l.level > 1)
                                return 2;
                            return 1;
                        }
                        else
                        if (n.l.r == null && n.l.l == null)
                        {
                            n.l = null;
                            if (n.r != null && n.r.level > 0)
                                return 2;
                            if (n.r == null)
                                n.level = 0;
                            return 1;
                        }
                        else
                        {
                            Node pmin = n.l.r;
                            while (pmin.l != null)
                                pmin = pmin.l;

                            res = Delr(n.l, pmin.val);
                            n.l.val = pmin.val;
                        }
                    }
                    if (res == 0)
                        return 0;
                    if (res == 1)
                    {
                        if (n.r.level - n.l.level > 1)
                            return 2;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 2)
                    {

                        if (n.l.r.l != null && (n.l.l == null || n.l.l.level < n.l.r.l.level))
                            n.l = Node.Rotlbig(n.l);
                        else
                            n.l = Node.Rotl(n.l);
                        if (n.r.level - n.l.level > 1)
                            return 2;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                    if (res == 3)
                    {
                        if (n.l.l.r != null && (n.l.r == null || n.l.r.level < n.l.l.r.level))
                            n.l = Node.Rotrbig(n.l);
                        else
                            n.l = Node.Rotr(n.l);
                        if (n.r.level - n.l.level > 1)
                            return 2;
                        n.level = Math.Max(n.l.level, n.r.level) + 1;
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        }
        public Tree()
        {
            tree = null;
        }
    }

    public unsafe class XORList : IDisposable
    {
        private Node* _first = null;
        private Node* _second = null;
        private Node* first = null;
        private Node* second = null;


        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct Node
        {
            public int val;
            public Node* xorLink;
        }
        private static Node* _ptrXor(Node* a, Node* b)
        {
            return (Node*)((ulong)a ^ (ulong)b);
        }
        private static Node* _next(Node* p1, Node* p2)
        {
            return _ptrXor(p2->xorLink, p1);
        }
        private static Node* _prev(Node* p1, Node* p2)
        {
            return _ptrXor(p1->xorLink, p2);
        }
        public XORList(int []arr)
        {
            if (arr.Length < 2)
                throw new Exception();
            _first = (Node*)Marshal.AllocHGlobal(sizeof(Node));
            (*_first).val = arr[0];

            _second = (Node*)Marshal.AllocHGlobal(sizeof(Node));
            (*_second).val = arr[1];

            var p0 = _first;
            var p1 = _second;
            for (int i = 2; i < arr.Length; i++)
            {
                var p2 = (Node*)Marshal.AllocHGlobal(sizeof(Node));
                (*p1).xorLink = _ptrXor(p0, p2);
                (*p2).val = arr[i];
                p0 = p1;
                p1 = p2;
            }
            (*p1).xorLink = _ptrXor(p0, _first);
            (*_first).xorLink = _ptrXor(p1, _second);

            ITstart();
        }
        public string Getstr()
        {
            Node* p0=_first;
            Node* p1=_second;
            string res="";

            while (true)
            {
                Node* p2 = _next(p0,p1);

                res += (*p0).val+",";

                p0 = p1;
                p1 = p2;
                if (p2 == _first)
                    break;
            }

            res += (*p0).val;

            return res;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ITstart()
        {
            first = _first;
            second = _second;
        }
        public void ITnext()
        {
            var p = _next(first, second);
            first = second;
            second = p;
        }
        public void ITprev()
        {
            var p = _prev(first, second);
            second = first;
            first = p;
        }
        public int ITget()
        {
            return (*first).val;
        }
        public void ITset(int _val)
        {
            (*first).val = _val;
        }

        public void ITadd(int _val)
        {
            int dd = 0;
            if (second == _second)
                dd = 1;
            Node* p0 = _prev(first, second);
            Node* p1 = _next(first, second);
            Node* pn = (Node*)Marshal.AllocHGlobal(sizeof(Node));
            (*pn).val = _val;
            (*pn).xorLink = _ptrXor(first, second);
            (*first).xorLink = _ptrXor(p0, pn);
            (*second).xorLink = _ptrXor(pn, p1);
            second = pn;
            if (dd == 1)
            {
                _second = pn;
            }
        }
        public void ITdel()
        {
            int dd = 0;
            if (second == _second)
                dd = 1;
            if (second == _first)
                dd = 2;
            Node* p0 = _prev(first, second);
            Node* p1 = _next(first, second);
            Node* p2 = _next(second, p1);
            Marshal.FreeHGlobal((IntPtr)second);
            (*first).xorLink = _ptrXor(p0, p1);
            (*p1).xorLink = _ptrXor(first, p2);
            second = p1;
            if (dd == 1)
            {
                _second = p1;
            }
            if (dd == 2)
            {
                _first=p1;
                _second = p2;
            }
        }

        public void Reverse()
        {
            Node* p0 = _prev(first, _second);
            Node* p1 = _prev(p0, _first);
            _first = p0;
            _first = p1;
        }
        protected virtual void Dispose(bool disposing)
        {
            var first = _first;
            var second = _second;

            var start = first;
            while (true)
            {
                var next = _next(first, second);
                Marshal.FreeHGlobal((IntPtr)first);
                if (next == start)
                    break;
                first = second;
                second = next;
            }
            Marshal.FreeHGlobal((IntPtr)second);
        }
    }
    public class MainClass
    {
        static void SortInserting(ref int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int val = arr[i];
                int r = i;
                while (r > 0 && arr[r - 1] > val)
                {
                    arr[r] = arr[r - 1];
                    r--;
                }

                arr[r] = val;
            }

        }
        static void Main()
        {
            Random rand = new Random(432);
            Tree t = new Tree();

            for (int i = 0; i < 100; i++)
            {
                int val = rand.Next(100);
                t.Add(val);
                Console.WriteLine(t.Getstr());
                Console.WriteLine(t.Test());
            }
            rand = new Random(432);
            for (int i = 0; i < 100; i++)
            {
                int val = rand.Next(100);
                Console.WriteLine("D:" + val);

                t.Del(val);
                Console.WriteLine(t.Getstr());
                Console.WriteLine(t.Test());
            }

            int[] arr = {0, 3, 5, 7, 45, 23, 2, 3, 4, 6, 0, 2, 1, -1};
            XORList li = new XORList(arr);

            Console.WriteLine(li.Getstr());
            for (int i = 0; i < 10; i++)
            {
                int val = rand.Next(100);
                li.ITadd(val);
                li.ITnext();
                li.ITnext();
                Console.WriteLine(li.Getstr());
            }
            for (int i = 0; i < 10; i++)
            {
                li.ITprev();
                li.ITprev();
                li.ITdel();
                Console.WriteLine(li.Getstr());
            }

            string s;
            s = "";
            foreach (var val in arr)
            {
                s = s + " " + val;
            }
            Console.WriteLine(s);
            SortInserting(ref arr);
            
            s = "";
            foreach (var val in arr)
            {
                s = s + " " + val;
            }
            Console.WriteLine(s);


            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
