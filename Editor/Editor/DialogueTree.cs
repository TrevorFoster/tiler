using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Editor
{
    class DialogueTree
    {
        public Dictionary<int,TreeNode> tree;
        public List<OptionNode> Limbo;
        public int currentNode;
        public int nextNode;
        public DialogueTree(Dictionary<int, TreeNode> Tree)
        {
            this.tree = Tree;
            currentNode = 0;
            Limbo = new List<OptionNode>();
        }
        public void Initialize(Vector2 position)
        {
            float y = 0;
            float x = 0;
            float boxwidth = 200;
            float boxheight = 80;
            int BetweenBoxesHeight = 30;
            int BetweenBoxesWidth = 30;
            
            foreach (int i in tree.Keys)
            {
                if (tree[i].options.Count == 0)
                {
                    x = 0;
                    tree[i].node.position = new Vector2(position.X + x, position.Y + 15 + y);
                    y += BetweenBoxesHeight + boxheight;
                }

                else
                {
                    x = 0;
                    Vector2 boxpos = new Vector2(position.X + x + (boxwidth * ((float)tree[i].options.Count / 2)) + (BetweenBoxesWidth * ((float)tree[i].options.Count / 2)) - boxwidth / 2 - BetweenBoxesWidth / 2, position.Y + 15 + y);
                    tree[i].node.position = boxpos;
                    y += BetweenBoxesHeight + boxheight;
                    for (int b = 0; b < tree[i].options.Count; b++)
                    {
                        tree[i].options[b].position = new Vector2(position.X + x, position.Y + (float)15 + y);
                        x += boxwidth + BetweenBoxesWidth;
                    }
                    y += BetweenBoxesHeight + boxheight;
                }

            }
        }

        public void Update()
        {
            nextNode = tree[currentNode].Update();
            if (nextNode!=-1)
            {
                if (nextNode != currentNode)
                {
                    tree[currentNode].Off();
                }
                else
                {
                    tree[currentNode].Reset();
                }
                currentNode = nextNode;
            }
        }

        public void Draw()
        {
            tree[currentNode].Draw();
        }
    }

    class TreeNode
    {
        //public DialogBox output;
        public OptionBox option = null;
        bool end;
        public List<OptionNode> options = new List<OptionNode>();
        public Node node;

        public string Message;

        public TreeNode(string Output,List<int> targets,List<string> values, string Font, string nodekey)
        {
            //output = new DialogBox(Output, true, new Rectangle(100, 100, 400, 75), Font);
            node = new Node(Output,nodekey);
            for (int i = 0; i < targets.Count;i++ )
            {
                options.Add(new OptionNode(values[i], targets[i]));
            }
            
            Message = Output;
            //option = new OptionBox(Directions.Values.ToArray(), Font, Directions.Keys.ToArray());
            end = false;
            //directions = Directions;
        }
        public TreeNode(string Output, string Font,string nodekey)
        {
            //output = new DialogBox(Output, true, new Rectangle(100, 100, 400, 75), Font);
            node = new Node(Output,nodekey);
            Message = Output;
            end = true;
        }

        internal int Update()
        {
            if (!end)
            {
                if (!option.on)
                {
                    option.on = true;
                }
            }
            //output.Update();
            if (end)
            {
                return -1;
            }
            else
            {
                return option.Update();
            }
        }
        public void Reset()
        {
            //output.Reset();
        }

        internal void Draw()
        {
            //output.Draw();
            if (!end)
            {
                option.Draw();
            }
        }

        internal void Off()
        {
            option.Toggle();
        }
    }
}
