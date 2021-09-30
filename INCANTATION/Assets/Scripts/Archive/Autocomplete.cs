using System.Collections;
using System.Collections.Generic;

public class Autocomplete
{
    //this is a trie

    private Node root;

    public Autocomplete()
    {
        root = new Node('\0');
    }

    public void Insert(string word)
    {
        Node curr = root;
        for(int i = 0; i < word.Length; i++)
        {
            char c = word.ToCharArray()[i];
            if(curr.children[c - 'a'] == null)
            {
                curr.children[c - 'a'] = new Node(c);
            }
            curr = curr.children[c - 'a'];
        }
        curr.isTerminal = true;
    }

    public bool Search(string word)
    {
        Node node = GetNode(word);
        return node != null && node.isTerminal;
    }

    public bool StartsWith(string prefix)
    {
        return GetNode(prefix) != null;
    }

    private Node GetNode(string word)
    {
        Node curr = root;
        for(int i = 0; i < word.Length; i++)
        {
            char c = word.ToCharArray()[i];
            if (curr.children[c - 'a'] == null) return null;
            curr = curr.children[c - 'a'];
        }
        return curr;
    }

    public string Suggest(string prefix)
    {
        string suggestion = "";
        string curr = "";
        Node lastNode = root;
        foreach(char c in prefix.ToCharArray())
        {
            lastNode = lastNode.children[c - 'a'];
            if (lastNode == null) return suggestion;
            curr += c;
        }
        return suggestion;
    }

    class Node
    {
        public char c;
        public bool isTerminal;
        public Node[] children;

        public Node(char c)
        {
            this.c = c;
            isTerminal = false;
            //VV 27 to include space?
            children = new Node[26];
        }
    }
}
