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
        new BookData("The Hostage", "アドベンチャー"),
        new BookData("Don Quixote", "アドベンチャー"),
        new BookData("Treasure Island", "アドベンチャー"),
        new BookData("White Fang", "アドベンチャー"),
        new BookData("Kim", "アドベンチャー"),
        new BookData("Hatchet", "アドベンチャー"),
        new BookData("Stuart Little", "アドベンチャー"),
        new BookData("Robinson", "アドベンチャー"),
        new BookData("Harry", "アドベンチャー"),
        new BookData("The Count", "アドベンチャー"),
        new BookData("Congo", "アドベンチャー"),
        new BookData("Cruel Sea", "アドベンチャー"),
        new BookData("Long Ships", "アドベンチャー"),
        new BookData("Jaws", "アドベンチャー"),
        new BookData("Odyssey", "アドベンチャー"),
        new BookData("Dune", "アドベンチャー"),
        new BookData("Desert", "アドベンチャー"),
        new BookData("Zorro", "アドベンチャー"),
        new BookData("Scaramouche", "アドベンチャー"),
        new BookData("Lord", "アドベンチャー"),
        new BookData("Roads", "アドベンチャー"),

        new BookData("The Higgler", "ロマンス"),
        new BookData("A Mere Interlude", "ロマンス"),
        new BookData("Irish Revel", "ロマンス"),
        new BookData("Twilight", "ロマンス"),
        new BookData("Lolita", "ロマンス"),
        new BookData("Adam Bede", "ロマンス"),
        new BookData("Rebecca", "ロマンス"),
        new BookData("Persuasion", "ロマンス"),
        new BookData("Dear John", "ロマンス"),
        new BookData("Cold Mountain", "ロマンス"),
        new BookData("Wedding", "ロマンス"),
        new BookData("Lucky One", "ロマンス"),
        new BookData("Wings of Dove", "ロマンス"),
        new BookData("Dawn", "ロマンス"),
        new BookData("Notebook", "ロマンス"),

        new BookData("Dancing Partner", "コメディ"),
        new BookData("Big Trouble", "コメディ"),
        new BookData("Catch-22", "コメディ"),
        new BookData("Election", "コメディ"),
        new BookData("Here We Are", "コメディ"),
        new BookData("The Princess Bride ", "コメディ"),
        new BookData("Lumber Room", "コメディ"),
        new BookData("Bossypants", "コメディ"),
        new BookData("Daddy’s Boy", "コメディ"),
        new BookData("American Cornball", "コメディ"),
        new BookData("Good Omens", "コメディ"),
        new BookData("Naked", "コメディ"),
        new BookData("Egg and I", "コメディ"),
        new BookData("Wishful Drinking", "コメディ"),
        new BookData("Zombie Spaceship", "コメディ"),
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