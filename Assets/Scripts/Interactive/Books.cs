using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : MonoBehaviour
{
    [SerializeField] public List<GameObject> Prefabs;

    [System.Serializable]
    public struct BookData
    {
        public string title;
        public string category;

        public BookData(string t, string c)
        {
            title = t;
            category = c;
        }
    }

    //[SerializeField]
    public static BookData[] bookList =
    {
        new BookData("The Hostage", "�A�h�x���`���["),
        new BookData("Don Quixote", "�A�h�x���`���["),
        new BookData("Treasure Island", "�A�h�x���`���["),
        new BookData("White Fang", "�A�h�x���`���["),
        new BookData("Kim", "�A�h�x���`���["),
        new BookData("Hatchet", "�A�h�x���`���["),
        new BookData("Stuart Little", "�A�h�x���`���["),
        new BookData("Robinson", "�A�h�x���`���["),
        new BookData("Harry", "�A�h�x���`���["),
        new BookData("The Count", "�A�h�x���`���["),
        new BookData("Congo", "�A�h�x���`���["),
        new BookData("Cruel Sea", "�A�h�x���`���["),
        new BookData("Long Ships", "�A�h�x���`���["),
        new BookData("Jaws", "�A�h�x���`���["),
        new BookData("Odyssey", "�A�h�x���`���["),
        new BookData("Dune", "�A�h�x���`���["),
        new BookData("Desert", "�A�h�x���`���["),
        new BookData("Zorro", "�A�h�x���`���["),
        new BookData("Scaramouche", "�A�h�x���`���["),
        new BookData("Lord", "�A�h�x���`���["),
        new BookData("Roads", "�A�h�x���`���["),

        new BookData("The Higgler", "���}���X"),
        new BookData("A Mere Interlude", "���}���X"),
        new BookData("Irish Revel", "���}���X"),
        new BookData("Twilight", "���}���X"),
        new BookData("Lolita", "���}���X"),
        new BookData("Adam Bede", "���}���X"),
        new BookData("Rebecca", "���}���X"),
        new BookData("Persuasion", "���}���X"),
        new BookData("Dear John", "���}���X"),
        new BookData("Cold Mountain", "���}���X"),
        new BookData("Wedding", "���}���X"),
        new BookData("Lucky One", "���}���X"),
        new BookData("Wings of Dove", "���}���X"),
        new BookData("Dawn", "���}���X"),
        new BookData("Notebook", "���}���X"),

        new BookData("Dancing Partner", "�R���f�B"),
        new BookData("Big Trouble", "�R���f�B"),
        new BookData("Catch-22", "�R���f�B"),
        new BookData("Election", "�R���f�B"),
        new BookData("Here We Are", "�R���f�B"),
        new BookData("The Princess Bride ", "�R���f�B"),
        new BookData("Lumber Room", "�R���f�B"),
        new BookData("Bossypants", "�R���f�B"),
        new BookData("Daddy�fs Boy", "�R���f�B"),
        new BookData("American Cornball", "�R���f�B"),
        new BookData("Good Omens", "�R���f�B"),
        new BookData("Naked", "�R���f�B"),
        new BookData("Egg and I", "�R���f�B"),
        new BookData("Wishful Drinking", "�R���f�B"),
        new BookData("Zombie Spaceship", "�R���f�B"),
    };

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < bookList.Length; i++)
        {
            BookData temp;
            int rnd = Random.Range(0, bookList.Length);
            temp = bookList[rnd];
            bookList[rnd] = bookList[i];
            bookList[i] = temp;
        }

        // Test

        Bookshelf bookshelf = GetComponentInParent<Bookshelf>();
        while (bookList.Length > 0)
        {
            GameObject book = popRandomBook();
            book.transform.position = transform.position;
            if (bookshelf && Random.Range(0, 2) == 0)
            {
                bookshelf.addBook(book);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public GameObject popRandomBook()
    {
        if (bookList.Length < 1) return null;

        GameObject book = Instantiate(Prefabs[Random.Range(0, Prefabs.Count - 1)]);
        BookData bd = bookList[bookList.Length - 1];
        book.GetComponent<BookObject>().setData(bd.title, bd.category);

        System.Array.Resize(ref bookList, bookList.Length - 1);

        return book;
    }
}